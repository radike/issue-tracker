using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Data.Entities;
using IssueTracker.Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System.Data.Entity;

namespace IssueTracker.Data.Services
{
    public class IssueService : IIssueService
    {
        private IIssueRepository _issueRepo;
        private IProjectRepository _projectRepo;
        private ICommentRepository _commentRepo;

        public IssueService(IIssueRepository issueRepository, IProjectRepository projectRepository, ICommentRepository commentRepo)
        {
            _issueRepo = issueRepository;
            _projectRepo = projectRepository;
            _commentRepo = commentRepo;
        }

        public ICollection<Issue> GetAllIssues()
        {
            return _issueRepo.Fetch()
                .Include(p => p.State)                                
                .Include(p => p.Project)
                .ToList();
        }
        public int GetNewCodeNumber()
        {
            return _issueRepo.GetAll().Max(x => (int?)x.CodeNumber) + 1 ?? 1;
        }
        public Issue GetByProjectCodeAndIssueNumber(string projectCode, int issueNumber)
        {
            return _issueRepo.Fetch().Include(i => i.Project).Where(i => i.Project.Code == projectCode && i.CodeNumber == issueNumber).Include(i => i.State).FirstOrDefault();
        }

        public ICollection<Comment> GetCommentsForIssue(Guid issueId)
        {
            return _commentRepo.GetAll().AsQueryable()
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .Where(n => n.IssueId == issueId)
                .OrderBy(n => n.Posted)
                .Include(n => n.Author)
                .ToList();
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
            return project == null ? 0 : GetIssueCount(issueType, project.Id, includeClosedIssue);
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

        public ICollection<Issue> GetAllVersions(Guid id)
        {
            return _issueRepo.GetAllVersions(id).OrderBy(x => x.CreatedAt).ToList();
        }

        public Issue GetNewEntityForEditing(string projectCode, int issueNumber)
        {
            return _issueRepo.Fetch()
                     .AsNoTracking()
                     .Where(i => i.Project.Code == projectCode && i.CodeNumber == issueNumber)
                     .OrderByDescending(x => x.CreatedAt)
                     .FirstOrDefault();
        }

        public Issue GetNewEntityForEditing(Guid issueId)
        {
            return _issueRepo.Fetch().AsNoTracking().Where(x => x.Id == issueId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }

        public Issue Add(Issue issue)
        {
            return _issueRepo.Add(issue);
        }
    }
}
