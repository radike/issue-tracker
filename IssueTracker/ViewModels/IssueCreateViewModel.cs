using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class IssueCreateViewModel : BaseVersioningViewModel
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public DateTime ProjectCreatedAt { get; set; }

        public string AssigneeId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}