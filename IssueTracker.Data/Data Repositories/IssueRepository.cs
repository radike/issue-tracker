using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using IssueTracker.Data.Abstractions;
using IssueTracker.Entities;

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
            return FindBy(predicate).OrderByDescending(x => x.Created).First();
        }
    }
}