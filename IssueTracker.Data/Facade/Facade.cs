using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Data.Services;
using IssueTracker.Entities;

namespace IssueTracker.Data.Facade
{
    public class IssueTrackerFacade : IFacade
    {
        private IIssueService _issueService;
        private IProjectService _projectService;
        private IStateService _stateService;

        public IssueTrackerFacade(IIssueService issueService, IStateService stateService, IProjectService projectService)
        {
            _issueService = issueService;
            _projectService = projectService;
            _stateService = stateService;
        }
        public ICollection<Issue> GetIssuesInProgress()
        {
            throw new NotImplementedException();
        }

        public ICollection<Issue> GetIssuesInProgress(string projectCode)
        {
            Project project = _projectService.GetProject(projectCode);

            return GetIssuesInProgress(project);
        }

        public ICollection<Issue> GetIssuesInProgress(Project project)
        {
            Guid? projectId = project != null ? project.Id : default(Guid?);
            return GetIssuesInProgress(projectId);
        }

        public ICollection<Issue> GetIssuesInProgress(Guid? projectId)
        {
            var finalStates = _stateService.GetFinalStateIds();
            var issuesInProgress = _issueService.GetAllIssues(projectId).Where(i => !i.State.IsInitial && finalStates.All(id => id != i.StateId));

            if (projectId.HasValue)
            {
                issuesInProgress.Where(i => i.ProjectId == projectId);
            }

            return issuesInProgress.ToList();
        }

        public ICollection<Issue> GetNewIssues(string projectCode)
        {
            Project project = _projectService.GetProject(projectCode);
            return GetNewIssues(project.Id);
        }

        public ICollection<Issue> GetNewIssues(Guid? projectId)
        {
            return _issueService.GetNewIssues(projectId);
        }

        public int GetRaisedIssueCount(DateTime fromDate, DateTime toDate)
        {
            return GetRaisedIssueCount(fromDate, toDate);
        }

        public int GetRaisedIssueCount(Guid? projectId, DateTime fromDate, DateTime toDate)
        {
            return _issueService.GetRaisedIssueCount(projectId, fromDate, toDate);
        }

        public ICollection<Issue> GetResolvedIssues(string projectCode)
        {
            Project project = _projectService.GetProject(projectCode);
            return GetResolvedIssues(project.Id);
        }

        public ICollection<Issue> GetResolvedIssues(Guid? projectId)
        {
            var resolvedIssues = _issueService.GetAllIssues(projectId).Where(i => IsIssueInFinalState(i));
            if (projectId.HasValue)
            {
                resolvedIssues.Where(i => i.ProjectId == projectId);
            }

            return resolvedIssues.ToList();
        }

        public bool IsIssueInFinalState(Issue issue)
        {
            var finalStates = _stateService.GetFinalStateIds();
            return (finalStates.Contains(issue.StateId)) ? true : false;
        }

        public ICollection<Tuple<string, int, int>> GetIssueBurndownChartData(string projectCode, int numberOfMonths)
        {
            Project project = _projectService.GetProject(projectCode);
            return GetIssueBurndownChartData(project.Id, numberOfMonths);
        }

        public ICollection<Tuple<string, int, int>> GetIssueBurndownChartData(Guid? projectId, int numberOfMonths)
        {
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            var chartData = new List<Tuple<string, int, int>>();
            for (int i = numberOfMonths - 1; i >= 0; i--)
            {
                //int month = currentMonth - i < 1 ? currentMonth - i; uprav year a month
                int month = currentMonth - i;
                var raisedIssues = _issueService.GetRaisedIssues(projectId, currentYear, month).Count;
                var resolvedIssues = _issueService.GetResolvedIssues(projectId, currentYear, month).Count;
                chartData.Add(new Tuple<string, int, int>(GetMonthName(month), resolvedIssues, raisedIssues));
            }

            return chartData;
        }

        private string GetMonthName(int month)
        {
            string monthName;
            switch (month)
            {
                case 8:
                    monthName = "Aug";
                    break;
                case 9:
                    monthName = "Sep";
                    break;
                case 10:
                    monthName = "Oct";
                    break;
                case 11:
                    monthName = "Nov";
                    break;
                default:
                case 12:
                    monthName = "Dec";
                    break;
            }

            return monthName;
        }
    }
}
