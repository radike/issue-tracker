using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class State : BaseEntity
    {
        // Prameters
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Is initial?")]
        public bool IsInitial { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }
    }
}