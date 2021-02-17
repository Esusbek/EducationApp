using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface ICartService
    {
        public void AddItem(List<OrderItemModel> currentOrder, PrintingEditionModel printingEdition, int amount);
        public void ChangeAmount(List<OrderItemModel> currentOrder, PrintingEditionModel printingEdition, int newAmount);
        public void DeleteItem(List<OrderItemModel> currentOrder, PrintingEditionModel item);
        public decimal GetTotal(List<OrderItemModel> currentOrder);
    }
}
