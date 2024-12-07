using Application.DTO.Response;
using Application.Interfaces.Handlers.BookHandlers;
using Application.UseCases.BookUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;

namespace Application.Handlers.BookHandlers
{
    public class GetPagedBooksHandler : IGetPagedBooksHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetPagedBooksHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ICollection<BookResponse>> Handle(GetPagedBooksUseCase usecase, CancellationToken token)
        {
            var paginationParams = usecase.PaginationParams;

            var pagedBooks = await unitOfWork.Books.GetPaged(paginationParams, token);

            return mapper.Map<ICollection<BookResponse>>(pagedBooks.Items);
        }
    }
}
