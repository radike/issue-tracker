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
        [Display(Name = "Project")]
        public Guid ProjectId { get; set; }

        [Required]
        public IssueType Type { get; set; }

        [Display(Name = "IssueAssigneeId", ResourceType = typeof(IssueStrings))]
        public string AssigneeId { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public string Code { get; set; }
    }
}