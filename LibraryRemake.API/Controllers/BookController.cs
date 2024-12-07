using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces.Handlers.BookHandlers;
using Application.UseCases.BookUseCases;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IAddBookHandler addBook;
        private readonly IDeleteBookHandler deleteBook;
        private readonly IGetAllBooksHandler getAllBooks;
        private readonly IGetBookByISBNHandler getBookByISBN;
        private readonly IUpdateBookHandler updateBook;
        private readonly IGetPagedBooksHandler getPagedBooks;
        public BookController(IAddBookHandler addBook, IDeleteBookHandler deleteBook,
            IGetAllBooksHandler getAllBooks, IGetBookByISBNHandler getBookByISBN,
            IUpdateBookHandler updateBook, IGetPagedBooksHandler getPagedBooks)
        {
            this.addBook = addBook;
            this.deleteBook = deleteBook;
            this.getAllBooks = getAllBooks;
            this.getBookByISBN = getBookByISBN;
            this.updateBook = updateBook;
            this.getPagedBooks = getPagedBooks;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Add(BookRequest request, CancellationToken token)
        {
            var useCase = new AddBookUseCase
            {
                BookRequest = request,
            };

            var response = await addBook.Handle(useCase, token);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{isbn}")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Delete(string isbn, CancellationToken token)
        {
            var useCase = new DeleteBookUseCase
            {
                ISBN = isbn,
            };

            var response = await deleteBook.Handle(useCase, token);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<BookResponse>>> GetAll(CancellationToken token)
        {
            var useCase = new GetAllBooksUseCase
            {

            };

            var books = await getAllBooks.Handle(useCase, token);
            return Ok(books);
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<BookResponse>> GetByISBN(string isbn)
        {
            var useCase = new GetBookByISBNUseCase
            {
                ISBN = isbn
            };

            var book = await getBookByISBN.Handle(useCase);
            return Ok(book);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Update(BookRequest request, CancellationToken token)
        {
            var useCase = new UpdateBookUseCase
            {
                BookRequest = request,
            };

            var response = await updateBook.Handle(useCase, token);
            return Ok(response);
        }

        [HttpGet("{pageNumber}, {pageSize}")]
        public async Task<ActionResult<List<BookResponse>>> GetPaged(int pageNumber, int pageSize, CancellationToken token)
        {
            var useCase = new GetPagedBooksUseCase
            {
                PaginationParams = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize },
            };

            var result = await getPagedBooks.Handle(useCase, token);
            return Ok(result);
        }
    }
}
