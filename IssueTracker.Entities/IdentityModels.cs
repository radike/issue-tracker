using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Common.Data.Core.Contracts;

namespace IssueTracker.Data.Entities
{

    public class ApplicationUserRole : IdentityUserRole<Guid> {}
    public class ApplicationUserLogin : IdentityUserLogin<Guid> {}
    public class ApplicationUserClaim : IdentityUserClaim<Guid> {}
    public class ApplicationRoleStore : RoleStore<ApplicationRole, Guid, ApplicationUserRole>
    {
        public ApplicationRoleStore(DbContext context)
            : base(context)
        {

        }
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole, Guid>
    {
        public ApplicationRoleManager(ApplicationRoleStore store) : base(store)
        {

        }
    }

    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>
    {
        public string Description { get; set; }

        public ApplicationRole() : base() { }
        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description)
            : this(name)
        {
            this.Description = description;
        }
    }

    public class ApplicatonUserStore :
        UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicatonUserStore(DbContext context)
            : base(context)
        {
        }
    }
}
