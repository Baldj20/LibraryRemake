using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.AuthorHandlers
{
    public class GetAllAuthorsHandler : IGetAllAuthorsHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllAuthorsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<AuthorResponse>> Handle(GetAllAuthorsUseCase usecase, CancellationToken token)
        {
            var authors = await unitOfWork.Authors.GetAll(token);

            if (authors.Count == 0) throw new NotFoundException("Authors not found");

            token.ThrowIfCancellationRequested();

            return mapper.Map<ICollection<AuthorResponse>>(authors);
        }
    }
}
