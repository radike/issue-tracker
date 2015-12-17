using IssueTracker.Entities;
using System;
using System.Collections.Generic;

namespace IssueTracker.Data.Services
{
    public interface IIssueService
    {
        ICollection<Issue> GetIssuesByType(IssueType type);
        ICollection<Issue> GetIssuesByType(IssueType type, Guid? projectId);
        ICollection<Issue> GetIssuesByType(IssueType type, Guid? projectId, bool includeClosedIssue = true);
        int GetIssueCount();
        int GetIssueCount(IssueType issueType);
        int GetIssueCount(IssueType issueType, Guid? projectId);
        int GetIssueCount(IssueType issueType, Project project, bool includeClosedIssue = true);
        int GetIssueCount(IssueType issueType, Guid? projectId, bool includeClosedIssue = true);
        ICollection<Issue> GetAllIssues();
        ICollection<Issue> GetAllIssues(Guid? projectId);
        Issue GetByProjectCodeAndIssueNumber(string projectCode, int issueNumber);
        ICollection<Comment> GetCommentsForIssue(Guid issueId);
        int GetNewCodeNumber();
        Issue GetNewEntityForEditing(string projectCode, int issueNumber);
        Issue GetNewEntityForEditing(Guid issueId);
        ICollection<Issue> GetAllVersions(Guid issueId);
        Issue Add(Issue issue);
        ICollection<Issue> GetNewIssues(Guid? projectId);
        int GetRaisedIssueCount(DateTime fromDate, DateTime toDate);
        int GetRaisedIssueCount(Guid? projectId, DateTime fromDate, DateTime toDate);
        ICollection<Issue> GetRaisedIssues(DateTime fromDate, DateTime toDate);
        ICollection<Issue> GetRaisedIssues(Guid? projectId, DateTime fromDate, DateTime toDate);
        ICollection<Issue> GetRaisedIssues(Guid? projectId, int year, int month);
        ICollection<Issue> GetResolvedIssues(Guid? projectId, int year, int month);
        ICollection<Issue> GetResolvedIssues(Guid? projectId, int year, int monthFrom, int monthTo);
        ICollection<Issue> GetResolvedIssues(Guid? projectId, DateTime fromDate, DateTime toDate);
    }
}