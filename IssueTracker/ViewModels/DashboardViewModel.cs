using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class DashboardViewModel
    {
        [Display(Name = "Select Project:")]
        public Guid? ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public int QuestionCount { get; set; }
        public int TaskCount { get; set; }
        public int BugCount { get; set; }
    }
}