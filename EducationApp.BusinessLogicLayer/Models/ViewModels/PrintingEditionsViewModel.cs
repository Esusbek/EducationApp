﻿using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class PrintingEditionsViewModel
    {
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public bool GetBook { get; set; }
        public bool GetNewspaper { get; set; }
        public bool GetJournal { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }
        public List<AuthorModel> Authors { get; set; }
        public PrintingEditionsViewModel()
        {
            CurrentPage = Constants.DEFAULTPAGE;
            GetBook = true;
            GetNewspaper = true;
            GetJournal = true;
            SortBy = Constants.DEFAULTEDITIONSORT;
            Ascending = true;
            Authors = new List<AuthorModel>();
        }
    }
}