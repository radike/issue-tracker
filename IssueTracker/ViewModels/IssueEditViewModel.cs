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

        [Display(Name = "Assignee")]
        public string AssigneeId { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public string Code { get; set; }
    }
}