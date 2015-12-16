using System.Data.Entity.ModelConfiguration;
using IssueTracker.Entities;

namespace IssueTracker.Data.Model_Configuration
{
    public class StateConfiguration : EntityTypeConfiguration<State>
    {
        public StateConfiguration()
        {
            Property(p => p.IsInitial).IsRequired();
            Property(p => p.Title).IsRequired();
        }
    }
}
