using System.Linq.Expressions;


namespace Core.IRepositories
{
    public interface IRepository<TEntity>: IDisposable where TEntity : class
    {       
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken = default);

        ValueTask<TEntity> SingleOrDefaultAsync<TKey>(TKey id);

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);


        void Remove(TEntity entity);
        void Remove<TKey>(TKey id);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task<IDisposable> BeginTransaction(CancellationToken cancellationToken = default);
        Task CommitTransaction(CancellationToken cancellationToken = default);
        Task RollbackTransaction(CancellationToken cancellationToken = default);
        Task<int> CommitAsync();
    }
}
