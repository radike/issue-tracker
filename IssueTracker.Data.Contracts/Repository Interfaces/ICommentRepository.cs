using Entities;
using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface ICommentRepository : IDataRepository<Comment>
    {

    }
}
