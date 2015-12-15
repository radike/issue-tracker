using Common.Data.Core.Contracts;
using IssueTracker.Data.Entities;
using IssueTracker.Data.Contracts;
using IssueTracker.Data.Model_Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueTracker.Data.Abstractions;

namespace IssueTracker.Data
{
    public class IssueTrackerContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDbContext
    {
        public IssueTrackerContext() : base()
        {
            //  Database.SetInitializer<IssueTrackerContext>(null);
            //DbContext
        }

        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<StateWorkflow> StateWorkflows { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new IssueConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new StateConfiguration());
            modelBuilder.Configurations.Add(new StateWorkflowConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Ignore<IIdentifiableEntity>();
            modelBuilder.Entity<Comment>().HasKey(c => new { c.Id, c.CreatedAt });
            modelBuilder.Entity<Issue>().HasKey(i => new { i.Id, i.CreatedAt });
            modelBuilder.Entity<Project>().HasKey(p => new { p.Id, p.CreatedAt });

            

        }

        public static IssueTrackerContext Create()
        {
            return new IssueTrackerContext();
        }
    }
}
