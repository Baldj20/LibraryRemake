namespace Domain.Interfaces.Repositories
{
    public interface IPagedRepository<T>
    {
        public Task<PagedResult<T>> GetPaged(PaginationParams paginationParams, CancellationToken token);
    }
}
