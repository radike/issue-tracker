using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
