using Common.Data.Core.Contracts;
using IssueTracker.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Data.Entity;

namespace IssueTracker.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<Guid>, IIdentifiableEntity
    {
        public ApplicationUser()
        {

        }

        public ApplicationUser(string userName)
            : base()
        {
            UserName = userName;
        }

        public ICollection<Project> Projects { get; set; }

        //string IUser<string>.Id
        //{
        //    get
        //    {
        //        return Id.ToString();
        //    }
        //}

        
    }


    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
    }

    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
    }

    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
    }

    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>
    {
    }

    public class ApplicatonUserStore :
        UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicatonUserStore(DbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}
