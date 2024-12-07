using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IGetPagedUsersHandler
    {
        public Task<ICollection<UserResponse>> Handle(GetPagedUsersUseCase usecase, CancellationToken token);
    }
}
