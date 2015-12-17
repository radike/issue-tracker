using System;
using System.Collections.Generic;
using IssueTracker.Entities;

namespace IssueTracker.Data.Services
{
    public interface IProjectService
    {
        Guid? GetProjectId(string code);
        IEnumerable<Project> GetProjects();
        IEnumerable<Project> GetProjectsForUser(Guid userId);
        Project GetProject(string code);
        Project GetProject(Guid id);
        void CreateProject(Project project);
        void EditProject(Project project);
        void DeleteProject(string code);
        bool ProjectCodeIsNotUnique(string code);
    }
}