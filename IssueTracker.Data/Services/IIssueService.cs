using IssueTracker.Data.Entities;
using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Services
{
    public interface IIssueService
    {
        ICollection<Issue> GetIssuesByType(IssueType type);
        ICollection<Issue> GetIssuesByType(IssueType type, Guid projectId);
        ICollection<Issue> GetIssuesByType(IssueType type, Guid projectId, bool includeClosedIssue = true);
        int GetIssueCount();
        int GetIssueCount(IssueType issueType);
        int GetIssueCount(IssueType issueType, Guid projectId);
        int GetIssueCount(IssueType issueType, Project project, bool includeClosedIssue = true);
        int GetIssueCount(IssueType issueType, Guid projectId, bool includeClosedIssue = true);
    }
}
