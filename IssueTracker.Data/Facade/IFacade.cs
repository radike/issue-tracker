using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Facade
{
    public interface IFacade
    {
        ICollection<Issue> GetIssuesInProgress();
        ICollection<Issue> GetIssuesInProgress(Project project);
        ICollection<Issue> GetIssuesInProgress(Guid? projectId);
        ICollection<Issue> GetIssuesInProgress(string projectCode);
    }
}
