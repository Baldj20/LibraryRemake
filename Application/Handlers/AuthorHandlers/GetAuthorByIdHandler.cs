using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.AuthorHandlers
{
    public class GetAuthorByIdHandler : IGetAuthorByIdHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAuthorByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<AuthorResponse> Handle(GetAuthorByIdUseCase usecase, CancellationToken token)
        {
            var id = usecase.Id;

            var author = await unitOfWork.Authors.GetByIdWithBooks(id, token);

            if (author == null) throw new NotFoundException("Author with this id does not exist");

            return mapper.Map<AuthorResponse>(author);
        }
    }
}
