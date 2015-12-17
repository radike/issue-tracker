using System;
using Common.Data.Core.Contracts;

namespace IssueTracker.Entities
{
    public class StateWorkflow : IIdentifiableEntity
    {
        public Guid Id { get; set; }

        // Foreign keys
        public Guid FromStateId { get; set; }

        public Guid ToStateId { get; set; }

        // Table definitions
        public virtual State FromState { get; set; }
        public virtual State ToState { get; set; }
    }
}