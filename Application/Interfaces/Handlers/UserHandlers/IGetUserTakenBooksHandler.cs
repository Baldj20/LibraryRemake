using Application.DTO.Response;
using Application.UseCases.UserUseCases;

namespace Application.Interfaces.Handlers.UserHandlers
{
    public interface IGetUserTakenBooksHandler
    {
        public Task<ICollection<UserBookResponse>> Handle(GetUserTakenBooksUseCase usecase, CancellationToken token);
    }
}
