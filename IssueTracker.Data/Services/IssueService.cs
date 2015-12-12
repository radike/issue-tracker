using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Data.Entities;
using IssueTracker.Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;

namespace IssueTracker.Data.Services
{
    public class IssueService : IIssueService
    {
        private IIssueRepository _issueRepo;
        private IProjectRepository _projectRepo;

        public IssueService(IIssueRepository issueRepository, IProjectRepository projectRepository)
        {
            _issueRepo = issueRepository;
            _projectRepo = projectRepository;
        }

        public int GetIssueCount()
        {
            throw new NotImplementedException();
        }

        public int GetIssueCount(IssueType issueType)
        {
            throw new NotImplementedException();
        }

        public int GetIssueCount(IssueType issueType, Guid projectId)
        {
            throw new NotImplementedException();
        }

        public int GetIssueCount(IssueType issueType, Project project, bool includeClosedIssue = true)
        {
            return GetIssueCount(issueType, project.Id, includeClosedIssue);
        }

        public int GetIssueCount(IssueType issueType, Guid projectId, bool includeClosedIssue = true)
        {
            return GetIssuesByType(issueType, projectId, includeClosedIssue).Count;
        }

        public ICollection<Issue> GetIssuesByType(IssueType type)
        {
            throw new NotImplementedException();
        }

        public ICollection<Issue> GetIssuesByType(IssueType type, Guid projectId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Issue> GetIssuesByType(IssueType issueType, Guid projectId, bool includeClosedIssue = false)
        {
            return _issueRepo.FindBy(i => i.ProjectId == projectId && i.Type == issueType).ToList();
        }
    }
}
