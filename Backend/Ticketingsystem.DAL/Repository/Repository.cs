using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketingsystem.Domain.Helpers;
using Ticketingsystem.Domain.Interfaces.Repository;

namespace Ticketingsystem.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }


        public virtual void Add(TEntity entity)
        {
            _entities.Add(entity);

        }
        public virtual async void AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);                 
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }
        public virtual void AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _entities.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity);
        }
        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }
    
        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }


        public async Task<int> CountAsync()
        {
            try
            {
                return await _entities.CountAsync();
            }
            catch (Exception)
            {
                return -1;
            }
        }               
        public async Task<RepositoryActionResult<TEntity>> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = await _entities.SingleOrDefaultAsync(predicate);

                if (entity == null)
                    return new RepositoryActionResult<TEntity>(entity, RepositoryActionStatus.NotFound);

                return new RepositoryActionResult<TEntity>(entity, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<TEntity>(null, RepositoryActionStatus.Error, ex);
            }
        }
        public async Task<RepositoryActionResult<TEntity>> GetAsync(int id)
        {
            try
            {
                var entity = await _entities.FindAsync(id);
                if (entity == null) 
                    return new RepositoryActionResult<TEntity>(entity, RepositoryActionStatus.NotFound);
                else
                    return new RepositoryActionResult<TEntity>(entity, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<TEntity>(null, RepositoryActionStatus.Error, ex);
            }
        }
        public async Task<RepositoryActionResult<IEnumerable<TEntity>>> GetAllAsync()
        {
            try
            {
                //return await _entities.ToListAsync();
                var entities = await _entities.ToListAsync();

                if (!entities.Any()) 
                    return new RepositoryActionResult<IEnumerable<TEntity>>(entities, RepositoryActionStatus.NotFound);

                return new RepositoryActionResult<IEnumerable<TEntity>>(entities, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<TEntity>>(null, RepositoryActionStatus.NotFound, ex);
            }
        }
        public async Task<RepositoryActionResult<IEnumerable<TEntity>>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entities = await _entities.Where(predicate).ToListAsync();

                if (entities == null)
                    return new RepositoryActionResult<IEnumerable<TEntity>>(entities, RepositoryActionStatus.NotFound);

                return new RepositoryActionResult<IEnumerable<TEntity>>(entities, RepositoryActionStatus.Ok);
            }
            catch (Exception ex)
            {
                return new RepositoryActionResult<IEnumerable<TEntity>>(null, RepositoryActionStatus.Error, ex);
            }
        }

    }       
}
