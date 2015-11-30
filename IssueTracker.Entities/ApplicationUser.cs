using IssueTracker.Core.Contracts;
using IssueTracker.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationUser : IdentityUser, IIdentifiableEntity
    {
        public ICollection<Project> Projects { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [NotMapped]
        public Guid EntityId
        {
            get { return new Guid(this.Id); }
            set { Id = value.ToString(); }
        }
    }
}
