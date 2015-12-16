using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Abstractions;
using IssueTracker.Entities;

namespace IssueTracker.Data.Data_Repositories
{
    public class ApplicationUserRepository : DataRepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbContext context)
            :base(context)
        {

        }
    }
}
