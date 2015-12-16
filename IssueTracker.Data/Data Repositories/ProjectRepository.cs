using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.Data.Abstractions;
using IssueTracker.Entities;

namespace IssueTracker.Data.Data_Repositories
{
    public class ProjectRepository : VersionedDataRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IDbContext context)
            :base(context)
        {

        }

        public ICollection<Project> GetProjectsForUser(Guid userId)
        {
            return FindBy(i => i.OwnerId == userId || i.Users.Any(u => u.Id == userId)).ToList();
        }
    }
}
