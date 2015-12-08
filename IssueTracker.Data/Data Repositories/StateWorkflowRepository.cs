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
    }
}
