using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Data.Entities;
using IssueTracker.Data.Services;
using IssueTracker.Services;

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
    }
}
