using System.Data.Entity.ModelConfiguration;
using IssueTracker.Entities;

namespace IssueTracker.Data.Model_Configuration
{
    public class StateWorkflowConfiguration : EntityTypeConfiguration<StateWorkflow>
    {
        public StateWorkflowConfiguration()
        {
            Property(p => p.FromStateId).IsRequired();
            Property(p => p.ToStateId).IsRequired();
        }
    }
}