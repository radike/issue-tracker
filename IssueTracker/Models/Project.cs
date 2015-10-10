using System.Collections.Generic;

namespace IssueTracker.Models
{
    public class Project : BaseEntity
    {
        // Parameters
        public string Title { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}