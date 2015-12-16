using Common.Data.Core;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
