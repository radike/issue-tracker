using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace IssueTracker.Models
{
    public class Project : BaseEntity
    {
        // Parameters
        [Required]
        [Display(Name = "Project title")]
        [MaxLength(255)]
        public string Title { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }

        [NotMapped]
        [Display(Name = "Users")]
        public ICollection<string> SelectedUsers { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}