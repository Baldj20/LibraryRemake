using Application.Exceptions;
using Domain;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context)
            : base(context)
        {

        }
        public new async Task<Author?> GetById(Guid id, CancellationToken token)
        {
            var author = await context.Authors
                .Where(author => author.Id == id)
                .FirstOrDefaultAsync();

            return author;
        }
        public async Task<Author?> GetByIdWithBooks(Guid id, CancellationToken token)
        {
            var author = await context.Authors
                .Where(author => author.Id == id)
                .Select(author => new Author
                {
                    Id = author.Id,
                    Name = author.Name,
                    Surname = author.Surname,
                    BirthDate = author.BirthDate,
                    Country = author.Country,
                    ImagePath = author.ImagePath,
                    Books = author.Books.Select(book => new Book
                    {
                        ISBN = book.ISBN,
                        Title = book.Title,
                        Genre = book.Genre,
                        Description = book.Description,
                        Count = book.Count,
                        AuthorId = author.Id
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return author;
        }
        public new async Task<ICollection<Author>> GetAll(CancellationToken token)
        {
            var authors = await context.Authors
                .AsNoTracking()
                .Select(author => new Author
                {
                    Id = author.Id,
                    Name = author.Name,
                    Surname = author.Surname,
                    BirthDate = author.BirthDate,
                    Country = author.Country,
                    ImagePath = author.ImagePath,
                    Books = author.Books.Select(book => new Book
                    {
                        ISBN = book.ISBN,
                        Title = book.Title,
                        Genre = book.Genre,
                        Description = book.Description,
                        Count = book.Count,
                        AuthorId = author.Id
                    }).ToList()
                })
                .ToListAsync();

            return authors;
        }
        public async Task<ICollection<Book>> GetBooks(Guid authorId, CancellationToken token)
        {
            var author = await context.Authors
                .Where(author => author.Id == authorId)
                .Include(author => author.Books)
                .FirstOrDefaultAsync();

            if (author == null) throw new NotFoundException("Author not found");

            var books = author.Books.Select(book => new Book
            {
                Title = book.Title,
                Genre = book.Genre,
                Description = book.Description,
                Count = book.Count,
                Author = new Author
                {
                    Name = author.Name,
                    Surname = author.Surname,
                    BirthDate = author.BirthDate,
                    Country = author.Country,
                }
            }).ToList();

            return books;
        }
        public async Task<PagedResult<Author>> GetPaged(PaginationParams paginationParams, CancellationToken token)
        {
            var query = context.Authors.AsQueryable();

            var totalItems = await query.CountAsync();

            var itemsQuery = query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize);

            token.ThrowIfCancellationRequested();

            var items = await itemsQuery
                .Select(author => new Author
                {
                    Name = author.Name,
                    Surname = author.Surname,
                    BirthDate = author.BirthDate,
                    Country = author.Country,
                    Books = author.Books.Select(book => new Book
                    {
                        Title = book.Title,
                        Genre = book.Genre,
                        Description = book.Description,
                        Count = book.Count,
                    }).ToList()
                })
                .ToListAsync(token);

            token.ThrowIfCancellationRequested();

            var pagedItems = new PagedResult<Author>
            {
                Items = items,
                TotalCount = totalItems,
                PageSize = paginationParams.PageSize,
                PageNumber = paginationParams.PageNumber,
                TotalPages = totalItems % paginationParams.PageSize == 0 ?
                    totalItems % paginationParams.PageSize :
                    totalItems % paginationParams.PageSize + 1,
            };

            return pagedItems;
        }
    }
}
