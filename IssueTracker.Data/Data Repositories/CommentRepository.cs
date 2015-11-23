using Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Data_Repositories
{
    public class CommentRepository : DataRepositoryBase<Comment>, ICommentRepository
    {
        protected override Comment AddEntity(IssueTrackerContext entityContext, Comment entity)
        {
            return entityContext.Comments.Add(entity);
        }

        protected override IEnumerable<Comment> GetEntities(IssueTrackerContext entityContext)
        {
            return from e in entityContext.Comments
                   select e;
        }

        protected override Comment GetEntity(IssueTrackerContext entityContext, Guid id)
        {
            var query = (from e in entityContext.Comments
                         where e.Id == id
                         select e);
            var results = query.FirstOrDefault();

            return results;
        }

        protected override Comment UpdateEntity(IssueTrackerContext entityContext, Comment entity)
        {
            return (from e in entityContext.Comments
                    where e.Id == entity.Id
                    select e).FirstOrDefault();
        }
    }
}
