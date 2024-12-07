using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context)
            :base(context)
        {
            
        }
        public new async Task<ICollection<Book>> GetAll(CancellationToken token)
        {
            var items = await context.Books
                .Select(book => new Book
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Genre = book.Genre,
                    Description = book.Description,
                    Count = book.Count,
                    Author = new Author
                    {
                        Name = book.Author.Name,
                        Surname = book.Author.Surname,
                        Country = book.Author.Country,
                        BirthDate = book.Author.BirthDate,
                    }
                })
               .ToListAsync(token);

            return items;
        }
        public async Task<Book?> GetByISBN(string isbn)
        {
            var book = await context.Books
                .Where(item => item.ISBN == isbn)
                .Select(book => new Book
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Genre = book.Genre,
                    Description = book.Description,
                    Count = book.Count,
                    Author = new Author
                    {
                        Id = book.Author.Id,
                        Name = book.Author.Name,
                        Surname = book.Author.Surname,
                        Country = book.Author.Country,
                        BirthDate = book.Author.BirthDate,
                    }
                })
                .FirstOrDefaultAsync();

            return book;
        }
    }
}
