using EducationApp.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEditionEntity : Base.BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        public Enums.PrintingEdition.Status Status { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEdition.Type Type { get; set; }

        public virtual ICollection<AuthorEntity> Authors { get; set; }
    }
}
