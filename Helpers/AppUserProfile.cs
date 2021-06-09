using AutoMapper;
using Store_Shared.Dto;
using Store_Shared.Models;

namespace Store_API.Helpers
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<ApplicationUser, RegisterModel>();
            CreateMap<RegisterModel, ApplicationUser>();
            CreateMap<ApplicationUser, AuthModel>();
            CreateMap<ApplicationUser, ApplicationUser>();
            CreateMap<EditUserModel, RegisterModel>();
            CreateMap<ApplicationUser, UserModel>();
        }
    }
}