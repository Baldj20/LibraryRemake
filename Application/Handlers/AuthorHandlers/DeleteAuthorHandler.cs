using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.AuthorHandlers
{
    public class DeleteAuthorHandler : IDeleteAuthorHandler
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteAuthorHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ActionSuccessStatusResponse> Handle(DeleteAuthorUseCase usecase, CancellationToken token)
        {
            var id = usecase.Id;

            token.ThrowIfCancellationRequested();

            var author = await unitOfWork.Authors.GetById(id, token);
            if (author == null) throw new NotFoundException("Author to delete not found");

            token.ThrowIfCancellationRequested();

            await unitOfWork.Authors.Delete(author, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Author deleted successfully"
            };
        }
    }
}
