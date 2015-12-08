
using Common.Data.Core;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Data_Repositories
{
    public class IssueRepository : VersionedDataRepository<Issue>, IIssueRepository
    {
        public IssueRepository(IDbContext context)
            :base(context)
        {

        }

        public Issue GetByName(string name)
        {
            using (IssueTrackerContext entityContext = new IssueTrackerContext())
            {
                return (from e in entityContext.Issues
                        where e.Name == name
                        select e).FirstOrDefault();
            }
        }
    }
}
