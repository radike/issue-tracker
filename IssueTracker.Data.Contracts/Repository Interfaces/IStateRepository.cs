using Common.Data.Core.Contracts;
using System;
using System.Collections.Generic;
using IssueTracker.Entities;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface IStateRepository : IDataRepository<State>
    {
        IEnumerable<State> GetStatesOrderedByIndex();
        State GetInitialState();

        int GetStatesOrderIndex();

        List<State> GetMovedStates(int toPosition, int fromPosition);
    }
}