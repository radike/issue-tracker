using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using IssueTracker.DAL;
using IssueTracker.Entities;
using IssueTracker.ViewModels;
using PagedList;
using System.Collections.Generic;
using IssueTracker.Abstractions;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class IssuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private const int ProjectsPerPage = 20;

        // GET: Issues
        public ActionResult Index(int? page, string sort)
        {
            // viewbag items are used in the header to sort the records
            ViewBag.CreatedSort = string.IsNullOrEmpty(sort) ? "created_desc" : string.Empty;
            ViewBag.ReporterSort = sort == "reporter" ? "reporter_desc" : "reporter";
            ViewBag.ProjectSort = sort == "project" ? "project_desc" : "project";
            ViewBag.AssigneeSort = sort == "assignee" ? "assignee_desc" : "assignee";
            ViewBag.StatusSort = sort == "status" ? "status_desc" : "project";

            var issuesTemp = db.Issues
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .Include(p => p.State).ToList()
                .OrderByDescending(x => x.Created);

            var issues = Mapper.Map<IEnumerable<IssueIndexViewModel>>(issuesTemp);
            var pageNumber = page ?? 1;

            // logic implemented to sort the records by clicking on the header
            switch(sort)
            {
                case "reporter_desc":
                    issues = issues.OrderByDescending(ii => ii.Reporter);
                    break;
                case "reporter":
                    issues = issues.OrderBy(ii => ii.Reporter);
                    break;
                case "status_desc":
                    issues = issues.OrderByDescending(ii => ii.State);
                    break;
                case "status":
                    issues = issues.OrderBy(ii => ii.State);
                    break;
                case "assignee_desc":
                    issues = issues.OrderByDescending(ii => ii.Assignee);
                    break;
                case "assignee":
                    issues = issues.OrderBy(ii => ii.Assignee);
                    break;
                case "project_desc":
                    issues = issues.OrderByDescending(ii => ii.Project);
                    break;
                case "project":
                    issues = issues.OrderBy(ii => ii.Project);
                    break;
                case "created_desc":
                    issues = issues.OrderByDescending(ii => ii.Created);
                    break;
                default:
                    issues = issues.OrderBy(ii => ii.Created);
                    break;
            }

            return View(issues.ToPagedList(pageNumber, ProjectsPerPage));
        }

        // GET: Issues/Details/5
        public ActionResult Details(string id)
        {
            if (!Helper.CheckIssueCodePattern(id))
            {
                return new HttpStatusCodeResult((HttpStatusCode.BadRequest));
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var codeSplitted = Helper.SplitIssueCode(id);
            var projectCode = codeSplitted[0];
            var issueNumber = int.Parse(codeSplitted[1]);

            if (projectCode == null || issueNumber == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var issue = db.Issues
                .Where(i => i.Project.Code == projectCode && i.CodeNumber == issueNumber)
                .OrderByDescending(x => x.CreatedAt)
                .Include(i => i.State).First();

            var viewModel = new IssueDetailViewModel
            {
                Issue = Mapper.Map<IssueIndexViewModel>(issue)
            };

            if (viewModel.Issue == null)
            {
                return HttpNotFound();
            }

            // possible workflows
            var workflows = db.StateWorkflows.Where(c => c.FromState.Id == viewModel.Issue.State.Id);
            viewModel.StateWorkflows = Mapper.Map<IEnumerable<StateWorkflowViewModel>>(workflows);

            // comments from all versions of the issue
            var comments = db.Comments
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .Where(n => n.IssueId == issue.Id)
                .OrderBy(n => n.Posted)
                .Include(n => n.User)
                .ToList();

            viewModel.Comments = Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            foreach (var comment in viewModel.Comments)
            {
                comment.User = db.Users.Find(comment.AuthorId);
            }

            return View(viewModel);
        }

        // GET: Issues/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ErrorSQL = TempData["ErrorSQL"] as string;

            var projectsTemp = db.Projects
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .ToList();

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ProjectId = new SelectList(projectsTemp, "Id", "Title");
            ViewBag.ReporterId = getLoggedUser().Id;

            return View();
        }

        // POST: Issues/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssueCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var initialState = getInitialState();
                if (initialState == null)
                {
                    TempData["ErrorSQL"] = "There is no initial state. The issue couldn't be created.";

                    return RedirectToAction("Create");
                }

                var projectTemp = db.Projects
                    .Where(x => x.Id == viewModel.ProjectId)
                    .OrderByDescending(x => x.CreatedAt).First();

                var issue = Mapper.Map<Issue>(viewModel);
                issue.StateId = initialState.Id;
                issue.ReporterId = getLoggedUser().Id;
                issue.Created = DateTime.Now;
                issue.ProjectCreatedAt = projectTemp.CreatedAt;
                issue.Id = Guid.NewGuid();
                issue.CodeNumber = db.Issues.Max(x => (int?)x.CodeNumber) + 1 ?? 1;

                db.Issues.Add(issue);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", viewModel.AssigneeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", viewModel.ProjectId);

            return View(viewModel);
        }

        private State getInitialState()
        {
            try
            {
                return db.States.First(s => s.IsInitial);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private ApplicationUser getLoggedUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = db.Users.First(dbUser => dbUser.Email == User.Identity.Name);
                return user;
            }
            return null;
        }

        // GET: Issues/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var issue = db.Issues.Where(x => x.Id == id).OrderByDescending(x => x.CreatedAt).Include(x => x.Project).First();

            if (issue == null)
            {
                return HttpNotFound();
            }

            var projectsTemp = db.Projects
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .ToList();

            var viewModel = Mapper.Map<IssueEditViewModel>(issue);

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", issue.AssigneeId);
            ViewBag.ProjectId = new SelectList(projectsTemp, "Id", "Title", issue.ProjectId);
            ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);

            return View(viewModel);
        }

        // POST: Issues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AssigneeId,ProjectId")] IssueEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // create a new entity
                var entityNew = db.Issues.AsNoTracking().Where(x => x.Id == viewModel.Id).OrderByDescending(x => x.CreatedAt).First();
                // in case the project was changed
                if (viewModel.ProjectId != entityNew.ProjectId)
                {
                    var projectTemp = db.Projects.Where(x => x.Id == viewModel.ProjectId).OrderByDescending(x => x.CreatedAt).First();
                    entityNew.ProjectCreatedAt = projectTemp.CreatedAt;
                }
                // map viewModel to the entity
                entityNew = Mapper.Map(viewModel, entityNew);
                // change CreatedAt
                entityNew.CreatedAt = DateTime.Now;
                // save the entity
                db.Issues.Add(entityNew);
                db.SaveChanges();
                
                return RedirectToAction("Details", new { id = entityNew.Code });
                
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", viewModel.AssigneeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", viewModel.ProjectId);

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ChangeStatus(Guid issueId, Guid to)
        {

            // create a new entity
            var entityNew = db.Issues.AsNoTracking().Where(x => x.Id == issueId).OrderByDescending(x => x.CreatedAt).First();
            if (entityNew != null)
            {
                // change status
                entityNew.StateId = to;
                // change CreatedAt
                entityNew.CreatedAt = DateTime.Now;
                // save the entity
                db.Issues.Add(entityNew);
                db.SaveChanges();
            }

            if (HttpContext.Request.UrlReferrer != null)
            {
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            return RedirectToAction("Index");
        }
    }
}
