using Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Data_Repositories
{
    public class StateRepository : DataRepositoryBase<State>, IStateRepository
    {
        protected override State AddEntity(IssueTrackerContext entityContext, State entity)
        {
            return entityContext.States.Add(entity);
        }

        protected override IEnumerable<State> GetEntities(IssueTrackerContext entityContext)
        {
            return from e in entityContext.States.Include("Issues")
                   select e;
        }

        protected override State GetEntity(IssueTrackerContext entityContext, Guid id)
        {
            var query = (from e in entityContext.States.Include("Issues")
                         where e.Id == id
                         select e);
            var results = query.FirstOrDefault();

            return results;
        }

        protected override State UpdateEntity(IssueTrackerContext entityContext, State entity)
        {
            return (from e in entityContext.States
                    where e.Id == entity.Id
                    select e).FirstOrDefault();
        }
    }
}
