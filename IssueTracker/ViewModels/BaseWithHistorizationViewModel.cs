using System;

namespace IssueTracker.ViewModels
{
    public class BaseWithHistorizationViewModel
    {
        public Guid Id { get; set; }

        // properties for historization of data
        public DateTime? DeletedAt { get; set; }

        public int CodeNumber { get; set; }
    }
}