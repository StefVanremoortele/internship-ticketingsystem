using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ticketingsystem.Domain.Helpers;

namespace Ticketingsystem.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task<int> CountAsync();

        Task<RepositoryActionResult<TEntity>> GetAsync(int id);
        Task<RepositoryActionResult<TEntity>> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<RepositoryActionResult<IEnumerable<TEntity>>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<RepositoryActionResult<IEnumerable<TEntity>>> GetAllAsync();
    }
}
