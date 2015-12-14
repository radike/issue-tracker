using IssueTracker.Data.Contracts.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IssueTracker.Data.Entities;

namespace IssueTracker.Services
{
    public class StateService : IStateService
    {
        private IStateRepository _stateRepo;
        private IStateWorkflowRepository _stateWorkflowRepo;

        public StateService(IStateRepository stateRepository, IStateWorkflowRepository stateWorkflowRepository )
        {
            _stateRepo = stateRepository;
            _stateWorkflowRepo = stateWorkflowRepository;
        }


        public IEnumerable<Guid> GetFinalStateIds()
        {
            var statesWithTransition = _stateWorkflowRepo.Fetch().GroupBy(x => x.FromStateId).Select(g => g.FirstOrDefault()).Select(x => x.FromStateId);
            var allStates = _stateRepo.Fetch().Select(x => x.Id);

            return allStates.Except(statesWithTransition).ToList();
        }

        public IEnumerable<State> GetStatesOrderedByIndex()
        {
            return _stateRepo.GetStatesOrderedByIndex();
        }
    }
}