using Entities;
using IssueTracker.Core.Contracts;
using System;
using System.Collections.Generic;

namespace IssueTracker.Data.Contracts.Repository_Interfaces
{
    public interface IStateRepository : IDataRepository<State>
    {
        IEnumerable<State> GetStatesOrderedByIndex();
        State GetInitialState();
        IEnumerable<Guid> GetFinalStateIds();
    }
}