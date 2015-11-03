using IssueTracker.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class CommentViewModel : BaseEntityWithHistorization
    {
        // Foreign keys
        [Required]
        [Display(Name = "Issue")]
        public Guid IssueId { get; set; }

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
    }
}