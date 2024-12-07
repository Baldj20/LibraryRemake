using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IGetAllUsersHandler
    {
        public Task<ICollection<UserResponse>> Handle(GetAllUsersUseCase usecase, CancellationToken token);
    }
}
