
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Data_Repositories
{
    public class IssueRepository : DataRepositoryBase<Issue>, IIssueRepository
    {
        protected override Issue AddEntity(IssueTrackerContext entityContext, Issue entity)
        {
            return entityContext.Issues.Add(entity);
        }

        protected override IEnumerable<Issue> GetEntities(IssueTrackerContext entityContext)
        {
            
                
                return from e in entityContext.Issues.AsNoTracking().Include("Comments").Include("Project").Include("Assignee").Include("Reporter").Include("State")
                   select e;
            
        }

        protected override Issue GetEntity(IssueTrackerContext entityContext, Guid id)
        {
            
                var query = (from e in entityContext.Issues.Include("Comments").Include("Project").Include("Assignee").Include("Reporter")
                             where e.Id == id
                             select e);
            
            var results = query.FirstOrDefault();

            return results;
        }

        protected override Issue UpdateEntity(IssueTrackerContext entityContext, Issue entity)
        {
            return (from e in entityContext.Issues
                    where e.Id == entity.Id
                    select e).FirstOrDefault();
        }

        public Issue GetByName(string name)
        {
            using (IssueTrackerContext entityContext = new IssueTrackerContext())
            {
                return (from e in entityContext.Issues
                        where e.Name == name
                        select e).FirstOrDefault();
            }
        }
    }
}
