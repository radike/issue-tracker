using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using Moq;
using IssueTracker.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IssueTracker.Entities;

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
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.Fetch()).Returns(issues.AsQueryable());

            var actual = issueService.GetAllIssues();

            Assert.AreEqual(actual.Count, 4);
        }

        [TestMethod]
        public void GetNewCodeNumberTest()
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
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug };

            List<Issue> issues = new List<Issue>()
            {
            };

            fakeIssueRepo.Setup(i => i.GetAll()).Returns(issues);

            var actual = issueService.GetNewCodeNumber();
            var expected = 1;

            Assert.AreEqual(expected, actual);

            issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.GetAll()).Returns(issues);

            actual = issueService.GetNewCodeNumber();
            expected = 5;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetByProjectCodeAndIssueNumberTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            Mock<ICommentRepository> fakeCommentRepo = new Mock<ICommentRepository>();

            var issueService = new IssueService(fakeIssueRepo.Object, fakeProjectRepo.Object, fakeCommentRepo.Object, fakeStateWorkflowRepo.Object);

            var firstProject = new Project() { Code = "COD"};
            var secodProject = new Project() { Code = "COP" };

            var firstIssueId = Guid.NewGuid();
            var secondIssueId = Guid.NewGuid();
            var thirdIssueId = Guid.NewGuid();
            var fourthIssueId = Guid.NewGuid();

            var firstIssue = new Issue() { Active = true, CodeNumber = 1, Description = "firstIssue", Id = firstIssueId, Name = "first", Type = Entities.IssueType.Bug, Project = firstProject };
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task, Project = secodProject };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question, Project = secodProject };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug, Project = secodProject };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.Fetch()).Returns(issues.AsQueryable());

            var actual = issueService.GetByProjectCodeAndIssueNumber("COD", 1);
            var expected = firstIssue;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIssueCountTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            Mock<ICommentRepository> fakeCommentRepo = new Mock<ICommentRepository>();

            var issueService = new IssueService(fakeIssueRepo.Object, fakeProjectRepo.Object, fakeCommentRepo.Object, fakeStateWorkflowRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();

            var firstProject = new Project() { Code = "COD", Id = firstProjectId };
            var secodProject = new Project() { Code = "COP", Id = secondProjectId };

            var firstIssueId = Guid.NewGuid();
            var secondIssueId = Guid.NewGuid();
            var thirdIssueId = Guid.NewGuid();
            var fourthIssueId = Guid.NewGuid();

            var firstIssue = new Issue() { Active = true, CodeNumber = 1, Description = "firstIssue", Id = firstIssueId, Name = "first", Type = Entities.IssueType.Bug, ProjectId = firstProjectId };
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task, ProjectId = firstProjectId };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question, ProjectId = secondProjectId };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug, ProjectId = secondProjectId };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.FindBy(It.IsAny<Expression<Func<Issue, bool>>>()))
                .Returns((Expression<Func<Issue, bool>> expression) => issues.AsQueryable().Where(expression));

            Project nullProject = null; 

            var actual = issueService.GetIssueCount(Entities.IssueType.Bug, nullProject, true);
            var expected = 2;

            Assert.AreEqual(expected, actual);

            actual = issueService.GetIssueCount(Entities.IssueType.Bug, secodProject, true);
            expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNewIssuesTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            Mock<ICommentRepository> fakeCommentRepo = new Mock<ICommentRepository>();

            var issueService = new IssueService(fakeIssueRepo.Object, fakeProjectRepo.Object, fakeCommentRepo.Object, fakeStateWorkflowRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();

            var firstProject = new Project() { Code = "COD", Id = firstProjectId };
            var secodProject = new Project() { Code = "COP", Id = secondProjectId };

            var newState = new State() { IsInitial = true };
            var inProgress = new State() { IsInitial = false };

            var firstIssueId = Guid.NewGuid();
            var secondIssueId = Guid.NewGuid();
            var thirdIssueId = Guid.NewGuid();
            var fourthIssueId = Guid.NewGuid();
            
            var firstIssue = new Issue() { Active = true, CodeNumber = 1, Description = "firstIssue", Id = firstIssueId, Name = "first", Type = Entities.IssueType.Bug, ProjectId = firstProjectId, State= newState};
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task, ProjectId = firstProjectId, State = inProgress };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question, ProjectId = secondProjectId, State = inProgress };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug, ProjectId = secondProjectId, State = newState };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.FindBy(It.IsAny<Expression<Func<Issue, bool>>>()))
                .Returns((Expression<Func<Issue, bool>> expression) => issues.AsQueryable().Where(expression));

            var actual = issueService.GetNewIssues(null).Count;
            var expected = 2;

            Assert.AreEqual(expected, actual);

            actual = issueService.GetNewIssues(firstProjectId).Count;
            expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRaisedIssuesCountTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            Mock<ICommentRepository> fakeCommentRepo = new Mock<ICommentRepository>();

            var issueService = new IssueService(fakeIssueRepo.Object, fakeProjectRepo.Object, fakeCommentRepo.Object, fakeStateWorkflowRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();

            var firstProject = new Project() { Code = "COD", Id = firstProjectId };
            var secodProject = new Project() { Code = "COP", Id = secondProjectId };

            var newState = new State() { IsInitial = true };
            var inProgress = new State() { IsInitial = false };



            var firstIssueId = Guid.NewGuid();
            var secondIssueId = Guid.NewGuid();
            var thirdIssueId = Guid.NewGuid();
            var fourthIssueId = Guid.NewGuid();

            var firstIssue = new Issue() { Active = true, CodeNumber = 1, Description = "firstIssue", Id = firstIssueId, Name = "first", Type = Entities.IssueType.Bug, ProjectId = firstProjectId, State = newState, Created = new DateTime(2015,1,22)};
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task, ProjectId = firstProjectId, State = inProgress, Created = new DateTime(2015, 2, 22) };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question, ProjectId = secondProjectId, State = inProgress, Created = new DateTime(2015, 3, 22) };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug, ProjectId = secondProjectId, State = newState, Created = new DateTime(2015, 1, 25) };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.FindBy(It.IsAny<Expression<Func<Issue, bool>>>()))
                .Returns((Expression<Func<Issue, bool>> expression) => issues.AsQueryable().Where(expression));

            var actual = issueService.GetRaisedIssueCount(new DateTime(2015, 1, 12), new DateTime(2015, 3, 21));
            var expected = 3;

            Assert.AreEqual(expected, actual);

            actual = issueService.GetRaisedIssueCount(firstProjectId, new DateTime(2015, 1, 12), new DateTime(2015, 3, 21));
            expected = 2;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetResolvedIssuesTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IStateWorkflowRepository> fakeStateWorkflowRepo = new Mock<IStateWorkflowRepository>();
            Mock<ICommentRepository> fakeCommentRepo = new Mock<ICommentRepository>();

            var issueService = new IssueService(fakeIssueRepo.Object, fakeProjectRepo.Object, fakeCommentRepo.Object, fakeStateWorkflowRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();

            var firstProject = new Project() { Code = "COD", Id = firstProjectId };
            var secodProject = new Project() { Code = "COP", Id = secondProjectId };

            var newState = new State() { IsInitial = true };
            var inProgress = new State() { IsInitial = false };



            var firstIssueId = Guid.NewGuid();
            var secondIssueId = Guid.NewGuid();
            var thirdIssueId = Guid.NewGuid();
            var fourthIssueId = Guid.NewGuid();

            var firstIssue = new Issue() { Active = true, CodeNumber = 1, Description = "firstIssue", Id = firstIssueId, Name = "first", Type = Entities.IssueType.Bug, ProjectId = firstProjectId, State = newState, ResolvedAt = new DateTime(2015, 1, 22) };
            var secondIssue = new Issue() { Active = true, CodeNumber = 2, Description = "secondIssue", Id = firstIssueId, Name = "second", Type = Entities.IssueType.Task, ProjectId = firstProjectId, State = inProgress, ResolvedAt = new DateTime(2015, 1, 28) };
            var thirdIssue = new Issue() { Active = true, CodeNumber = 3, Description = "thirdIssue", Id = firstIssueId, Name = "third", Type = Entities.IssueType.Question, ProjectId = secondProjectId, State = inProgress, ResolvedAt = new DateTime(2015, 3, 22) };
            var fourthIssue = new Issue() { Active = true, CodeNumber = 4, Description = "fourthIssue", Id = firstIssueId, Name = "fourth", Type = Entities.IssueType.Bug, ProjectId = secondProjectId, State = newState, ResolvedAt = new DateTime(2015, 1, 25) };

            List<Issue> issues = new List<Issue>()
            {
                firstIssue,secondIssue,thirdIssue,fourthIssue
            };

            fakeIssueRepo.Setup(i => i.FindBy(It.IsAny<Expression<Func<Issue, bool>>>()))
                .Returns((Expression<Func<Issue, bool>> expression) => issues.AsQueryable().Where(expression));

            var actual = issueService.GetResolvedIssues(firstProjectId ,new DateTime(2015, 1, 12), new DateTime(2015, 1, 25)).Count;
            var expected = 1;

            Assert.AreEqual(expected, actual);

            actual = issueService.GetResolvedIssues(firstProjectId, 2015, 1).Count;
            expected = 2;

            Assert.AreEqual(expected, actual);

            actual = issueService.GetResolvedIssues(secondProjectId, 2015, 1, 2).Count;
            expected = 1;

            Assert.AreEqual(expected, actual);
        }
    }
}
