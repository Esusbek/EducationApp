using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Base
{
    public class BaseModel
    {
        public ICollection<string> Errors = new List<string>();
    }
}
