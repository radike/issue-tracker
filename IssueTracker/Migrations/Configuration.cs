namespace IssueTracker.Migrations
{
    using Data;
    using global::Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IssueTrackerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        public string PasswordHash { get; private set; }

        protected override void Seed(IssueTrackerContext context)
        {
            //  This method will be called after migrating to the latest version.

            SeedUsersAndRoles(context);
        }

        private static void SeedUsersAndRoles(IssueTrackerContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = UserRoles.Administrators.ToString() },
                new IdentityRole { Name = UserRoles.Users.ToString() }
            };

            roles.ForEach(role =>
            {
                if (!context.Roles.Any(r => r.Name == role.Name))
                {
                    roleManager.Create(role);
                }
            });

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var users = new List<User>();

            users.Add(new User("admin@admin.com", "Password@123", UserRoles.Administrators));
            users.Add(new User("user@user.com", "Password@123", UserRoles.Users));

            users.ForEach(user =>
            {
                if (!context.Users.Any(u => u.UserName == user.UserName))
                {
                    var newUser = new ApplicationUser { UserName = user.UserName, Email = user.UserName };
                    userManager.Create(newUser, user.Password);
                    userManager.AddToRole(newUser.Id, user.Role.ToString());
                }
            });
        }

        private class User
        {
            public string UserName { get; private set; }
            public string Password { get; private set; }
            public UserRoles Role { get; private set; }

            public User(string userName, string password, UserRoles role)
            {
                UserName = userName;
                Password = password;
                Role = role;
            }
        }
    }
}
