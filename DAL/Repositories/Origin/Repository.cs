﻿using Microsoft.EntityFrameworkCore;
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

        public void Add(TEntity entity)
        {
            if (context.Entry(entity).State != EntityState.Detached)
            {
                context.Entry(entity).State = EntityState.Detached;

            }

            context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().AsNoTracking().Where<TEntity>(predicate);
        }

        public TEntity Get(int id)
        {

            var result = context.Set<TEntity>().Find(id);
            if (result == null)
            {
                return null;
            }
            context.Entry(result).State = EntityState.Detached;
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

        public void Remove(TEntity entity)
        {
            if (context.Entry(entity).State != EntityState.Detached)
            {
                context.Entry(entity).State = EntityState.Detached;

            }

            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Deleted;
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Detached;
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().AsNoTracking().Where<TEntity>(predicate).FirstOrDefaultAsync();
        }

        public async void AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }
    }
}
