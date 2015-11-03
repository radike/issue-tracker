using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTracker.Entities
{
    public class Issue : BaseEntityWithHistorization
    {
        // Foreign keys
        [Required]
        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }

        [Required]
        [ForeignKey("State")]
        public Guid StateId { get; set; }

        [Required]
        [ForeignKey("Reporter")]
        public string ReporterId { get; set; }

        [ForeignKey("Assignee")]
        public string AssigneeId { get; set; }

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

        // Table definitions
        public virtual Project Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        // Custom properties
        [NotMapped]
        public string Code => Project.Code + "-" + CodeNumber;
    }
}