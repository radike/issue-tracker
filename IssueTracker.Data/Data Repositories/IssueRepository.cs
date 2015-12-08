
using Common.Data.Core;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

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
            return FindBy(i => i.Name.Equals(name)).FirstOrDefault();
        }

        public override Issue FindSingleBy(Expression<Func<Issue, bool>> predicate)
        {
            return base.FindBy(predicate).OrderByDescending(x => x.Created).First();
        }
    }
}
