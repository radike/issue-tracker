using System;
using System.Collections.Generic;
using Common.Data.Core.Contracts;

namespace IssueTracker.Entities
{
    public class State : IIdentifiableEntity
    {
        // Parameters
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsInitial { get; set; }
        public string Colour { get; set; }
        public int OrderIndex { get; set; }

        // Table definitions
        public virtual ICollection<Issue> Issues { get; set; }
    }
}