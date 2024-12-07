using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IGetAllAuthorsHandler
    {
        public Task<ICollection<AuthorResponse>> Handle(GetAllAuthorsUseCase usecase, CancellationToken token);
    }
}
