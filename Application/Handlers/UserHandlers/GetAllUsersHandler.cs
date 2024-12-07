using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.UserHandlers
{
    public class GetAllUsersHandler : IGetAllUsersHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<UserResponse>> Handle(GetAllUsersUseCase usecase, CancellationToken token)
        {
            var users = await unitOfWork.Users.GetAll(token);           

            if (users.Count == 0) throw new NotFoundException("Users not found");

            return mapper.Map<ICollection<UserResponse>>(users);
        }
    }
}
