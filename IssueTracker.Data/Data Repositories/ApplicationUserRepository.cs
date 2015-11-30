using Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Data_Repositories
{
    public class ApplicationUserRepository : DataRepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        //public class CommentRepository : DataRepositoryBase<Comment>, ICommentRepository
        protected override ApplicationUser AddEntity(IssueTrackerContext entityContext, ApplicationUser entity)
        {
            return entityContext.Users.Add(entity);
        }

        protected override IEnumerable<ApplicationUser> GetEntities(IssueTrackerContext entityContext)
        {
            return from e in entityContext.Users
                   select e;
        }

        protected override ApplicationUser GetEntity(IssueTrackerContext entityContext, Guid id)
        {
            var query = (from e in entityContext.Users
                         where e.Id == id.ToString()
                         select e);
            var results = query.FirstOrDefault();

            return results;
        }

        protected override ApplicationUser UpdateEntity(IssueTrackerContext entityContext, ApplicationUser entity)
        {
            return (from e in entityContext.Users
                    where e.Id == entity.Id.ToString()
                    select e).FirstOrDefault();
        }
    }
}
