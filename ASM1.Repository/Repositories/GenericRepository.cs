using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ASM1.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Sinh id ngẫu nhiên, kiểm tra trùng cho entity có int id property.
        /// </summary>
        public async Task<int> GenerateUniqueIdAsync(Expression<Func<T, int>> idSelector)
        {
            var rand = new Random();
            int id;
            bool exists;
            do
            {
                id = rand.Next(1_000_000, 9_999_999);
                exists = await _dbSet.AnyAsync(e => idSelector.Compile().Invoke(e) == id);
            } while (exists);
            return id;
        }
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() =>
            {
                _dbSet.Update(entity);
            });
        }
    }
}
