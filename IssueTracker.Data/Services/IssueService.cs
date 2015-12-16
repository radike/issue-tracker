using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Data.Entities;
using IssueTracker.Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System.Data.Entity;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.Data.Services
{
    public class IssueService : IIssueService
    {
        private IIssueRepository _issueRepo;
        private IProjectRepository _projectRepo;
        private ICommentRepository _commentRepo;
        private IStateWorkflowRepository _stateWfRepo;

        public IssueService(IIssueRepository issueRepository, IProjectRepository projectRepository, ICommentRepository commentRepo, IStateWorkflowRepository stateWorkflowRepository)
        {
            _issueRepo = issueRepository;
            _projectRepo = projectRepository;
            _commentRepo = commentRepo;
            _stateWfRepo = stateWorkflowRepository;
        }

        public ICollection<Issue> GetAllIssues()
        {
            return GetAllIssues(null);
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

        public int GetIssueCount(IssueType issueType, Guid? projectId)
        {
            throw new NotImplementedException();
        }

        public int GetIssueCount(IssueType issueType, Project project, bool includeClosedIssue = true)
        {
            Guid? projectId;

            if (project == null)
            {
                projectId = null;
            }
            else
            {
                projectId = project.Id;
            }

            return GetIssueCount(issueType, projectId, includeClosedIssue);
        }

        public int GetIssueCount(IssueType issueType, Guid? projectId, bool includeClosedIssue = true)
        {
            return GetIssuesByType(issueType, projectId, includeClosedIssue).Count;
        }

        public ICollection<Issue> GetIssuesByType(IssueType type)
        {
            throw new NotImplementedException();
        }

        public ICollection<Issue> GetIssuesByType(IssueType type, Guid? projectId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Issue> GetIssuesByType(IssueType issueType, Guid? projectId, bool includeClosedIssue = false)
        {
            var issuesByType = _issueRepo.FindBy(i => i.Type == issueType);

            if (projectId != null)
            {
                issuesByType = issuesByType.Where(i => i.ProjectId == projectId);
            }

            return issuesByType.ToList();
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

        public ICollection<Issue> GetAllIssues(Guid? projectId)
        {
            var allIssues = _issueRepo.Fetch()
                                      .Include(p => p.State)
                                      .Include(p => p.Project);

            if (projectId.HasValue)
            {
                allIssues = allIssues.Where(i => i.ProjectId == projectId);
            }

            return allIssues.ToList();
        }

        public ICollection<Issue> GetNewIssues(Guid? projectId)
        {
            var newIssues = _issueRepo.FindBy(i => i.State.IsInitial);
            if (projectId.HasValue)
            {
                newIssues = newIssues.Where(i => i.ProjectId == projectId);
            }

            return newIssues.ToList();
        }

        public int GetRaisedIssueCount(DateTime fromDate, DateTime toDate)
        {
            return GetRaisedIssueCount(null, fromDate, toDate);
        }

        public int GetRaisedIssueCount(Guid? projectId, DateTime fromDate, DateTime toDate)
        {
            return GetRaisedIssues(projectId, fromDate, toDate).Count;
        }

        public ICollection<Issue> GetRaisedIssues(int year, int monthFrom, int monthTo)
        {
            return GetRaisedIssues(null, year, monthFrom, monthTo);
        }

        public ICollection<Issue> GetRaisedIssues(Guid? projectId, int year, int month)
        {
            return GetRaisedIssues(projectId, year, month, month);
        }

        public ICollection<Issue> GetRaisedIssues(Guid? projectId, int year, int monthFrom, int monthTo)
        {
            DateTime fromDate = new DateTime(year, monthFrom, 1, 0, 0, 0);
            DateTime toDate = new DateTime(year, monthTo, DateTime.DaysInMonth(year, monthTo), 23, 59, 59);

            return GetRaisedIssues(projectId, fromDate, toDate);
        }

        public ICollection<Issue> GetRaisedIssues(DateTime fromDate, DateTime toDate)
        {
            return GetRaisedIssues(null, fromDate, toDate);
        }

        public ICollection<Issue> GetRaisedIssues(Guid? projectId, DateTime fromDate, DateTime toDate)
        {
            var raisedIssues = _issueRepo.FindBy(i => fromDate <= i.Created && i.Created <= toDate);
            if (projectId.HasValue)
            {
                raisedIssues = raisedIssues.Where(i => i.ProjectId == projectId);
            }

            return raisedIssues.ToList();
        }

        public ICollection<Issue> GetResolvedIssues(Guid? projectId, int year, int month)
        {
            return GetResolvedIssues(projectId, year, month, month);
        }

        public ICollection<Issue> GetResolvedIssues(Guid? projectId, int year, int monthFrom, int monthTo)
        {
            DateTime fromDate = new DateTime(year, monthFrom, 1, 0, 0, 0);
            DateTime toDate = new DateTime(year, monthTo, DateTime.DaysInMonth(year, monthTo), 23, 59, 59);

            return GetResolvedIssues(projectId, fromDate, toDate);
        }

        public ICollection<Issue> GetResolvedIssues(Guid? projectId, DateTime fromDate, DateTime toDate)
        {
            var raisedIssues = _issueRepo.FindBy(i => fromDate <= i.ResolvedAt && i.ResolvedAt <= toDate);
            if (projectId.HasValue)
            {
                raisedIssues = raisedIssues.Where(i => i.ProjectId == projectId);
            }

            return raisedIssues.ToList();
        }
    }
}
