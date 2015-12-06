using Common.Data.Core.Contracts;
using IssueTracker.Data.Entities;
using IssueTracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface IProjectRepository : IDataRepository<Project>
    {
        IEnumerable<Project> GetActiveProjects();
    }
}
