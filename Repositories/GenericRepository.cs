using kit_stem_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace kit_stem_api.Repositories
{
    public abstract class GenericRepository<T> where T : class
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
        public (IEnumerable<T>, int) GetFilter(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null,
            params Func<IQueryable<T>, IQueryable<T>>[]? includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

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

            return (query.ToList(), CountTotalPages(filter, take));
        }

        private int CountTotalPages(Expression<Func<T, bool>>? filter, int? take)
        {
            IQueryable<T> query = _dbSet;

            // Apply the filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Count total number of records
            int totalRecords = query.Count();

            // Ensure 'take' has a value and is greater than 0, otherwise assume all records fit on one page
            if (!take.HasValue || take.Value <= 0)
            {
                return 1; // If no 'take' value or invalid 'take', return 1 page
            }

            // Calculate total pages based on total records and take (items per page)
            int totalPages = (int)Math.Ceiling((double)totalRecords / take.Value);

            return totalPages;
        }

        public bool Create(T entity)
        {
            _dbContext.Add(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public bool Create(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
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
        public async Task<(IEnumerable<T>, int)> GetFilterAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null,
            params Func<IQueryable<T>, IQueryable<T>>[]? includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }
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

            return (await query.ToListAsync(), await CountTotalPagesAsync(filter, take));
        }
        public async Task<bool> CreateAsync(T entity)
        {
            _dbContext.Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateAsync(IEnumerable<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);
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

        public void PrepareCreate(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _dbContext.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public bool PrepareUpdate(IEnumerable<T> entities)
        {
            _dbContext.Attach(entities);
            return _dbContext.SaveChanges() > 0;
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

        private async Task<int> CountTotalPagesAsync(Expression<Func<T, bool>>? filter = null, int? take = null)
        {
            IQueryable<T> query = _dbSet;

            // Apply the filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Count total number of records
            int totalRecords = await query.CountAsync();

            // Ensure 'take' has a value and is greater than 0, otherwise assume all records fit on one page
            if (!take.HasValue || take.Value <= 0)
            {
                return 1; // If no 'take' value or invalid 'take', return 1 page
            }

            // Calculate total pages based on total records and take (items per page)
            int totalPages = (int)Math.Ceiling((double)totalRecords / take.Value);

            return totalPages;
        }
    }
}
