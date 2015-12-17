using IssueTracker.Entities;
using IssueTracker.Locale;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IssueTracker.ViewModels
{
    public class IssueEditViewModel
    {
        // Issue editable fields
        [Required]
        [Display(Name = "IssueProjectId", ResourceType = typeof(IssueStrings))]
        public Guid ProjectId { get; set; }

        [Required]
        [Display(Name = "IssueType", ResourceType = typeof(IssueStrings))]
        public IssueType Type { get; set; }

        [Display(Name = "IssueAssigneeId", ResourceType = typeof(IssueStrings))]
        public string AssigneeId { get; set; }

        [Required]
        [Display(Name = "IssueTitle", ResourceType = typeof(IssueStrings))]
        public string Name { get; set; }

        [AllowHtml]
        [Display(Name = "IssueDescription", ResourceType = typeof(IssueStrings))]
        public string Description { get; set; }

        public string Code { get; set; }
    }
}