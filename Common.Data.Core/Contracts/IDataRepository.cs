using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Data.Core.Contracts
{
    public interface IDataRepository<T> 
        where T : class, IIdentifiableEntity, new()
    {
        T Add(T entity);
        void Remove(T entity);
        void Remove(Guid id);
        T Update(T entity);
        ICollection<T> GetAll();
        IQueryable<T> Fetch();
        T Get(Guid id);
        void Save();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T FindSingleBy(Expression<Func<T, bool>> predicate);
    }
}