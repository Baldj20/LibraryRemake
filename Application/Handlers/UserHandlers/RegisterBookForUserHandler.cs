using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation;

namespace Application.Handlers.UserHandlers
{
    public class RegisterBookForUserHandler : IRegisterBookForUserHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public RegisterBookForUserHandler(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ActionSuccessStatusResponse> Handle(RegisterBookForUserUseCase usecase, CancellationToken token)
        {
            var userLogin = usecase.UserLogin;
            var bookISBN = usecase.BookISBN;
            var receiptDate = usecase.ReceiptDate;
            var returnDate = usecase.ReturnDate;

            var book = await unitOfWork.Books.GetByISBN(bookISBN);
            if (book == null) throw new NotFoundException("Book with given ISBN not found");

            if (book.Count == 0) throw new BookIsOutException();

            token.ThrowIfCancellationRequested();

            var user = await unitOfWork.Users.GetByLogin(userLogin);
            if (user == null) throw new NotFoundException("User with given login not found");

            user.TakenBooks.Add(new UserBook
            {
                BookISBN = bookISBN,
                UserLogin = userLogin,
                ReceiptDate = receiptDate,
                ReturnDate = returnDate,
            });

            book.Count--;

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "Book registered successfully"
            };
        }
    }
}
