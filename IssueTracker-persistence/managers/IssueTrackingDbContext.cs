using IssueTracker_persistence.entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker_persistence.managers
{
    public class IssueTrackerDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
    }
}
