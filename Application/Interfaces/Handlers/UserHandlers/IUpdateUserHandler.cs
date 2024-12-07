using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IUpdateUserHandler
    {
        public Task<ActionSuccessStatusResponse> Handle(UpdateUserUseCase usecase, CancellationToken token);
    }
}
