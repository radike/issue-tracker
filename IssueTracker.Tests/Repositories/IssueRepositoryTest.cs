using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using Moq;
using IssueTracker.Data.Services;
using IssueTracker.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace IssueTracker.Tests.Repositories
{
    [TestClass]
    public class IssueRepositoryTest
    {
        [TestMethod]
        public void GetAllIssuesTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            Mock<ICommentRepository> fakeCommentRepo = new Mock<ICommentRepository>();

            var issueService = new IssueService(fakeIssueRepo.Object, fakeProjectRepo.Object, fakeCommentRepo.Object, fakeStateWorkflowRepo.Object);

            var firstIssueId = Guid.NewGuid();
            var secondIssueId = Guid.NewGuid();
            var thirdIssueId = Guid.NewGuid();
            var fourthIssueId = Guid.NewGuid();

            var firstIssue = new Issue() { Active = true, CodeNumber = 1, Description = "firstIssue", Id = firstIssueId, Name = "first", Type = Entities.IssueType.Bug };
            var secondIssue = new Issue() { Active = false, CodeNumber = 1, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 1, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 1, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.Fetch()).Returns(issues.AsQueryable());

            var actual = issueService.GetAllIssues();

            Assert.AreEqual(actual.Count, 4);
        }
    }
}
