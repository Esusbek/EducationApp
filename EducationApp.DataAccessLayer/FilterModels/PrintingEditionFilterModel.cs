using EducationApp.Shared.Enums;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.FilterModels
{
    public class PrintingEditionFilterModel
    {
        public string Title { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }
        public List<Enums.PrintingEditionType> Type { get; set; }
        public bool GetBook { get; set; }
        public bool GetNewspaper { get; set; }
        public bool GetJournal { get; set; }
        public List<int> EditionIds { get; set; }
        public PrintingEditionFilterModel()
        {
            Title = "";
            LowPrice = default;
            HighPrice = default;
            Type = new List<Enums.PrintingEditionType>();
            GetBook = true;
            GetNewspaper = true;
            GetJournal = true;
            EditionIds = new List<int>();
        }
    }
}
