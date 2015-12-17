using System;
using System.Collections.Generic;
using Common.Data.Core.Contracts;

namespace IssueTracker.Entities
{
    public class Project : IIdentifiableEntity, IVersionableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Parameters
        public string Title { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }

        public ICollection<Guid> SelectedUsers { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Guid OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }
    }
}