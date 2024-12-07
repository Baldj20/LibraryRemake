using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.UserHandlers
{
    public class GetUserTakenBooksHandler : IGetUserTakenBooksHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetUserTakenBooksHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<UserBookResponse>> Handle(GetUserTakenBooksUseCase usecase, CancellationToken token)
        {
            var login = usecase.Login;

            var user = await unitOfWork.Users.GetByLoginWithBooks(login);
            if (user == null) throw new NotFoundException("Requested user not found");

            return mapper.Map<ICollection<UserBookResponse>>(user.TakenBooks);
        }
    }
}
