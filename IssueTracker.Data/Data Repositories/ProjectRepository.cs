using Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Data_Repositories
{
    public class ProjectRepository : DataRepositoryBase<Project>, IProjectRepository
    {
        protected override Project AddEntity(IssueTrackerContext entityContext, Project entity)
        {
            return entityContext.Projects.Add(entity);
        }

        protected override IEnumerable<Project> GetEntities(IssueTrackerContext entityContext)
        {
            return from e in entityContext.Projects
                   select e;
        }

        protected override Project GetEntity(IssueTrackerContext entityContext, Guid id)
        {
            var query = (from e in entityContext.Projects
                         where e.Id == id
                         select e);
            var results = query.FirstOrDefault();

            return results;
        }

        protected override Project UpdateEntity(IssueTrackerContext entityContext, Project entity)
        {
            return (from e in entityContext.Projects
                    where e.Id == entity.Id
                    select e).FirstOrDefault();
        }
    }
}
