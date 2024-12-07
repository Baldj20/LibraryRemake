using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.UserHandlers
{
    public class DeleteUserHandler : IDeleteUserHandler
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ActionSuccessStatusResponse> Handle(DeleteUserUseCase usecase, CancellationToken token)
        {
            var login = usecase.Login;

            var user = await unitOfWork.Users.GetByLogin(login);

            if (user == null) throw new NotFoundException("User to delete not found");

            token.ThrowIfCancellationRequested();

            await unitOfWork.Users.Delete(user, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "User deleted successfully"
            };
        }
    }
}
