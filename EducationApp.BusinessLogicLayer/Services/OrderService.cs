﻿using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using EducationApp.Shared.Exceptions;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _itemRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPrintingEditionService _printingEditionService;
        private readonly ICurrencyConvertionProvider _currencyConverter;
        private readonly IValidationProvider _validator;
        private readonly IMapper _mapper;
        private readonly UrlConfig _urlConfig;
        public OrderService(IMapper mapper, IPrintingEditionService printingEditionService, IOptions<UrlConfig> urlConfig, ICurrencyConvertionProvider currencyConverter, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IPaymentRepository paymentRepository, IValidationProvider validationProvider)
        {
            _urlConfig = urlConfig.Value;
            _orderRepository = orderRepository;
            _itemRepository = orderItemRepository;
            _paymentRepository = paymentRepository;
            _printingEditionService = printingEditionService;
            _currencyConverter = currencyConverter;
            _mapper = mapper;
            _validator = validationProvider;
        }


        public void PayOrder(string paymentIntentId)
        {
            if (string.IsNullOrWhiteSpace(paymentIntentId))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INVALIDINTENTIDERROR);
            }
            var payment = _paymentRepository.Get(new PaymentFilterModel { TransactionId = paymentIntentId}).FirstOrDefault();
            var filter = new OrderFilterModel
            {
                PaymentId = payment.Id
            };
            var order = _orderRepository.Get(filter).FirstOrDefault();
            order.Status = Enums.OrderStatusType.Paid;
            _orderRepository.Update(order);
        }

        public SessionModel CreateCheckoutSession(OrderModel order)
        {
            _validator.ValidateOrder(order);
            bool isExisting = order.Id != default;
            PaymentEntity payment = isExisting ? new PaymentEntity { Id = order.PaymentId } : null;
            if (!isExisting)
            {
                order.Status = Enums.OrderStatusType.Unpaid;
                order.Date = DateTime.UtcNow;
                payment = new PaymentEntity
                {
                    TransactionId = default
                };
                _paymentRepository.Insert(payment);
            }
            var dbOrder = _mapper.Map<OrderEntity>(order);
            if (!isExisting)
            {
                dbOrder.PaymentId = payment.Id;
                dbOrder.Total = order.CurrentItems.Sum(item => item.Price);
                _orderRepository.Insert(dbOrder);
            }
            var items = new List<SessionLineItemOptions>();
            var editionIds = order.CurrentItems.Select(item => item.PrintingEditionId).ToList();
            var printingEditions = _printingEditionService.GetPrintingEditionsRange(new PrintingEditionFilterModel { EditionIds = editionIds});
            var dbItems = _mapper.Map<List<OrderItemEntity>>(order.CurrentItems);
            dbItems.ForEach(item => item.OrderId = dbOrder.Id);
            foreach (var item in dbItems)
            {
                var printingEdition = printingEditions.Where(edition => edition.Id == item.PrintingEditionId).FirstOrDefault();
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = printingEdition.Price * 100,
                        Currency = item.Currency.ToString(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = printingEdition.Title,
                            Description = printingEdition.Description
                        }
                    },
                    Quantity = item.Amount
                };
                items.Add(lineItem);
            }
            if (!isExisting)
            {
                _itemRepository.InsertRange(dbItems);
            }
            string successUrl = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.STRIPESUCCESSPATH
            }.ToString();
            string cancelUrl = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.STRIPECANCELPATH
            }.ToString();
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    Constants.DEFAULTPAYMENTMETHOD
                },
                LineItems = items,
                Mode = Constants.DEFAULTPAYMENTMODE,
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };
            var service = new SessionService();
            Session session = service.Create(options);

            payment.TransactionId = session.PaymentIntentId;
            _paymentRepository.Update(payment);
            return new SessionModel
            {
                Id = session.Id,
                PaymentIntentId = session.PaymentIntentId
            };
        }
        public int GetLastPageUser(string userId)
        {
            var filter = new OrderFilterModel
            {
                UserId = userId
            };
            var dbOrders = _orderRepository.GetAll(filter).ToList();
            int lastPage = (int)Math.Ceiling(dbOrders.Count / (double)Constants.ORDERPAGESIZE);
            return lastPage;
        }
        public int GetLastPage(bool getPaid = true, bool getUnpaid = true)
        {
            var filter = new OrderFilterModel
            {
                GetPaid = getPaid,
                GetUnpaid = getUnpaid
            };
            var dbOrders = _orderRepository.GetAll(filter).ToList();
            int lastPage = (int)Math.Ceiling(dbOrders.Count / (double)Constants.ORDERPAGESIZE);
            return lastPage;
        }
        public List<OrderModel> GetAllOrders(bool getPaid = true, bool getUnpaid = true, string field = null, string ascending = Constants.DEFAULTSORTORDER, int page = Constants.DEFAULTPAGE, bool getRemoved = true)
        {
            var filter = new OrderFilterModel
            {
                GetPaid = getPaid,
                GetUnpaid = getUnpaid
            };
            var dbOrders = _orderRepository.Get(filter, field, ascending == Constants.DEFAULTSORTORDER, getRemoved, page);
            var orders = new List<OrderModel>();
            var orderIds = dbOrders.Select(order => order.Id).ToList();
            var allItems = _itemRepository.Get(new OrderItemFilterModel { OrderIds = orderIds});
            foreach (var order in dbOrders)
            {
                var mappedOrder = _mapper.Map<OrderModel>(order);
                var currentItems = allItems.Where(item => item.OrderId == order.Id).ToList();
                var mappedItems = _mapper.Map<List<OrderItemModel>>(currentItems);
                mappedOrder.CurrentItems = mappedItems;
                mappedOrder.Total = currentItems.Sum(item => item.SubTotal);
                orders.Add(mappedOrder);
            }
            return orders;
        }
        public OrderResponseModel GetUserOrders(UserModel user, int page = Constants.DEFAULTPAGE)
        {
            var filter = new OrderFilterModel { 
                UserId=user.Id
            };
            var dbOrders = _orderRepository.Get(filter, page: page).ToList();
            var orders = new List<OrderModel>();
            var orderIds = dbOrders.Select(order => order.Id).ToList();
            var allItems = _itemRepository.Get(new OrderItemFilterModel { OrderIds = orderIds });
            foreach (var order in dbOrders)
            {
                var mappedOrder = _mapper.Map<OrderModel>(order);
                var currentItems = allItems.Where(item => item.OrderId == order.Id).ToList();
                mappedOrder.CurrentItems = _mapper.Map<List<OrderItemModel>>(currentItems);
                mappedOrder.Total = currentItems.Sum(item => item.SubTotal);
                orders.Add(mappedOrder);
            }
            return new OrderResponseModel
            {
                Orders = orders,
                LastPage = GetLastPageUser(user.Id)
            };

        }

        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            return await _currencyConverter.ConvertAsync(fromCurrency, toCurrency, amount);
        }
    }
}
