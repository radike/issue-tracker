using IssueTracker.Data.Entities;
using IssueTracker.Locale;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IssueTracker.ViewModels
{
    public class CommentViewModel : BaseVersioningViewModel
    {
        // Foreign keys
        [Required]
        [Display(Name = "CommentIssue", ResourceType = typeof(CommentStrings))]
        public Guid IssueId { get; set; }

        [Required]
        public DateTime IssueCreatedAt { get; set; }

        // Parameters
        [Required]
        [Display(Name = "CommentText", ResourceType = typeof(CommentStrings))]
        [AllowHtml]
        public string Text { get; set; }

        [Display(Name = "CommentPostedOn", ResourceType = typeof(CommentStrings))]
        public DateTime? Posted { get; set; }

        [Display(Name = "CommentAuthor", ResourceType = typeof(CommentStrings))]
        public Guid AuthorId { get; set; }

        // Table definitions
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "CommentIssue", ResourceType = typeof(CommentStrings))]
        public virtual Issue Issue { get; set; }
        
    }
}