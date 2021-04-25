using DAL.Repositories.Origin;
using System;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        public int Complete();
    }
}
