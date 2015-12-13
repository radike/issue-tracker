using Common.Data.Core;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
