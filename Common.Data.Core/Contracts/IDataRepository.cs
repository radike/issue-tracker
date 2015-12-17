using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Data.Core.Contracts
{
    public interface IDataRepository<TEntity>
        where TEntity : class, IIdentifiableEntity, new()
    {
        TEntity Add(TEntity entity);
        void Remove(TEntity entity);
        void Remove(Guid id);
        TEntity Update(TEntity entity);
        ICollection<TEntity> GetAll();
        IQueryable<TEntity> Fetch();
        TEntity Get(Guid id);
        void Save();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity FindSingleBy(Expression<Func<TEntity, bool>> predicate);
    }
}