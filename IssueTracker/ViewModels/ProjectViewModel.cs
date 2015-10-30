using IssueTracker.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {
        // Parameters
        [Required]
        [Display(Name = "Project title")]
        [MaxLength(255)]
        public string Title { get; set; }

        // Table definitions
        public virtual IEnumerable<IssueViewModel> Issues { get; set; }

        [Display(Name = "Users")]
        public IEnumerable<string> SelectedUsers { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }
    }
}