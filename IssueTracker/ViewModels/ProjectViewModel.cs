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
        [Display(Name = "ProjectTitle", ResourceType = typeof(IssueTracker.Locale.ProjectStrings))]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "ProjectCode", ResourceType = typeof(IssueTracker.Locale.ProjectStrings))]
        public string Code { get; set; }

        // Table definitions
        public virtual IEnumerable<IssueIndexViewModel> Issues { get; set; }

        public IPagedList<IssueIndexViewModel> IssuesPage { get; set; }

        [Display(Name = "ProjectUsers", ResourceType = typeof(IssueTracker.Locale.ProjectStrings))]
        public IEnumerable<Guid> SelectedUsers { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }

        [Display(Name = "ProjectOwner", ResourceType = typeof(IssueTracker.Locale.ProjectStrings))]
        public Guid OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }
        
    }
}