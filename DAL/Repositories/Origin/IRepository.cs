using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories.Origin
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);

        TEntity GetWithRelated(Expression<Func<TEntity, bool>> match, string EntityName,
            string SecondEnitytyName, string ThirdEntityName);
        int Add(TEntity entity);
        void AddAsync(TEntity entity);
        int AddRange(IEnumerable<TEntity> entities);
        int Remove(TEntity entity);
        int RemoveRange(IEnumerable<TEntity> entities);
        int Update(TEntity entity);
    }
}
