using System;

namespace IssueTracker.ViewModels
{
    public class BaseVersioningViewModel
    {
        public Guid Id { get; set; }

        // properties for historization of data
        public DateTime CreatedAt { get; set; }

        public bool Active { get; set; }

        public BaseVersioningViewModel()
        {
            CreatedAt = DateTime.Now;
            Active = true;
        }
    }
}