using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class IssueCreateViewModel
    {
        [Required]
        public Guid ProjectId { get; set; }

        public string AssigneeId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}