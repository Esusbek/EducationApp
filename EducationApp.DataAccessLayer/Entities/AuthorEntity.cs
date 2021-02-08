using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorEntity: Base.BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<PrintingEditionEntity> PrintingEditions { get; set; }
    }
}
