using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class CartService : Interfaces.ICartService
    {
        public CartService()
        {}

        public void AddItem(List<OrderItemModel> currentOrder, PrintingEditionModel printingEdition, int amount)
        {
            currentOrder.Add(new OrderItemModel
            {
                PrintingEditionId = printingEdition.Id,
                Amount = amount,
                Price = printingEdition.Price,
                //TODO: Conversion to usd 
                SubTotal = amount * printingEdition.Price,
                Currency = printingEdition.Currency
            });
        }
        public void ChangeAmount(List<OrderItemModel> currentOrder, PrintingEditionModel printingEdition, int newAmount)
        {
            var item = currentOrder.FirstOrDefault(oi => oi.PrintingEditionId == printingEdition.Id);
            if (item is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.PrintingEditionIsNotInCart);
            }
            item.Amount = newAmount;
            //TODO: Conversion to usd 
            item.SubTotal = item.Price * item.Amount;
        }

        public void DeleteItem(List<OrderItemModel> currentOrder, PrintingEditionModel item)
        {
            currentOrder.RemoveAll(oi => oi.PrintingEditionId == item.Id);
        }

        public decimal GetTotal(List<OrderItemModel> currentOrder)
        {
            decimal sum = 0M;
            foreach (var item in currentOrder)
            {
                //TODO: Conversion to usd 
                sum += item.SubTotal;
            }
            return sum;
        }

        
    }
}
