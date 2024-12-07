using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext context;
        public IUserRepository Users { get; private set; }
        public IBookRepository Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public IUserBookRepository UserBooks { get; private set; }
        public IRefreshTokenRepository RefreshTokens { get; private set; }

        public UnitOfWork(LibraryDbContext context)
        {
            this.context = context;

            Users = new UserRepository(context);
            Books = new BookRepository(context);
            Authors = new AuthorRepository(context);
            UserBooks = new UserBookRepository(context);
            RefreshTokens = new RefreshTokenRepository(context);
        }

        public async Task CompleteAsync(CancellationToken token)
        {
            await context.SaveChangesAsync(token);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
