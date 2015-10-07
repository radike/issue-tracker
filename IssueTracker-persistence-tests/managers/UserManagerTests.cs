using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using IssueTracker_persistence.entities;
using IssueTracker_persistence.managers;

namespace IssueTracker_persistence_tests.managers
{
    [TestClass]
    public class UserManagerTests
    {
        [TestMethod]
        public void CreateUser_saves_a_user_via_context()
        {
            var mockSet = new Mock<DbSet<User>>();

            var mockContext = new Mock<IssueTrackerDbContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            var manager = new UserManager(mockContext.Object);
            var user = new User();
            manager.CreateUser(user);

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
