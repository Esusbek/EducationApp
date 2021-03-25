using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
