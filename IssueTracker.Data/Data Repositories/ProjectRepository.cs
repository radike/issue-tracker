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

        public IEnumerable<Project> GetActiveProjects()
        {
            using (var entityContext = new IssueTrackerContext())
            {
                return entityContext.Projects
                    .Where(n => n.Active)
                    .GroupBy(n => n.Id)
                    .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault()).ToList();
            }
        }
    }
}
