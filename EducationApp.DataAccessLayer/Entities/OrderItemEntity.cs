using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    [Dapper.Contrib.Extensions.Table("OrderItems")]
    public class OrderItemEntity : BaseEntity
    {
        public int Amount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal SubTotal { get; set; }

        public Enums.CurrencyType Currency { get; set; }
        public int OrderId { get; set; }
        [Computed]
        public virtual OrderEntity Order { get; set; }
        public int PrintingEditionId { get; set; }
        [Computed]
        public virtual PrintingEditionEntity PrintingEdition { get; set; }
    }
}
