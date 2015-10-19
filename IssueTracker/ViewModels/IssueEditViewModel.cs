using System;

namespace IssueTracker.ViewModels
{
    public class IssueEditViewModel
    {
        // Id
        public Guid Id { get; set; }
        
        // Issue editable fields
        public Guid ProjectId { get; set; }
        public Guid AssigneeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}