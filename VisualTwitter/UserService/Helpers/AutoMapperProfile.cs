using AutoMapper;
using UserService.DataTransferObjects;
using UserService.Models;

namespace UserService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserRegisterDTO, User>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
