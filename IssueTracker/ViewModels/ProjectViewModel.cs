using IssueTracker.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class ProjectViewModel : BaseWithHistorizationViewModel
    {
        // Parameters
        [Required]
        [Display(Name = "Project title")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Code { get; set; }

        // Table definitions
        public virtual IEnumerable<IssueIndexViewModel> Issues { get; set; }

        [Display(Name = "Users")]
        public IEnumerable<string> SelectedUsers { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }
    }
}