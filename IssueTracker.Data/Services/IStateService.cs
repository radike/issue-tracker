using IssueTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Services
{
    public interface IStateService
    {
        IEnumerable<Guid> GetFinalStateIds();

        IEnumerable<State> GetStatesOrderedByIndex();
    }
}
