
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
            Property(p => p.Name).HasMaxLength(25).IsRequired();
            //this.HasMany(a => a.Users).WithMany(b => b.Issues).Map(m =>
            //{
            //    m.MapLeftKey("UserId");
            //    m.MapRightKey("IssueId");
            //    m.ToTable("ProjectApplicationUser");
            //});
            Ignore(p => p.EntityId);
        }
    }
}
