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
                    ForMember(destination => destination.Id, option => option.Ignore()).
                    ForMember(destination => destination.ProfilePictureStorageName, option => option.Ignore()).
                    ForMember(destination => destination.ProfilePictureURL, option => option.Ignore());
                CreateMap<UserEntity, UserModel>();
            }
        }
    }
}
