using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class AuthorsViewModel
    {
        public List<AuthorModel> Authors { get; set; }
        public int Page { get; set; }
        public int LastPage { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }
        public AuthorsViewModel()
        {
            Page = Constants.DEFAULTPAGE;
            SortBy = Constants.DEFAULTAUTHORSORT;
            Ascending = true;
        }
    }
}
