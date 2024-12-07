using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBookRepository : IRepository<Book>, IPagedRepository<Book>
    {
        public Task<Book?> GetByISBN(string isbn);
    }
}
