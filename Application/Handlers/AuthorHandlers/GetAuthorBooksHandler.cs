using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.AuthorHandlers
{
    public class GetAuthorBooksHandler : IGetAuthorBooksHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetAuthorBooksHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<BookResponse>> Handle(GetAuthorBooksUseCase usecase, CancellationToken token)
        {
            var id = usecase.Id;
            token.ThrowIfCancellationRequested();

            var books = await unitOfWork.Authors.GetBooks(id, token);

            if (books.Count == 0) throw new NotFoundException("Author does not have books");

            return mapper.Map<ICollection<BookResponse>>(books);
        }
    }
}
