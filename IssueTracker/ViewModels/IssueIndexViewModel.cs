using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IssueTracker.ViewModels
{
    public class IssueIndexViewModel : BaseVersioningViewModel
    {
        // Parameters
        [Display(Name = "Issue title")]
        public string Name { get; set; }

        public StateViewModel State { get; set; }

        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        [Display(Name = "Issue description")]
        [AllowHtml]
        public string Description { get; set; }

        public int CodeNumber { get; set; }

        // Table definitions
        public virtual ProjectViewModel Project { get; set; }
        public virtual ApplicationUser Reporter { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public virtual ICollection<CommentViewModel> Comments { get; set; }

        // Custom properties
        public string Code
        {
            get { return Project.Code + "-" + CodeNumber; }
        }
        
    }
}