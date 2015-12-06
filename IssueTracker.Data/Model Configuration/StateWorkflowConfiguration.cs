using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Model_Configuration
{
    public class StateWorkflowConfiguration : EntityTypeConfiguration<StateWorkflow>
    {
        public StateWorkflowConfiguration()
        {
            Property(p => p.FromStateId).IsRequired();
            Property(p => p.ToStateId).IsRequired();
            Ignore(p => p.FromState);
            Ignore(p => p.ToState);
        }
    }
}
