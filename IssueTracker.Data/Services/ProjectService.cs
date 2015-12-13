using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IssueTracker.Data.Services
{
    public class ProjectService : IProjectService
    {
        private IProjectRepository _projectRepo;
        private IIssueRepository _issueRepo;
        private IApplicationUserRepository _userRepo;

        public ProjectService(IProjectRepository projectRepository, IIssueRepository issueRepository, IApplicationUserRepository applicationUserRepository)
        {
            _projectRepo = projectRepository;
            _issueRepo = issueRepository;
            _userRepo = applicationUserRepository;
        }

        public Guid? GetProjectId(String code)
        {
            var project = _projectRepo.FindSingleBy(x => x.Code == code);
            if (project == null)
                return null;
            return project.Id;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projectRepo.Fetch()
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .ToList();
        }

        public Project GetProject(String code)
        {
            var project = _projectRepo.Fetch()
                .AsQueryable()
                .Where(p => p.Code == code && p.Active)
                .OrderByDescending(x => x.CreatedAt)
                .Include(p => p.Issues)
                .Include(p => p.Owner)
                .Include(p => p.Users)
                .FirstOrDefault();
            project.Issues = _issueRepo.Fetch().Where(i => i.ProjectId == project.Id).ToList();
            return project;
        }

        public IEnumerable<Project> GetProjectsForUser(Guid userId)
        {
            return _projectRepo.GetProjectsForUser(userId);
        }

        public void CreateProject(Project project)
        {
            if (ProjectCodeIsNotUnique(project.Code))
            {
                throw new ProjectCodeIsInUseException();
            }
            project.Id = Guid.NewGuid();
            project.Code = project.Code.ToUpper();
            project.CreatedAt = DateTime.Now;
            addProjectOwnerToProjectUsers(project);
            project.Users = _userRepo.FindBy(u => project.SelectedUsers.Contains(u.Id)).ToList();

            _projectRepo.Add(project);
        }

        public bool ProjectCodeIsNotUnique(String code)
        {
            return Enumerable.Any(_projectRepo.GetAll(), p => p.Code.Equals(code.ToUpper()));
        }

        public class ProjectCodeIsInUseException : Exception
        {

        }

        public void EditProject(Project project)
        {
            project.CreatedAt = DateTime.Now;
            addProjectOwnerToProjectUsers(project);
            project.Users = _userRepo.FindBy(u => project.SelectedUsers.Contains(u.Id)).ToList();

            _projectRepo.Add(project);
        }

        public void DeleteProject(String code)
        {
            _projectRepo.Remove(GetProjectId(code).Value);
        }

        private static void addProjectOwnerToProjectUsers(Project project)
        {
            project.SelectedUsers = project.SelectedUsers != null ? project.SelectedUsers.Union(new[] { project.OwnerId }).ToList() : new List<Guid> { project.OwnerId };
        }
    }
}
