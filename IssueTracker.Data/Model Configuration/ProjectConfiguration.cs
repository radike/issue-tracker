using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Model_Configuration
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            Property(p => p.Title).HasMaxLength(255).IsRequired();
            Ignore(p => p.EntityId);
            this.HasMany(a => a.Users).WithMany(b => b.Projects).Map(m =>
            {
                m.MapLeftKey("Project_Id","Project_CreatedAt");
                m.MapRightKey("ApplicationUser_id");
                m.ToTable("ProjectApplicationUser");
            });
        }
    }
}
