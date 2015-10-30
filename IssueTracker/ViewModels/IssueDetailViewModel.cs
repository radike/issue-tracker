using System.Collections.Generic;

namespace IssueTracker.ViewModels
{
    public class IssueDetailViewModel
    {
        // Issue deatil
        public IssueViewModel Issue { get; set; }

        // Change status, possible workflows
        public IEnumerable<StateWorkflowViewModel> StateWorkflows { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

    }
}