using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.BookHandlers;
using Application.UseCases.BookUseCases;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.BookHandlers
{
    public class DeleteBookHandler : IDeleteBookHandler
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteBookHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ActionSuccessStatusResponse> Handle(DeleteBookUseCase usecase, CancellationToken token)
        {
            var ISBN = usecase.ISBN;

            token.ThrowIfCancellationRequested();

            var book = await unitOfWork.Books.GetByISBN(ISBN);
            if (book == null) throw new NotFoundException("Book to delete not found");

            token.ThrowIfCancellationRequested();

            await unitOfWork.Books.Delete(book, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Book deleted successfully"
            };
        }
    }
}
