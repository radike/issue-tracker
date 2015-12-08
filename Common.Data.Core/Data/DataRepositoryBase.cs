using Common.Data.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Data.Core
{
    public abstract class DataRepositoryBase<TEntity> : IDataRepository<TEntity>
        where TEntity : class, IIdentifiableEntity, new()
    {
        private IDbContext _context;
        public DataRepositoryBase(IDbContext context)
        {
            _context = context;
        }

        public TEntity Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual void Remove(Guid id)
        {
            TEntity entity = new TEntity() { Id = id };
            _context.Set<TEntity>().Attach(entity);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            TEntity existingEntity = _context.Set<TEntity>().Find(entity.Id);
            if (existingEntity != null)
            {
                _context.Entry<TEntity>(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return existingEntity;
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public virtual TEntity Get(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual IQueryable<TEntity> Fetch()
        {
            return _context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity FindSingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).SingleOrDefault();
        }
    }
}
