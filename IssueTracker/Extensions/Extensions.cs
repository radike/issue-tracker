using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using IssueTracker.Entities;

namespace IssueTracker.Extensions
{
    public static class Extensions
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this ApplicationUser user, ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
