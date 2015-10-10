using System.Collections.Generic;

namespace IssueTracker.Models
{
    public class State : BaseEntity
    {
        // Prameters
        public string Title { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }
    }
}