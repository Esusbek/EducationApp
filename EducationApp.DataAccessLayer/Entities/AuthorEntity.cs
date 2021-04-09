using EducationApp.DataAccessLayer.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table ("Authors")]
    public class AuthorEntity : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<PrintingEditionEntity> PrintingEditions { get; set; }
    }
}
