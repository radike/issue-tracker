using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTracker.Entities
{
    public class Project : BaseEntityWithHistorization
    {
        // Parameters
        [Required]
        [Display(Name = "Project title")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Code { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }

        [NotMapped]
        [Display(Name = "Users")]
        public ICollection<string> SelectedUsers { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}