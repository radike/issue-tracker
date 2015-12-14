using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Services
{
    public interface IProjectService
    {

        Guid? GetProjectId(String code);

        IEnumerable<Project> GetProjects();

        IEnumerable<Project> GetProjectsForUser(Guid userId);

        Project GetProject(String code);

        Project GetProject(Guid id);

        void CreateProject(Project project);

        void EditProject(Project project);

        void DeleteProject(String code);

        bool ProjectCodeIsNotUnique(String code);
    }
}
