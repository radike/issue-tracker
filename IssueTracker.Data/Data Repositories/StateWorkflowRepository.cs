using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.Data.Abstractions;
using IssueTracker.Entities;

namespace IssueTracker.Data.Data_Repositories
{
    public class StateWorkflowRepository : DataRepositoryBase<StateWorkflow>, IStateWorkflowRepository
    {
        public StateWorkflowRepository(IDbContext context)
            :base(context)
        {

        }

        public IEnumerable<StateWorkflow> GetPossibleWorkflows(Guid fromStateId)
        {
            return GetAll().Where(c => c.FromStateId == fromStateId).ToList();
        }
    }
}
