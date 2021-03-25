using EducationApp.DataAccessLayer.Entities.Base;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorEntity : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<PrintingEditionEntity> PrintingEditions { get; set; }
    }
}
