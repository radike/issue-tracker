using System;
using System.Collections.Generic;
using Common.Data.Core.Contracts;

namespace IssueTracker.Entities
{
    public class Issue : IIdentifiableEntity, IVersionableEntity
    {
        // composite primary key
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public Guid ProjectId { get; set; }

        
        public DateTime ProjectCreatedAt { get; set; }
        
        public Guid StateId { get; set; }

        public Guid ReporterId { get; set; }

        public Guid? AssigneeId { get; set; }
        
        public bool Active { get; set; }

        // Parameters
        public IssueType Type { get; set; }

        public string Name { get; set; }

        public virtual State State { get; set; }
        
        public DateTime Created { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public string Description { get; set; }
        
        public int CodeNumber { get; set; }

        // Table definitions
        public virtual Project Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        // Custom properties
        public string Code
        {
            get { return Project.Code + "-" + CodeNumber; }
        }
    }
}
