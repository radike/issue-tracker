using IssueTracker.Data;
using IssueTracker.Data.Data_Repositories;
using IssueTracker.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IssueTracker.Tests.Repositories
{
    [TestClass]
    public class StateRepositoryTest
    {
        [TestMethod]
        public void GetStatesTest()
        {
            var data = new List<State>
            {
                new State() {Title = "Open", IsInitial = true, OrderIndex = 1},
                new State() {Title = "Build", IsInitial = false, OrderIndex = 2},
                new State() {Title = "Closed", IsInitial = false, OrderIndex = 3}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<State>>();

            mockSet.As<IQueryable<State>>().Setup(m => m.Provider)
                    .Returns(data.Provider);
            mockSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IssueTrackerContext>();
            mockContext.Setup(m => m.States).Returns(mockSet.Object);
            
            var service = new StateRepository(mockContext.Object);

            var actual = service.GetInitialState();

            Assert.Equals(actual.Title, "Open");
        }
    }
}
