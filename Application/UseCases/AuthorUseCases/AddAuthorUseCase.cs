using Application.DTO.Request;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.AuthorUseCases
{
    public class AddAuthorUseCase
    {
        public AuthorRequest AuthorRequest { get; set; }
    }
}
