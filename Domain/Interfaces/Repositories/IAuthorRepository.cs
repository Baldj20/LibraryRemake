using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IAuthorRepository : IRepository<Author>, IPagedRepository<Author>
    {
        public Task<ICollection<Book>> GetBooks(Guid authorId, CancellationToken token);
        public Task<Author?> GetByIdWithBooks(Guid id, CancellationToken token);
    }
}
