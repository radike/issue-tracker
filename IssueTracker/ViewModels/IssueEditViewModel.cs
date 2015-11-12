using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class IssueEditViewModel : BaseViewModel
    {
        // Issue editable fields
        [Required]
        [Display(Name = "Project")]
        public Guid ProjectId { get; set; }

        [Display(Name = "Assignee")]
        public string AssigneeId { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Code { get; set; }
    }
}