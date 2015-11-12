using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Entities
{
    public class State : BaseEntity
    {
        // Parameters
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Is initial?")]
        public bool IsInitial { get; set; }

        public string Colour { get; set; }

        public int OrderIndex { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }
    }
}