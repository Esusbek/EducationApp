using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class PrintingEditionsViewModel
    {
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public int Page { get; set; }
        public int LastPage { get; set; }
        public bool IsBook { get; set; }
        public bool IsNewspaper { get; set; }
        public bool IsJournal { get; set; }
        public string SortBy { get; set; }
        public string Ascending { get; set; }
        public List<AuthorModel> Authors { get; set; }
        public PrintingEditionsViewModel()
        {
            Page = Constants.DEFAULTPAGE;
            IsBook = true;
            IsNewspaper = true;
            IsJournal = true;
            SortBy = Constants.DEFAULTEDITIONSORT;
            Ascending = Constants.DEFAULTSORTORDER;
            Authors = new List<AuthorModel>();
        }
    }
}
