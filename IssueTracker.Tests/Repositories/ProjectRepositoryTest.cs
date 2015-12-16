using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using Moq;
using IssueTracker.Data.Services;
using IssueTracker.Data.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace IssueTracker.Tests.Repositories
{
    [TestClass]
    public class ProjectRepositoryTest
    {
        [TestMethod]
        public void GetProjectCodeTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IApplicationUserRepository> fakeUserRepo = new Mock<IApplicationUserRepository>();

            var projectService = new ProjectService(fakeProjectRepo.Object, fakeIssueRepo.Object, fakeUserRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();

            List<Project> projects = new List<Project>()
            {
                new Project() {Active = true, Code = "my-code", CreatedAt = DateTime.Now, Id = firstProjectId, Title = "first project title" },
                new Project() {Active = true, Code = "secondCode", CreatedAt = DateTime.Now, Id = secondProjectId, Title = "second project" }
            };

            fakeProjectRepo.Setup(i => i.FindSingleBy(It.IsAny<Expression<Func<Project, bool>>>()))
                .Returns((Expression<Func<Project, bool>> expression) => projects.AsQueryable().Where(expression).SingleOrDefault());

            var actual = projectService.GetProjectId("secondCode");

            Assert.AreEqual(actual.Value, secondProjectId);
        }

        [TestMethod]
        public void GetProjectsTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IApplicationUserRepository> fakeUserRepo = new Mock<IApplicationUserRepository>();

            var projectService = new ProjectService(fakeProjectRepo.Object, fakeIssueRepo.Object, fakeUserRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();
            var thirdProjectId = Guid.NewGuid();

            List<Project> projects = new List<Project>()
            {
                new Project() {Active = true, Code = "my-code", CreatedAt = DateTime.Now, Id = firstProjectId, Title = "first project title" },
                new Project() {Active = true, Code = "secondCode", CreatedAt = DateTime.Now, Id = secondProjectId, Title = "second project" },
                new Project() {Active = true, Code = "secondCode", CreatedAt = DateTime.Now.AddDays(1), Id = secondProjectId, Title = "second project with edited title" },
                new Project() {Active = false, Code = "thirdCode", CreatedAt = DateTime.Now, Id = thirdProjectId, Title = "third project" }
            };

            fakeProjectRepo.Setup(i => i.Fetch()).Returns(projects.AsQueryable());

            var actual = projectService.GetProjects();

            Assert.AreEqual(actual.Count(), 2);
        }

        [TestMethod]
        public void GetProjectTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IApplicationUserRepository> fakeUserRepo = new Mock<IApplicationUserRepository>();

            var projectService = new ProjectService(fakeProjectRepo.Object, fakeIssueRepo.Object, fakeUserRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();
            var thirdProjectId = Guid.NewGuid();

            List<Project> projects = new List<Project>()
            {
                new Project() {Active = true, Code = "my-code", CreatedAt = DateTime.Now, Id = firstProjectId, Title = "first project title" },
                new Project() {Active = true, Code = "secondCode", CreatedAt = DateTime.Now, Id = secondProjectId, Title = "second project" },
                new Project() {Active = true, Code = "secondCode", CreatedAt = DateTime.Now.AddDays(1), Id = secondProjectId, Title = "second project with edited title" },
                new Project() {Active = false, Code = "thirdCode", CreatedAt = DateTime.Now, Id = thirdProjectId, Title = "third project" }
            };

            fakeProjectRepo.Setup(i => i.Fetch()).Returns(projects.AsQueryable());

            var actualFirstTile = projectService.GetProject(firstProjectId).Title;
            var actualSecond = projectService.GetProject("secondCode").Title;

            var expectedFirstTitle = "first project title";
            var expectedSecondTitle = "second project with edited title";

            Assert.AreEqual(expectedFirstTitle, actualFirstTile);
            Assert.AreEqual(expectedSecondTitle, actualSecond);
        }

        [TestMethod]
        public void GetProjectsForUserTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IApplicationUserRepository> fakeUserRepo = new Mock<IApplicationUserRepository>();

            var projectService = new ProjectService(fakeProjectRepo.Object, fakeIssueRepo.Object, fakeUserRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();
            var thirdProjectId = Guid.NewGuid();

            var firstUserId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            var thirdUserId = Guid.NewGuid();
            var fourthUserId = Guid.NewGuid();

            List<ApplicationUser> firstAndSecondUsers = new List<ApplicationUser>()
            {
                new ApplicationUser {Id = firstUserId, UserName = "first user" },
                new ApplicationUser {Id = secondUserId, UserName = "second user" }
            };

            List<ApplicationUser> thirdAndFourth = new List<ApplicationUser>()
            {
                new ApplicationUser {Id = thirdUserId, UserName = "third user" },
                new ApplicationUser {Id = fourthUserId, UserName = "fourth user" }
            };

            var firstProject = new Project() { Active = true, Code = "my-code", CreatedAt = DateTime.Now, Id = firstProjectId, Title = "first project title", Users = firstAndSecondUsers, OwnerId = thirdUserId };
            var secondProject = new Project() { Active = true, Code = "secondCode", CreatedAt = DateTime.Now, Id = secondProjectId, Title = "second project", Users = thirdAndFourth, OwnerId = firstUserId };
            var secondsecondProject = new Project() { Active = true, Code = "secondCode", CreatedAt = DateTime.Now.AddDays(1), Id = secondProjectId, Title = "second project with edited title", Users = thirdAndFourth, OwnerId = firstUserId };
            var thirdProject = new Project() { Active = false, Code = "thirdCode", CreatedAt = DateTime.Now, Id = thirdProjectId, Title = "third project", Users = thirdAndFourth, OwnerId = fourthUserId };

            List<Project> projects = new List<Project>()
            {
                firstProject,
                secondProject,
                secondsecondProject,
                thirdProject
            };

            fakeProjectRepo.Setup(i => i.FindBy(It.IsAny<Expression<Func<Project, bool>>>()))
                .Returns((Expression<Func<Project, bool>> expression) => projects.AsQueryable().Where(expression));

            var actual = projectService.GetProjectsForUser(firstUserId);

            Assert.AreEqual(actual.Contains(firstProject), true);
            Assert.AreEqual(actual.Contains(secondsecondProject), true);
        }

        [TestMethod]
        public void ProjectCodeIsNotUniqueTest()
        {
            Mock<IProjectRepository> fakeProjectRepo = new Mock<IProjectRepository>();
            Mock<IIssueRepository> fakeIssueRepo = new Mock<IIssueRepository>();
            Mock<IApplicationUserRepository> fakeUserRepo = new Mock<IApplicationUserRepository>();

            var projectService = new ProjectService(fakeProjectRepo.Object, fakeIssueRepo.Object, fakeUserRepo.Object);

            var firstProjectId = Guid.NewGuid();
            var secondProjectId = Guid.NewGuid();
            var thirdProjectId = Guid.NewGuid();

            var firstProject = new Project() { Active = true, Code = "my-code", CreatedAt = DateTime.Now, Id = firstProjectId, Title = "first project title" };
            var secondProject = new Project() { Active = true, Code = "SECONDCODE", CreatedAt = DateTime.Now, Id = secondProjectId, Title = "second project" };
            var secondsecondProject = new Project() { Active = true, Code = "SECONDCODE", CreatedAt = DateTime.Now.AddDays(1), Id = secondProjectId, Title = "second project with edited title" };
            var thirdProject = new Project() { Active = false, Code = "thirdCode", CreatedAt = DateTime.Now, Id = thirdProjectId, Title = "third project" };

            List<Project> projects = new List<Project>()
            {
                firstProject,
                secondProject,
                secondsecondProject,
                thirdProject
            };

            fakeProjectRepo.Setup(i => i.GetAll()).Returns(projects);

            var actual = projectService.ProjectCodeIsNotUnique("secondCode");
            var expected = true;

            Assert.AreEqual(expected, actual);

            actual = projectService.ProjectCodeIsNotUnique("newcode");
            expected = false;

            Assert.AreEqual(expected, actual);
        }
    }
}
