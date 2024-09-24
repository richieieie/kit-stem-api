using kit_stem_api.Data;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Repository.Base
{
    public class GenericRepository<T> where T : class
    {
        protected readonly KitStemDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(KitStemDbContext context)
        {
            _dbContext ??= context;
            _dbSet = context.Set<T>();
        }
        public List<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
            //return _dbContext.Set<T>().AsNoTracking().ToList();
        }
        public IEnumerable<T> GetFilter(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null)
        {
            IQueryable<T> query = _dbSet;

            // Apply the filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering if provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Apply pagination: Skip and Take values
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return [.. query];
        }
        public bool Create(T entity)
        {
            _dbContext.Add(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public bool Update(T entity)
        {
            var tracker = _dbContext.Attach(entity);
            tracker.State = EntityState.Modified;
            return _dbContext.SaveChanges() > 0;
        }

        public bool Remove(T entity)
        {
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public T? GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T? GetById(string code)
        {
            return _dbContext.Set<T>().Find(code);
        }

        public T? GetById(Guid code)
        {
            return _dbContext.Set<T>().Find(code);
        }

        #region Asynchronous

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetFilterAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null)
        {
            IQueryable<T> query = _dbSet;

            // Apply the filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering if provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Apply pagination: Skip and Take values
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<bool> CreateAsync(T entity)
        {
            _dbContext.Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            var tracker = _dbContext.Attach(entity);
            tracker.State = EntityState.Modified;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(string code)
        {
            return await _dbContext.Set<T>().FindAsync(code);
        }

        public async Task<T?> GetByIdAsync(Guid code)
        {
            return await _dbContext.Set<T>().FindAsync(code);
        }

        #endregion


        #region Separating asigned entities and save operators        

        public void PrepareCreate(T entity)
        {
            _dbContext.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _dbContext.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _dbContext.Remove(entity);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion Separating assign entity and save operators
    }
}
