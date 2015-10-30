using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.ViewModels
{
    public class StateWorkflowViewModel : BaseViewModel
    {
        // Foreign keys
        public Guid FromStateId { get; set; }

        public Guid ToStateId { get; set; }

        // Table definitions
        public StateViewModel FromState { get; set; }
        public StateViewModel ToState { get; set; }

    }
}