using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
        public DbSet<TEntity> Table => context.Set<TEntity>();

        public async Task<ICollection<Game>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await context.Games
                .Where(game => ids.Contains(game.Id))
                .ToListAsync();
        }

        public async Task<int> CommitAsync()
        {
           return await context.SaveChangesAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            Table.Remove(entity);
        }

        public IQueryable<TEntity> GetByExpression(bool asNoTracking = false, Expression<Func<TEntity, bool>>? expression = null, params string[] includes)
        {
            var query = Table.AsQueryable();
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            query = asNoTracking == true
                ? query.AsNoTracking()
                : query;

            return expression is not null
                ? query.Where(expression)
                : query;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await Table.FindAsync(id);
        }
    }
}
