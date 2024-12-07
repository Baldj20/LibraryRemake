using Domain;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IPagedRepository<T>, IRepository<T> where T : class
    {
        protected readonly LibraryDbContext context;
        public Repository(LibraryDbContext context)
        {
            this.context = context;
        }
        public async Task Add(T entity, CancellationToken token)
        {
            context.Set<T>().Add(entity);
        }

        public async Task Delete(T entity, CancellationToken token)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task<ICollection<T>> GetAll(CancellationToken token)
        {
            var items = await context.Set<T>()
               .AsNoTracking()
               .ToListAsync(token);

            return items;
        }

        public async Task<T?> GetById(Guid id, CancellationToken token)
        {
            if (typeof(T).IsSubclassOf(typeof(EntityWithId)))
            {
                var item = await context.Set<T>().Where(item => ((EntityWithId)(object)item).Id == id).FirstOrDefaultAsync();

                return item;
            }

            throw new Application.Exceptions.NotSupportedException(typeof(T).Name, "GetByID");
        }

        public async Task<PagedResult<T>> GetPaged(PaginationParams paginationParams, CancellationToken token)
        {
            var query = context.Set<T>().AsQueryable();

            var totalItems = await query.CountAsync();

            var itemsQuery = query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize);

            token.ThrowIfCancellationRequested();

            var items = await itemsQuery.ToListAsync();

            token.ThrowIfCancellationRequested();

            var pagedItems = new PagedResult<T>
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

        public async Task Update(T entity, CancellationToken token)
        {
            context.Set<T>().Update(entity);
        }
    }
}
