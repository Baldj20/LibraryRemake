using Application.DTO.Response;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.UserHandlers
{
    public class GetPagedUsersHandler : IGetPagedUsersHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetPagedUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<UserResponse>> Handle(GetPagedUsersUseCase usecase, CancellationToken token)
        {
            var paginationParams = usecase.PaginationParams;

            var pagedUsers = await unitOfWork.Users.GetPaged(paginationParams, token);

            return mapper.Map<ICollection<UserResponse>>(pagedUsers.Items);
        }
    }
}
