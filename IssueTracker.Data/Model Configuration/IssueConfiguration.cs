using System.Data.Entity.ModelConfiguration;
using IssueTracker.Entities;

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
            Property(p => p.AssigneeId).IsOptional();
            Property(p => p.Active).IsRequired();
            Property(p => p.Created).IsRequired();
            Property(p => p.CodeNumber).IsRequired();
            Property(p => p.Type).IsRequired();
            Property(p => p.ResolvedAt).IsOptional();

            HasRequired(p => p.Project)
            .WithMany(c => c.Issues)
            .HasForeignKey(p => new { p.ProjectId, p.ProjectCreatedAt });
            HasRequired(p => p.State).WithMany(p => p.Issues).HasForeignKey(p => p.StateId);
           
            Ignore(p => p.Code);
        }
    }
}
