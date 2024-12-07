using Application.DTO.Request;
using Application.DTO.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Login,
                 opt => opt.MapFrom(src => src.UserLogin))
                .ForMember(dest => dest.HashedPassword,
                 opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
                .ForMember(dest => dest.TakenBooks, opt => opt.Ignore());
            CreateMap<AddUserRequest, User>()
                .ForMember(dest => dest.Login,
                 opt => opt.MapFrom(src => src.UserLogin))
                .ForMember(dest => dest.HashedPassword,
                 opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
                .ForMember(dest => dest.TakenBooks, opt => opt.Ignore());
        }
    }
}
