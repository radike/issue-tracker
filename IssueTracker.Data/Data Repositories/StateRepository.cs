using Common.Data.Core;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Entities;
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
        public StateRepository(IDbContext context)
            :base(context)
        {

        }
        //protected override State AddEntity(IssueTrackerContext entityContext, State entity)
        //{
        //    return entityContext.States.Add(entity);
        //}

        //protected override IEnumerable<State> GetEntities(IssueTrackerContext entityContext)
        //{
        //    return from e in entityContext.States.Include("Issues")
        //           select e;
        //}

        //protected override State GetEntity(IssueTrackerContext entityContext, Guid id)
        //{
        //    var query = (from e in entityContext.States.Include("Issues")
        //                 where e.Id == id
        //                 select e);
        //    var results = query.FirstOrDefault();

        //    return results;
        //}

        //protected override State UpdateEntity(IssueTrackerContext entityContext, State entity)
        //{
        //    return (from e in entityContext.States
        //            where e.Id == entity.Id
        //            select e).FirstOrDefault();
        //}

        public IEnumerable<State> GetStatesOrderedByIndex()
        {
            using (var entityContext = new IssueTrackerContext())
            {
                return entityContext.States.OrderBy(x => x.OrderIndex).ToList();
            }
        }

        public State GetInitialState()
        {
            using (var entityContext = new IssueTrackerContext())
            {
                return entityContext.States.FirstOrDefault(x => x.IsInitial);
            }
        }

        public int GetStatesOrderIndex()
        {
            return GetAll().Max(x => (int?)x.OrderIndex) + 1 ?? 1;
        }

        public List<State> GetMovedStates(int toPosition, int fromPosition)
        {
            return GetAll().Where(c => (toPosition <= c.OrderIndex && c.OrderIndex <= fromPosition)).ToList();
        }
    }
}