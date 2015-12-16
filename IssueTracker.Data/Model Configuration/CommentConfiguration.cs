using System.Data.Entity.ModelConfiguration;
using IssueTracker.Entities;

namespace IssueTracker.Data.Model_Configuration
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(p => p.Posted).IsOptional();
            Property(p => p.Text).IsRequired();
            Property(p => p.Active).IsRequired();
            Property(p => p.IssueId).IsRequired();

            HasRequired(p => p.Issue)
            .WithMany(c => c.Comments)
            .HasForeignKey(p => new { p.IssueId, p.IssueCreatedAt });
        }
    }
}
