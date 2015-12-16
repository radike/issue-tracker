using System;
using System.Collections.Generic;
using IssueTracker.Entities;

namespace IssueTracker.Data.Facade
{
    public interface IFacade
    {
        bool IsIssueInFinalState(Issue issue);
        ICollection<Issue> GetIssuesInProgress();
        ICollection<Issue> GetIssuesInProgress(Project project);
        ICollection<Issue> GetIssuesInProgress(Guid? projectId);
        ICollection<Issue> GetIssuesInProgress(string projectCode);
        ICollection<Issue> GetNewIssues(string projectCode);
        ICollection<Issue> GetNewIssues(Guid? projectId);
        ICollection<Issue> GetResolvedIssues(string id);
        ICollection<Issue> GetResolvedIssues(Guid? id);
        ICollection<Tuple<string, int, int>> GetIssueBurndownChartData(string projectCode, int numberOfMonths);
        ICollection<Tuple<string, int, int>> GetIssueBurndownChartData(Guid? projectId, int numberOfMonths);
    }
}
