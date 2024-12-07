using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IGetUserByLoginHandler
    {
        public Task<UserResponse> Handle(GetUserByLoginUseCase usecase, CancellationToken token);
    }
}
