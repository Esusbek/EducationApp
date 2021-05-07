using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Base
{
    public class BaseErrorsModel
    {
        public List<string> Errors;
        public BaseErrorsModel()
        {
            Errors = new List<string>();
        }
    }
}
