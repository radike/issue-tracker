using IssueTracker.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IssueTracker.Locale;

namespace IssueTracker.ViewModels
{
    public class IssueCreateViewModel : BaseVersioningViewModel
    {
        [Required]
        [Display(Name = "IssueProjectId", ResourceType = typeof(IssueStrings))]
        public Guid ProjectId { get; set; }

        [Required]
        public DateTime ProjectCreatedAt { get; set; }

        [Required]
        public IssueType Type { get; set; }

        [Display(Name = "IssueAssigneeId", ResourceType = typeof(IssueStrings))]        public string AssigneeId { get; set; }

        [Required]
        [Display(Name = "IssueTitle", ResourceType = typeof(IssueStrings))]
        public string Name { get; set; }

        [AllowHtml]
        [Display(Name = "IssueDescription", ResourceType = typeof(IssueStrings))]
        public string Description { get; set; }
    }
}