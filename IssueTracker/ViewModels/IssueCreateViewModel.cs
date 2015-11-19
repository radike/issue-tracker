using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        [AllowHtml]
        public string Description { get; set; }
    }
}