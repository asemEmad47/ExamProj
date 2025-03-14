using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Interfaces;

namespace ExamProj.Repos.GeneralRepo
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly Context _context;

        public BaseRepo(Context context)
        {
            _context = context;
        }

        public async Task<T?> GetByID(int id)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();

            if (primaryKey == null)
                throw new Exception($"No key detected for this entity {typeof(T).Name}");

            string primaryKeyName = primaryKey.Name;

            var entity = await _context.Set<T>().AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);

            return entity;
        }

        public async Task<T?> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T?> Update(int id, T entity)
        {
            var existingEntity = await GetByID(id);
            if (existingEntity == null) return null;

            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();

            if (primaryKey == null)
                throw new Exception($"No primary key detected for entity {typeof(T).Name}");

            string primaryKeyName = primaryKey.Name;

            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);

            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
    }
}
