using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class DashboardViewModel
    {
        [Display(Name = "DashboardSelectProject", ResourceType = typeof(Locale.SharedStrings))]
        public Guid? ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public int QuestionCount { get; set; }
        public int TaskCount { get; set; }
        public int BugCount { get; set; }
    }
}