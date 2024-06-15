using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<TEntity>().AsNoTracking().CountAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return Context.Set<TEntity>().AnyAsync(cancellationToken);
        }

        public ValueTask<TEntity> SingleOrDefaultAsync<TKey>(TKey id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<TEntity>().ToListAsync(cancellationToken);
        }


        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }


        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void Remove<TKey>(TKey id)
        {
            TEntity entity = Context.Set<TEntity>().Find(id);
            Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }


        public async Task<IDisposable> BeginTransaction(CancellationToken cancellationToken = default) =>
           await Context.Database.BeginTransactionAsync(cancellationToken);

        public Task CommitTransaction(CancellationToken cancellationToken = default) =>
            Context.Database.CommitTransactionAsync(cancellationToken);

        public Task RollbackTransaction(CancellationToken cancellationToken = default) =>
            Context.Database.RollbackTransactionAsync(cancellationToken);

        public async Task<int> CommitAsync() => await Context.SaveChangesAsync();

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
