using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Abstractions;
using IssueTracker.Entities;

namespace IssueTracker.Data.Data_Repositories
{
    public class CommentRepository : VersionedDataRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IDbContext context)
            :base(context)
        {

        }
    }
}