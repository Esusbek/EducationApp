using EducationApp.BusinessLogicLayer.Models.Base;
using EducationApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationApp.BusinessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionModel : BaseModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0.0, double.PositiveInfinity)]
        public decimal Price { get; set; }

        [Required]
        public Enums.PrintingEditionStatusType Status { get; set; }
        [Required]
        public Enums.CurrencyType Currency { get; set; }
        [Required]
        public Enums.PrintingEditionType Type { get; set; }

        [Required]
        public List<string> Authors { get; set; }
    }
}
