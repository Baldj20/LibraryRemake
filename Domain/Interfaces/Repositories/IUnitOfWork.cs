namespace Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        IUserBookRepository UserBooks { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        public Task CompleteAsync(CancellationToken token);
    }
}
