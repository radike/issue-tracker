using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IssueTracker.Data.Data_Repositories;
using IssueTracker.Services;
using IssueTracker.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.Data.Contracts.Repository_Interfaces;

namespace IssueTracker.Tests.Repositories
{
    [TestClass]
    public class StateRepositoryTest
    {
        [TestMethod]
        public void GetFinalStatesIdsTest()
        {
            Mock<IStateRepository> fakeStateRepo = new Mock<IStateRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            var stateService = new StateService(fakeStateRepo.Object, fakeStateWorkflowRepo.Object);

            var openStateId = Guid.NewGuid();
            var closedStateId = Guid.NewGuid();
            List<State> states = new List<State>()
            {
                new State {Title = "Open", Colour = "black", IsInitial = true, OrderIndex = 1, Id = openStateId},
                new State {Title = "Closed", Colour = "white", IsInitial = false, OrderIndex = 2, Id = closedStateId}
            };

            List<StateWorkflow> workflows = new List<StateWorkflow>()
            {
                new StateWorkflow {FromStateId = openStateId, ToStateId = closedStateId }
            };

            fakeStateRepo.Setup(i => i.Fetch()).Returns(states.AsQueryable());
            fakeStateWorkflowRepo.Setup(i => i.Fetch()).Returns(workflows.AsQueryable());

            var actual = stateService.GetFinalStateIds();

            Assert.AreEqual(actual.FirstOrDefault(), closedStateId);
        }
    }
}
