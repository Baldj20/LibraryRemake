using Application.DTO.Response;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.AuthorHandlers
{
    public class GetPagedAuthorsHandler : IGetPagedAuthorsHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetPagedAuthorsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ICollection<AuthorResponse>> Handle(GetPagedAuthorsUseCase usecase, CancellationToken token)
        {
            var paginationParams = usecase.PaginationParams;

            token.ThrowIfCancellationRequested();

            var pagedAuthors = await unitOfWork.Authors.GetPaged(paginationParams, token);

            return mapper.Map<ICollection<AuthorResponse>>(pagedAuthors.Items);
        }
    }
}
