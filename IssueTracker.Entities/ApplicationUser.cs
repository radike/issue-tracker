using System;
using System.Collections.Generic;
using Common.Data.Core.Contracts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IssueTracker.Entities
{
    public class ApplicationUser : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<Guid>, IIdentifiableEntity
    {
        public ApplicationUser()
        {

        }

        public ApplicationUser(string userName)
        {
            UserName = userName;
        }

        public ICollection<Project> Projects { get; set; }
    }
}