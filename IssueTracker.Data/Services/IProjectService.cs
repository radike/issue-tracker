using System;
using System.Collections.Generic;
using IssueTracker.Entities;

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
