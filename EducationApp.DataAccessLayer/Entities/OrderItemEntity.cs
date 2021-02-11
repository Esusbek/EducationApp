using EducationApp.Shared.Enums;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItemEntity : Base.BaseEntity
    {
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int PrintingEditionId { get; set; }
        public decimal SubTotal { get; set; }

        public Enums.Currency Currency { get; set; }
        public OrderEntity Order { get; set; }
        public PrintingEditionEntity PrintingEdition { get; set; }
    }
}
