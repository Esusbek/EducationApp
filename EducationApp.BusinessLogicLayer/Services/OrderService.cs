using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class OrderService: IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderItemRepository _itemRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly IPrintingEditionService _printingEditionService;
        private readonly IMapper _mapper;
        public OrderService(ApplicationContext context, IMapper mapper, IPrintingEditionService printingEditionService)
        {
            _orderRepository = new OrderRepository(context);
            _itemRepository = new OrderItemRepository(context);
            _paymentRepository = new PaymentRepository(context);
            _printingEditionService = printingEditionService;
            _mapper = mapper;
        }
        
        public void ConfirmOrder(OrderModel currentOrder, UserModel user)
        {
            currentOrder.Status = Enums.Order.Status.Unpaid;
            currentOrder.Date = DateTime.Now;
            currentOrder.UserId = user.Id.ToString();
            var dbOrder = _mapper.Map<OrderEntity>(currentOrder);
            _orderRepository.Insert(dbOrder);
            dbOrder = _orderRepository.Get(o => o.Date == dbOrder.Date && o.UserId == dbOrder.UserId).FirstOrDefault();
            foreach (var item in currentOrder.CurrentItems)
            {
                var dbItem = _mapper.Map<OrderItemEntity>(item);
                dbItem.Order = dbOrder;
                _itemRepository.Insert(dbItem);
            }
        }

        public void PayOrder(OrderModel order, string transactionId)
        {
            var payment = new PaymentEntity
            {
                TransactionId = transactionId
            };
            _paymentRepository.Insert(payment);
            var paymentId = _paymentRepository.Get(p => p.TransactionId == transactionId).FirstOrDefault().Id;
            var dbOrder = _orderRepository.GetById(order.Id);
            dbOrder.PaymentId = paymentId;
            dbOrder.Status = Enums.Order.Status.Paid;
            _orderRepository.Update(dbOrder);
        }

        public string CreateCheckoutSession(OrderModel order)
        {
            var items = new List<SessionLineItemOptions>();
            foreach (var item in order.CurrentItems)
            {
                var printingEdition = _printingEditionService.GetPrintingEdition(item.PrintingEditionId);
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = item.Price,
                        Currency = item.Currency.ToString("g"),
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
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    Constants.Defaults.DefaultPaymentMethod
                },
                LineItems = items,
                Mode = Constants.Defaults.DefaultPaymentMode,
                SuccessUrl = Constants.Urls.StripeSuccessUrl,
                CancelUrl = Constants.Urls.StripeCancelUrl
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return session.Id;
        }

        public List<OrderModel> GetAllOrders(int page=Constants.Defaults.DefaultPage)
        {
            var dbOrders = _orderRepository.GetAll(page).ToList();
            var orders = new List<OrderModel>();
            foreach (var order in dbOrders)
            {
                var mappedOrder = _mapper.Map<OrderModel>(order);
                var currentItems = _itemRepository.Get(oi => oi.OrderId == order.Id).ToList();
                var mappedItems = MapItems(currentItems);
                mappedOrder.CurrentItems = mappedItems;
                orders.Add(mappedOrder);
            }
            return orders;
        }
        public List<OrderModel> GetUserOrders(UserModel user, int page = Constants.Defaults.DefaultPage)
        {
            var dbOrders = _orderRepository.Get(o => o.UserId == user.Id.ToString(), page: page).ToList();
            var orders = new List<OrderModel>();
            foreach (var order in dbOrders)
            {
                var mappedOrder = _mapper.Map<OrderModel>(order);
                var currentItems = _itemRepository.Get(oi => oi.OrderId == order.Id).ToList();
                var mappedItems = MapItems(currentItems);
                mappedOrder.CurrentItems = mappedItems;
                orders.Add(mappedOrder);
            }
            return orders;
        }
        private List<OrderItemModel> MapItems(List<OrderItemEntity> items)
        {
            var result = new List<OrderItemModel>();
            foreach (var item in items)
            {
                var mappedItem = _mapper.Map<OrderItemModel>(item);
                result.Add(mappedItem);
            }
            return result;
        }
    }
}
