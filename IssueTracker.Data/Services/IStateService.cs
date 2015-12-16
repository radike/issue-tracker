using System;
using System.Collections.Generic;
using IssueTracker.Entities;

namespace IssueTracker.Data.Services
{
    public interface IStateService
    {
        IEnumerable<Guid> GetFinalStateIds();

        IEnumerable<State> GetStatesOrderedByIndex();
        ICollection<State> GetInitialStates();
    }
}
