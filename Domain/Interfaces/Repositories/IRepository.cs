namespace Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        public Task<ICollection<T>> GetAll(CancellationToken token);
        public Task Add(T entity, CancellationToken token);
        public Task Update(T entity, CancellationToken token);
        public Task Delete(T entity, CancellationToken token);
        public Task<T?> GetById(Guid id, CancellationToken token);
    }
}
