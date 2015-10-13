using System;

namespace IssueTracker.Models
{
    public class Comment : BaseEntity
    {
        // Foreign keys
        public Guid IssueId { get; set; }

        // Parameters
        public string Text { get; set; }
        public DateTime? Posted { get; set; }

        // Table definitions
        public ApplicationUser User { get; set; }
        public virtual Issue Issue { get; set; }
    }
}