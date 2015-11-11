using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTracker.Entities
{
    public class Comment
    {
        // Composite primary key
        [Key, Column(Order = 0)]
        public Guid Id { get; set; }

        [Key, Column(Order = 1)]
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
        
    }
}