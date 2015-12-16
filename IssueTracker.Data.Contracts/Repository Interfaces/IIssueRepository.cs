using Common.Data.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Entities;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface IIssueRepository : IDataRepository<Issue>
    {
        Issue GetByName(string name);
        IQueryable<Issue> GetAllVersions(Guid id);
    }
}
