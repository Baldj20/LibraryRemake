using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(LibraryDbContext context) 
            : base(context)
        {

        }

        public async Task<User?> GetByLogin(string login)
        {
            var user = await context.Users
                .Where(user => user.Login == login)
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<User?> GetByLoginWithBooks(string login)
        {
            var user = await context.Users
                .Where(user => user.Login == login)
                .AsNoTracking()
                .Select(user => new User
                {
                    Login = user.Login,
                    Role = user.Role,
                    TakenBooks = user.TakenBooks.Select(book => new UserBook
                    {
                        Book = book.Book,
                        ReceiptDate = book.ReceiptDate,
                        ReturnDate = book.ReturnDate,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return user;
        }
        public async new Task<ICollection<User>> GetAll(CancellationToken token)
        {
            var items = await context.Users
               .AsNoTracking()
               .Select(user => new User
               {
                   Login = user.Login,
                   Role = user.Role,
                   TakenBooks = user.TakenBooks.Select(book => new UserBook
                   {
                       Book = book.Book,
                       ReceiptDate = book.ReceiptDate,
                       ReturnDate = book.ReturnDate,
                   }).ToList()
               })
               .ToListAsync(token);

            return items;
        }
    }
}
