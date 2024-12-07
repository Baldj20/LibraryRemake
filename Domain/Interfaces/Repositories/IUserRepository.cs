using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>, IPagedRepository<User>
    {
        public Task<User?> GetByLogin(string login);
        public Task<User?> GetByLoginWithBooks(string login);
    }
}
