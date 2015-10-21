using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class IssueEditViewModel
    {
        // Id
        public Guid Id { get; set; }

        // Issue editable fields
        [Required]
        public Guid ProjectId { get; set; }

        public Guid AssigneeId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}