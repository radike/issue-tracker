using System.Collections.Generic;
using IssueTracker.Models;

namespace IssueTracker.ViewModels
{
    public class IssueDetailViewModel
    {
        // Issue deatil
        public Issue Issue { get; set; }

        // Change status, possible workflows
        public IEnumerable<StateWorkflow> StateWorkflows { get; set; }

    }
}