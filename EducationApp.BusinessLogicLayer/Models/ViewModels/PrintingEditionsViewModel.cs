using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class PrintingEditionsViewModel
    {
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
    }
}
