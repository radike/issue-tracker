using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Comment  : IIdentifiableEntity
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

        public string AuthorId { get; set; }

        // Table definitions
        public virtual ApplicationUser User { get; set; }

        public virtual Issue Issue { get; set; }

        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
