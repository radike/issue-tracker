


using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface IIssueRepository : IDataRepository<Issue>
    {
        Issue GetByName(string name);
    }
}
