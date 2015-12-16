using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Entities;
using IssueTracker.Data.Services;
using IssueTracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class HomeController : Controller
    {
        private IIssueRepository _issueRepo;
        private IProjectService _projectService;
        private IIssueService _issueService;

        public HomeController(IProjectService projectService, IIssueService issueService, IIssueRepository issueRepository)
        {
            _projectService = projectService;
            _issueService = issueService;
            _issueRepo = issueRepository;
        }

        public ActionResult Index([Bind(Include = "ProjectId")] DashboardViewModel viewModel)
        {
            Guid userId = new Guid(User.Identity.GetUserId());
            var usersProjects = _projectService.GetProjectsForUser(userId);
            Project projectToDisplay = viewModel.ProjectId == null ? usersProjects.FirstOrDefault() : usersProjects.Single(p => p.Id == viewModel.ProjectId);

            viewModel.ProjectCode = projectToDisplay.Code;
            viewModel.QuestionCount = _issueService.GetIssueCount(Entities.IssueType.Question, projectToDisplay, false);
            viewModel.TaskCount = _issueService.GetIssueCount(Entities.IssueType.Task, projectToDisplay, false);
            viewModel.BugCount = _issueService.GetIssueCount(Entities.IssueType.Bug, projectToDisplay, false);
            ViewBag.UsersList = new SelectList(usersProjects, "Id", "Title");

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index", new { Culture = Abstractions.CultureHelper.CurrentCulture.Name });
        }

        public JsonResult AutoCompleteSearch(string query)
        {
            var allIssues = _issueRepo.Fetch()
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .Include(n => n.Project)
                .OrderByDescending(x => x.Created);

            var result = allIssues.Where(x => (x.Project.Code + x.CodeNumber + ": " + x.Name).ToLower().Contains(query.ToLower())).Select(x => new { x.Id, Code = x.Project.Code + "-" + x.CodeNumber, Title = x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}