using Application.DTO.Request;
using Application.DTO.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookResponse>();
            CreateMap<BookRequest, Book>()
                .ForMember(dest => dest.TakenBooks, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore());
        }
    }
}
