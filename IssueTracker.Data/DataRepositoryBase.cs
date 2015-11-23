using IssueTracker.Core.Contracts;
using IssueTracker.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, IssueTrackerContext>
        where T : class, IIdentifiableEntity, new()
    {
    }
}
