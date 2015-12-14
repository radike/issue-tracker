using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class StateWorkflowViewModel : BaseViewModel
    {
        // Foreign keys
        [Display(Name = "StateWorkflowFrom", ResourceType = typeof(Locale.StateWorkflowStrings))]
        public Guid FromStateId { get; set; }

        [Display(Name = "StateWorkflowTo", ResourceType = typeof(Locale.StateWorkflowStrings))]
        public Guid ToStateId { get; set; }

        // Table definitions
        public StateViewModel FromState { get; set; }
        public StateViewModel ToState { get; set; }

    }
}