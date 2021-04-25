using DAL.Repositories.Origin;
using System;
using System.Collections;

namespace DAL.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {

        private readonly BillsContext _context;
        private Hashtable _services;

        public UnitOfWork(BillsContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            _services ??= new Hashtable();
            var type = typeof(Repository<>).MakeGenericType(typeof(TEntity));
            if (_services.ContainsKey(typeof(TEntity).Name)) return (Repository<TEntity>)_services[typeof(TEntity)];

            var service = (Repository<TEntity>)Activator.CreateInstance(type, _context);
            _services.Add(typeof(TEntity).Name, service);

            return service;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public int Complete()
        {
            return this._context.SaveChanges();
        }
    }
}