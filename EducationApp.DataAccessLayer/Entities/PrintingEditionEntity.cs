using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEditionEntity: Base.BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Enums.Enums.PrintingEdition.Status Status { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public Enums.Enums.PrintingEdition.Type Type { get; set; }

        public virtual ICollection<AuthorEntity> Authors { get; set; }
    }
}
