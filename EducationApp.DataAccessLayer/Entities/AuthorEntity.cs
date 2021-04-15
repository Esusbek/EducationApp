using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table("Authors")]
    public class AuthorEntity : BaseEntity
    {
        public string Name { get; set; }
        [Computed]
        public virtual ICollection<PrintingEditionEntity> PrintingEditions { get; set; }
    }
}
