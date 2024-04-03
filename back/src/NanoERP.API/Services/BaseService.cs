using Microsoft.EntityFrameworkCore;
using NanoERP.API.Data;
using NanoERP.API.Domain.interfaces;

namespace NanoERP.API.Services
{
    public class BaseService<T>(DataContext db) where T : class, IEntity
    {
        private readonly DataContext _db = db;

        public async Task<IEnumerable<T>> GetAsync() 
        {
            return await _db.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var entities = await _db.Set<T>().AsNoTracking().ToListAsync();
            return entities.FirstOrDefault(e => e.Id.ToString() == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task TruncateAsync()
        {
            _db.Set<T>().RemoveRange(_db.Set<T>());
            await _db.SaveChangesAsync();
        }
    }
}