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
        public bool IsBook { get; set; }
        public bool IsNewspaper { get; set; }
        public bool IsJournal { get; set; }
        public List<int> EditionIds { get; set; }
        public PrintingEditionFilterModel()
        {
            Title = string.Empty;
            LowPrice = default;
            HighPrice = default;
            Type = new List<Enums.PrintingEditionType>();
            IsBook = true;
            IsNewspaper = true;
            IsJournal = true;
            EditionIds = new List<int>();
        }
    }
}
