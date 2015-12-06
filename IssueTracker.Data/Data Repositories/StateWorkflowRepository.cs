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
    public class StateWorkflowRepository : DataRepositoryBase<StateWorkflow>, IStateWorkflowRepository
    {
        public StateWorkflowRepository(IDbContext context)
            :base(context)
        {

        }
        //protected override StateWorkflow AddEntity(IssueTrackerContext entityContext, StateWorkflow entity)
        //{
        //    return entityContext.StateWorkflows.Add(entity);
        //}

        //protected override IEnumerable<StateWorkflow> GetEntities(IssueTrackerContext entityContext)
        //{
        //    return from e in entityContext.StateWorkflows
        //           select e;
        //}

        //protected override StateWorkflow GetEntity(IssueTrackerContext entityContext, Guid id)
        //{
        //    var query = (from e in entityContext.StateWorkflows
        //                 where e.Id == id
        //                 select e);
        //    var results = query.FirstOrDefault();

        //    return results;
        //}

        //protected override StateWorkflow UpdateEntity(IssueTrackerContext entityContext, StateWorkflow entity)
        //{
        //    return (from e in entityContext.StateWorkflows
        //            where e.Id == entity.Id
        //            select e).FirstOrDefault();
        //}
    }
}
