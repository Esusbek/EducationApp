using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogicLayer.Common.MappingProfiles
{
    public partial class MappingProfiles
    {
        public class UserMapProfile: Profile
        {
            public UserMapProfile()
            {
                CreateMap<UserModel, UserEntity>();
                CreateMap<UserEntity, UserModel>();
            }
        }
    }
}
