using Application.DTO.Request;
using Application.DTO.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class UserBookProfile : Profile
    {
        public UserBookProfile()
        {
            CreateMap<UserBook, UserBookResponse>();
            CreateMap<UserBookRequest, UserBook>();
        }
    }
}
