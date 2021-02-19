using EducationApp.Shared.Enums;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderItemModel : Base.BaseModel
    {
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int PrintingEditionId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Price { get; set; }

        public Enums.CurrencyType Currency { get; set; }
    }
}
