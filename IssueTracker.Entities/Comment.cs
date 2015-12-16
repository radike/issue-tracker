using System;
using Common.Data.Core.Contracts;

namespace IssueTracker.Entities
{
    public class Comment : IIdentifiableEntity, IVersionableEntity
    {
        // composite primary key
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public Guid IssueId { get; set; }

        public DateTime IssueCreatedAt { get; set; }

        public bool Active { get; set; }

        // Parameters
        public string Text { get; set; }

        public DateTime? Posted { get; set; }

        public Guid? AuthorId { get; set; }

        // Table definitions
        public virtual ApplicationUser Author { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
