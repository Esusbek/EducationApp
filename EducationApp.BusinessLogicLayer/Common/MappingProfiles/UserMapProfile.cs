using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.BusinessLogicLayer.Common.MappingProfiles
{
    public partial class MappingProfiles
    {
        public class UserMapProfile : Profile
        {
            public UserMapProfile()
            {
                CreateMap<UserModel, UserEntity>().
                    ForMember(dest => dest.Id, opt => opt.Ignore());
                CreateMap<UserEntity, UserModel>();
            }
        }
    }
}
