using System;
using System.Collections.Generic;

namespace IssueTracker.Models
{
    public class Issue : BaseEntity
    {
        // Foreign keys
        public Guid ProjectId { get; set; }
        public Guid StateId { get; set; }
        public string ReporterId { get; set; }
        public string AssigneeId { get; set; }

        // Parameters
        public string Name { get; set; }
        public State State { get; set; }
        public string Description { get; set; }

        // Table definitions
        public virtual Project Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}