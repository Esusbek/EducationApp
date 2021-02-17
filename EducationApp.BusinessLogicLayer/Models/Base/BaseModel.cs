using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Base
{
    public class BaseModel
    {
        public int Id { get; set; }
        public IEnumerable<string> Errors;
        public BaseModel()
        {
            Errors = new List<string>();
        }
    }
}
