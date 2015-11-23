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
        [Required]
        [Display(Name = "Issue")]
        [ForeignKey("Issue"), Column(Order = 2)]
        public Guid IssueId { get; set; }

        [ForeignKey("Issue"), Column(Order = 3)]
        public DateTime IssueCreatedAt { get; set; }

        [Required]
        public bool Active { get; set; }

        // Parameters
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Posted on")]
        public DateTime? Posted { get; set; }

        [Display(Name = "Author")]
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
