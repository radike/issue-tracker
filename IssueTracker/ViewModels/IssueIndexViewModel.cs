using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IssueTracker.Locale;

namespace IssueTracker.ViewModels
{
    public class IssueIndexViewModel : BaseVersioningViewModel
    {
        // Parameters
        [Display(Name = "IssueTitle", ResourceType = typeof(IssueStrings))]
        public string Name { get; set; }

        [Display(Name = "IssueState", ResourceType = typeof(IssueStrings))]
        public StateViewModel State { get; set; }

        [Display(Name = "IssueCreated", ResourceType = typeof(IssueStrings))]
        public DateTime Created { get; set; }

        [Display(Name = "IssueDescription", ResourceType = typeof(IssueStrings))]
        [AllowHtml]
        public string Description { get; set; }

        public int CodeNumber { get; set; }

        // Table definitions
        [Display(Name = "IssueProjectId", ResourceType = typeof(IssueStrings))]
        public virtual ProjectViewModel Project { get; set; }

        [Display(Name = "IssueReporterId", ResourceType = typeof(IssueStrings))]
        public virtual ApplicationUser Reporter { get; set; }

        [Display(Name = "IssueAssigneeId", ResourceType = typeof(IssueStrings))]
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<CommentViewModel> Comments { get; set; }

        // Custom properties
        public string Code
        {
            get { return Project.Code + "-" + CodeNumber; }
        }
        
    }
}