using Common.Data.Core.Contracts;
using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;

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