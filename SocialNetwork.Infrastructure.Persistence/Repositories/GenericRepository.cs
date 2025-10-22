using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Domain.Interfaces;
using SocialNetwork.Infrastructure.Persistence.Context;

namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> 
        where Entity : class
    {
        public readonly SocialNetworkDbContext _context;

        public GenericRepository(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Set<Entity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<Entity>> GetAllAsync()
        {
            var entity = _context.Set<Entity>().AsQueryable();

            return await entity.ToListAsync();
        }

        public virtual async Task<Entity?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<Entity>().FindAsync(id);

             if (entity == null) 
             { 
               throw new Exception($"{typeof(Entity).Name} with id {id} not found");
             }

                return entity;

        }
        public virtual async Task<Entity> UpdateAsync(int id, Entity entry)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception($"{typeof(Entity).Name} with id {id} not found");
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Entry(entity!).CurrentValues.SetValues(entry!);
            await _context.SaveChangesAsync();
            return entity!;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception($"{typeof(Entity).Name} with id {id} not found");
            }

            _context.Set<Entity>().Remove(entity!);
            await _context.SaveChangesAsync();
            return true;

        }

        public virtual async Task<List<Entity>> AddRangeAsync(List<Entity> entities)
        {

            var val = entities.FirstOrDefault();

            if (val == null)
            {
                throw new ArgumentNullException(nameof(val));
            }

            await _context.Set<Entity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;

        }

        public virtual IQueryable<Entity> GetAllQuery()
        {
            var entity = _context.Set<Entity>().AsQueryable();
            return entity;
        }

        public virtual IQueryable<Entity> GetAllQueryWithInclude(List<string> includes)
        {
            var query = _context.Set<Entity>().AsQueryable();


            foreach (var property in includes)
            {
                query = query.Include(property);
            }

            return query;
        }

        public virtual List<Entity> GetAllListWithInclude(List<string> includes)
        {
            var query = _context.Set<Entity>().AsQueryable();

            foreach (var property in includes)
            {
                query = query.Include(property);
            }

            return (List<Entity>)query;
        }
    }
}

    

