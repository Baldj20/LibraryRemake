using Application.DTO.Request;

namespace Application.UseCases.AuthorUseCases
{
    public class UpdateAuthorUseCase
    {
        public Guid Id { get; set; }
        public AuthorRequest AuthorRequest { get; set; }
    }
}
