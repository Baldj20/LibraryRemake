using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces.Handlers.AuthorHandlers;
using Application.UseCases.AuthorUseCases;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAddAuthorHandler addAuthor;
        private readonly IDeleteAuthorHandler deleteAuthor;
        private readonly IGetAllAuthorsHandler getAllAuthors;
        private readonly IGetAuthorBooksHandler getAuthorBooks;
        private readonly IGetAuthorByIdHandler getAuthorById;
        private readonly IUpdateAuthorHandler updateAuthor;
        private readonly IGetPagedAuthorsHandler getPagedAuthors;
        public AuthorController(IAddAuthorHandler addAuthor, IDeleteAuthorHandler deleteAuthor,
            IGetAllAuthorsHandler getAllAuthors, IGetAuthorBooksHandler getAuthorBooks,
            IGetAuthorByIdHandler getAuthorById, IUpdateAuthorHandler updateAuthor,
            IGetPagedAuthorsHandler getPagedAuthors)
        {
            this.addAuthor = addAuthor;
            this.deleteAuthor = deleteAuthor;
            this.getAllAuthors = getAllAuthors;
            this.getAuthorBooks = getAuthorBooks;
            this.getAuthorById = getAuthorById;
            this.updateAuthor = updateAuthor;
            this.getPagedAuthors = getPagedAuthors;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Add(AuthorRequest request, CancellationToken token)
        {
            var useCase = new AddAuthorUseCase
            {
                AuthorRequest = request
            };
            var response = await addAuthor.Handle(useCase, token);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Delete(Guid id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var useCase = new DeleteAuthorUseCase
            {
                Id = id,
            };

            var response = await deleteAuthor.Handle(useCase, token);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorResponse>>> GetAll(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var useCase = new GetAllAuthorsUseCase
            {

            };

            var authors = await getAllAuthors.Handle(useCase, token);
            return Ok(authors);
        }

        [HttpGet("{id}/books")]
        public async Task<ActionResult<List<BookResponse>>> GetBooks([FromRoute(Name = "id")] Guid authorId,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var useCase = new GetAuthorBooksUseCase
            {
                Id = authorId,
            };

            var books = await getAuthorBooks.Handle(useCase, token);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponse>> GetById([FromRoute(Name = "id")] Guid authorId, CancellationToken token)
        {
            var useCase = new GetAuthorByIdUseCase
            {
                Id = authorId,
            };

            var author = await getAuthorById.Handle(useCase, token);
            return Ok(author);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Update(Guid id, AuthorRequest request, CancellationToken token)
        {
            var useCase = new UpdateAuthorUseCase
            {
                Id = id,
                AuthorRequest = request,
            };

            var response = await updateAuthor.Handle(useCase, token);
            return Ok(response);
        }

        [HttpGet("{pageNumber}, {pageSize}")]
        public async Task<ActionResult<List<AuthorResponse>>> GetPaged(int pageNumber, int pageSize, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var useCase = new GetPagedAuthorsUseCase
            {
                PaginationParams = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize },
            };

            var result = await getPagedAuthors.Handle(useCase, token);
            return Ok(result);
        }
    }
}
