using ApiEcommerce_VS.Models;
using ApiEcommerce_VS.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce_VS.Mapping
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserLoginResponseDto>().ReverseMap();
        }
    }
}
