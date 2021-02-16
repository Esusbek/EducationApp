using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.BusinessLogicLayer.Common.MappingProfiles
{
    public partial class MappingProfiles
    {
        public class AuthorMapProfile : Profile
        {
            public AuthorMapProfile()
            {
                CreateMap<AuthorModel, AuthorEntity>();
                CreateMap<AuthorEntity, AuthorModel>();
            }
        }
    }
}
