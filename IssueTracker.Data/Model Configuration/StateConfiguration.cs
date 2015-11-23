using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data.Model_Configuration
{
    public class StateConfiguration : EntityTypeConfiguration<State>
    {
        public StateConfiguration()
        {
            Property(p => p.IsInitial).IsRequired();
            Property(p => p.Title).IsRequired();
            Ignore(p => p.EntityId);
        }
    }
}
