

using Entities;
using IssueTracker.Core;
using IssueTracker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Entities
{
    public class Issue : IIdentifiableEntity
    {
        // composite primary key
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        [Required]
        [ForeignKey("Project"), Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [ForeignKey("Project"), Column(Order = 1)]
        public DateTime ProjectCreatedAt { get; set; }

        [Required]
        [ForeignKey("State")]
        public Guid StateId { get; set; }

        [Required]
        [ForeignKey("Reporter")]
        public string ReporterId { get; set; }

        [ForeignKey("Assignee")]
        public string AssigneeId { get; set; }

        [Required]
        public bool Active { get; set; }

        // Parameters
        [Required]
        [Display(Name = "Issue title")]
        [MaxLength(255)]
        public string Name { get; set; }

        public State State { get; set; }

        [Required]
        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        [Display(Name = "Issue description")]
        public string Description { get; set; }

        [Required]
        public int CodeNumber { get; set; }

        // Table definitions
        public virtual Project Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        // Custom properties
        [NotMapped]
        public string Code
        {
            get { return Project.Code + "-" + CodeNumber; }
        }


        public Guid EntityId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
}
