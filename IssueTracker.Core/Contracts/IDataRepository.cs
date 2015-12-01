using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Core.Contracts
{
    public interface IDataRepository<T> 
        where T : class, IIdentifiableEntity, new()
    {
        T Add(T entity);
        void Remove(T entity);
        void Remove(Guid id);
        T Update(T entity);
        IEnumerable<T> Get();
        T Get(Guid id);
    }
}