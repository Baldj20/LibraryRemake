using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IDeleteUserHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(DeleteUserUseCase usecase, CancellationToken token);
    }
}
