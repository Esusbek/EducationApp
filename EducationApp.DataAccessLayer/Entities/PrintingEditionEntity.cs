using EducationApp.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEditionEntity : Base.BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        [Required]
        public string SubTitle { get; set; }

        public Enums.PrintingEditionStatusType Status { get; set; }
        public Enums.CurrencyType Currency { get; set; }
        public Enums.PrintingEditionType Type { get; set; }

        public virtual ICollection<AuthorEntity> Authors { get; set; }
    }
}
