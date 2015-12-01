
using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Model_Configuration
{
    public class IssueConfiguration : EntityTypeConfiguration<Issue>
    {
        public IssueConfiguration()
        {
            Property(p => p.Name).HasMaxLength(255).IsRequired();
            Property(p => p.ProjectId).IsRequired();
            Property(p => p.ProjectCreatedAt).IsRequired();
            Property(p => p.StateId).IsRequired();
            Property(p => p.ReporterId).IsRequired();
            Property(p => p.AssigneeId).IsRequired();
            Property(p => p.Active).IsRequired();
            Property(p => p.Created).IsRequired();
            Property(p => p.CodeNumber).IsRequired();

            HasRequired(p => p.Project)
            .WithMany(c => c.Issues)
            .HasForeignKey(p => new { p.ProjectId, p.ProjectCreatedAt });
            HasRequired(p => p.State).WithMany(p => p.Issues).HasForeignKey(p => p.StateId);
           
            Ignore(p => p.EntityId);
            Ignore(p => p.Code);
        }
    }
}
