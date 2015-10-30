using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class IssueViewModel : BaseViewModel
    {
        // Foreign keys
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid StateId { get; set; }

        [Required]
        public string ReporterId { get; set; }

        public string AssigneeId { get; set; }

        // Parameters
        [Required]
        [Display(Name = "Issue title")]
        [MaxLength(255)]
        public string Name { get; set; }

        public StateViewModel State { get; set; }

        [Required]
        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        [Display(Name = "Issue description")]
        public string Description { get; set; }

        // Table definitions
        public virtual ProjectViewModel Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<CommentViewModel> Comments { get; set; }
    }
}