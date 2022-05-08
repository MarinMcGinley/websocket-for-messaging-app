using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly UserContext _context;
        public GenericRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<int> CreateEntity(T entity)
        {
            await _context.AddAsync<T>(entity);
            return _context.SaveChanges();
        }

        public async Task<EntityEntry<T>> UpdateEntity(T entity) {
            var entityEntry = _context.Update<T>(entity);
            _context.SaveChanges();
            return entityEntry;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();        
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec) {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec); 
        }
    }
}