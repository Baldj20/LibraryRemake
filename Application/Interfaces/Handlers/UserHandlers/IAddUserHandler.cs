using Application.DTO.Response;
using Application.UseCases.BookUseCases;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IAddUserHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(AddUserUseCase usecase, CancellationToken token);
    }
}
