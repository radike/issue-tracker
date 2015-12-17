using System.Data.Entity.ModelConfiguration;
using IssueTracker.Entities;

namespace IssueTracker.Data.Model_Configuration
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            Property(p => p.Title).HasMaxLength(255).IsRequired();
            Property(p => p.Code).IsRequired();
            Property(p => p.Active).IsRequired();
            Ignore(p => p.SelectedUsers);
            HasMany(a => a.Users).WithMany(b => b.Projects).Map(m =>
            {
                m.MapLeftKey("Project_Id","Project_CreatedAt");
                m.MapRightKey("ApplicationUser_id");
                m.ToTable("ProjectApplicationUser");
            });
        }
    }
}