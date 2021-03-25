using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItemEntity : BaseEntity
    {
        public int Amount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal SubTotal { get; set; }

        public Enums.CurrencyType Currency { get; set; }
        public int OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
        public int PrintingEditionId { get; set; }
        public virtual PrintingEditionEntity PrintingEdition { get; set; }
    }
}
