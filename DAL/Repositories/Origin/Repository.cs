using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories.Origin
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BillsContext context;

        public Repository(BillsContext context)
        {
            this.context = context;
        }

        public int Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            return context.SaveChanges();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().AsNoTracking().Where<TEntity>(predicate).FirstOrDefaultAsync();
        }

        public async void AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public int AddRange(IEnumerable<TEntity> entities)
        {
            var result = context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Set<TEntity>().AddRange(entities);
            return result;
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().AsNoTracking().Where(predicate).ToList();
        }

        public TEntity Get(int id)
        {

            var result = context.Set<TEntity>().Find(id);
            if (result == null)
            {
                return null;
            }
            context.ChangeTracker.Clear();
            return result;
        }

        public TEntity GetWithRelated(Expression<Func<TEntity, bool>> match, string FirstEntityName,
            string SecondEntityName, string ThirdEntityName)
        {
            var result = context.Set<TEntity>().Include(FirstEntityName).Include(SecondEntityName)
                .Include(ThirdEntityName)
                .AsNoTracking().FirstOrDefault(match);

            if (result == null)
            {
                return null;
            }
            return result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().AsNoTracking().ToList();
        }

        public int Remove(TEntity entity)
        {
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Deleted;
            var result = context.SaveChanges();
            context.ChangeTracker.Clear();
            return result;
        }

        public int Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            var result = context.SaveChanges();
            context.ChangeTracker.Clear();
            return result;
        }

        public int RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
            return context.SaveChanges();

        }
    }
}
