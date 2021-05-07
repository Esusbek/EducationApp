using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{

    [Dapper.Contrib.Extensions.Table("PrintingEditions")]
    public class PrintingEditionEntity : BaseEntity
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
        [Computed]
        public virtual ICollection<AuthorEntity> Authors { get; set; }
    }
}
