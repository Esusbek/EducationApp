using EducationApp.BusinessLogicLayer.Models.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class AuthorsViewModel
    {
        public List<AuthorModel> Authors { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
    }
}
