using Application.DTO.Response;
using Application.UseCases.AuthorUseCases;

namespace Application.Interfaces.Handlers.AuthorHandlers
{
    public interface IGetPagedAuthorsHandler
    {
        public Task<ICollection<AuthorResponse>> Handle(GetPagedAuthorsUseCase usecase, CancellationToken token);
    }
}
