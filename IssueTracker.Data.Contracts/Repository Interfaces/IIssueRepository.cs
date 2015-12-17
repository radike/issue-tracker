using Common.Data.Core.Contracts;
using System;
using System.Linq;
using IssueTracker.Entities;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface IIssueRepository : IDataRepository<Issue>
    {
        Issue GetByName(string name);
        IQueryable<Issue> GetAllVersions(Guid id);
    }
}