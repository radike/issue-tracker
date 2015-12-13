using System.Collections.Generic;
using IssueTracker.Controllers;

namespace IssueTracker.ViewModels
{
    public class IssueDetailViewModel
    {
        // Issue deatil
        public IssueIndexViewModel Issue { get; set; }

        // Change status, possible workflows
        public IEnumerable<StateWorkflowViewModel> StateWorkflows { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public IEnumerable<IssuesController.IssueChange> Changes { get; set; }

    }
}