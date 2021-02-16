using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.BusinessLogicLayer.Common.MappingProfiles
{
    public partial class MappingProfiles
    {
        public class PrintingEditionMapProfile : Profile
        {
            public PrintingEditionMapProfile()
            {
                CreateMap<PrintingEditionModel, PrintingEditionEntity>();
                CreateMap<PrintingEditionEntity, PrintingEditionModel>();
            }
        }
    }
}
