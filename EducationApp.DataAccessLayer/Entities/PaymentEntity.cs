using EducationApp.DataAccessLayer.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table("Payments")]
    public class PaymentEntity : BaseEntity
    {
        public string TransactionId { get; set; }
    }
}
