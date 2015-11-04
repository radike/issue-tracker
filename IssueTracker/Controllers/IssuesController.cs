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
    public class IssuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private const int ProjectsPerPage = 20;

        // GET: Issues
        public ActionResult Index(int? page, string sort)
        {
            // viewbag items are used in the header to sort the records
            ViewBag.CreatedSort = String.IsNullOrEmpty(sort) ? "created_desc" : string.Empty;
            ViewBag.ReporterSort = sort == "reporter" ? "reporter_desc" : "reporter";
            ViewBag.ProjectSort = sort == "project" ? "project_desc" : "project";
            ViewBag.AssigneeSort = sort == "assignee" ? "assignee_desc" : "assignee";
            ViewBag.StatusSort = sort == "status" ? "status_desc" : "project";

            var issues = Mapper.Map<IEnumerable<IssueViewModel>>(db.Issues.Where(i => i.DeletedAt == null).OrderByDescending(i => i.Created).Include(i => i.State));
            int pageNumber = page ?? 1;

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

            var viewModel = new IssueDetailViewModel
            {
                Issue = Mapper.Map<IssueViewModel>(db.Issues.Where(i => i.Project.Code == projectCode && i.CodeNumber == issueNumber && i.DeletedAt == null).Include(i => i.State).First())
            };

            if (viewModel.Issue == null)
            {
                return HttpNotFound();
            }

            // possible workflows
            var workflows = db.StateWorkflows.Where(c => c.FromState.Id == viewModel.Issue.State.Id);
            viewModel.StateWorkflows = Mapper.Map<IEnumerable<StateWorkflowViewModel>>(workflows);
            // comments from all versions of the issue
            var comments = db.Comments.Where(c => c.Issue.Project.Code == projectCode && c.Issue.CodeNumber == issueNumber && c.DeletedAt == null).OrderBy(o => o.Posted).ToList();
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

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            ViewBag.StateId = new SelectList(db.States, "Id", "Title");
            ViewBag.ReporterId = GetLoggedUser().Id;

            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssueCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var initialState = GetInitialState();
                if (initialState == null)
                {
                    TempData["ErrorSQL"] = "There is no initial state. The issue couldn't be created.";

                    return RedirectToAction("Create");
                }

                var issue = Mapper.Map<Issue>(viewModel);
                issue.StateId = initialState.Id;
                issue.ReporterId = GetLoggedUser().Id;
                issue.Created = DateTime.UtcNow;
                issue.Id = Guid.NewGuid();
                issue.CodeNumber = db.Issues.Max(x => (int?)x.CodeNumber) + 1 ?? 1;

                db.Issues.Add(issue);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", viewModel.AssigneeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", viewModel.ProjectId);
            //ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);
            return View(viewModel);
        }

        private State GetInitialState()
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

        private ApplicationUser GetLoggedUser()
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

            var issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }

            var viewModel = Mapper.Map<IssueEditViewModel>(issue);
            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", issue.AssigneeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", issue.ProjectId);
            ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);

            return View(viewModel);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,AssigneeId,ProjectId")] IssueEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // deactivate original entity
                var entityToDeactivate = db.Issues.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.Id);
                if (entityToDeactivate != null)
                {
                    entityToDeactivate.DeletedAt = DateTime.Now;
                    db.Entry(entityToDeactivate).State = EntityState.Modified;
                    db.SaveChanges();

                    // create a new entity
                    var entityNew = db.Issues.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.Id);
                    // map viewModel to the entity
                    Mapper.CreateMap<IssueViewModel, Issue>();
                    entityNew = Mapper.Map(viewModel, entityNew);
                    // create a new Id and set the issue to active
                    entityNew.Id = Guid.NewGuid();
                    entityNew.DeletedAt = null;
                    // save the entity
                    db.Issues.Add(entityNew);
                    db.SaveChanges();
                
                    return RedirectToAction("Details", new { id = entityToDeactivate.Code });
                }

                return RedirectToAction("Index");
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", viewModel.AssigneeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", viewModel.ProjectId);
            return View(viewModel);
        }

        // GET: Issues/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<IssueViewModel>(issue));
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var issue = db.Issues.Find(id);
            issue.DeletedAt = DateTime.Now;
            db.Entry(issue).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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
            // deactivate original entity
            var entityToDeactivate = db.Issues.AsNoTracking().FirstOrDefault(x => x.Id == issueId);
            if (entityToDeactivate != null)
            {
                entityToDeactivate.DeletedAt = DateTime.Now;
                db.Entry(entityToDeactivate).State = EntityState.Modified;
                db.SaveChanges();

                // create a new entity
                var entityNew = db.Issues.AsNoTracking().FirstOrDefault(x => x.Id == issueId);
                // change status
                if (entityNew != null)
                {
                    entityNew.StateId = to;
                    // create a new Id and set the issue to active
                    entityNew.Id = Guid.NewGuid();
                    entityNew.DeletedAt = null;
                    // save the entity
                    db.Issues.Add(entityNew);
                }
                db.SaveChanges();

                if (HttpContext.Request.UrlReferrer != null)
                {       
                    return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
