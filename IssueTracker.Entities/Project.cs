using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace IssueTracker.Entities
{
    public class Project
    {
        // Composite primary key
        [Key, Column(Order = 0)]
        public Guid Id { get; set; }

        [Key, Column(Order = 1)]
        public DateTime CreatedAt { get; set; }

        // Parameters
        [Required]
        [Display(Name = "Project title")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public bool Active { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }

        [NotMapped]
        [Display(Name = "Users")]
        public ICollection<string> SelectedUsers { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        [Display(Name = "Project owner")]
        public string OwnerId { get; set; }

        public ApplicationUser Owner {
            get { return Users == null ? null : Users.FirstOrDefault(u => u.Id == OwnerId); }
        }

    }
}