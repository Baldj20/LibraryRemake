using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.UserHandlers
{
    public class GetUserByLoginHandler : IGetUserByLoginHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetUserByLoginHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<UserResponse> Handle(GetUserByLoginUseCase usecase, CancellationToken token)
        {
            var login = usecase.Login;

            var user = await unitOfWork.Users.GetByLogin(login);

            if (user == null) throw new NotFoundException("User with this login not found");

            return mapper.Map<UserResponse>(user);
        }
    }
}
