using System;
using IssueTracker.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace IssueTracker.ViewModels
{
    public class ProjectViewModel : BaseVersioningViewModel
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

        public IPagedList<IssueIndexViewModel> IssuesPage { get; set; }

        [Display(Name = "Users")]
        public IEnumerable<Guid> SelectedUsers { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }

        [Display(Name = "Project owner")]
        public Guid OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }
        
    }
}