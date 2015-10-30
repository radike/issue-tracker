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

namespace IssueTracker.Controllers
{
    public class IssuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Issues
        public ActionResult Index(int? page)
        {
            ViewBag.ErrorSQL = TempData["ErrorSQL"] as string;

            var issues = Mapper.Map<IEnumerable<IssueViewModel>>(db.Issues.OrderByDescending(i => i.Created));
            int pageNumber = page ?? 1;
            const int pageSize = 20;
            return View(issues.ToPagedList(pageNumber, pageSize));
        }

        // GET: Issues/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var viewModel = new IssueDetailViewModel
            {
                Issue = Mapper.Map<IssueViewModel>(db.Issues.Where(i => i.Id == id).Include(i => i.State).First())
            };

            if (viewModel.Issue == null)
            {
                return HttpNotFound();
            }

            var workflows = db.StateWorkflows.Where(c => c.FromState.Id == viewModel.Issue.State.Id);
            viewModel.StateWorkflows = Mapper.Map<IEnumerable<StateWorkflowViewModel>>(workflows);
            var comments = db.Comments.Where(c => c.IssueId == id).OrderBy(o => o.Posted).ToList();
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
                    return RedirectToAction("Index");
                }
                Issue issue = Mapper.Map<Issue>(viewModel);
                issue.StateId = initialState.Id;
                issue.ReporterId = GetLoggedUser().Id;
                issue.Created = DateTime.UtcNow;
                issue.Id = Guid.NewGuid();
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
            return db.States.Where(s => s.IsInitial).First();
        }

        private ApplicationUser GetLoggedUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = db.Users.Where(dbUser => dbUser.Email == User.Identity.Name).First();
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

            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }

            IssueEditViewModel viewModel = Mapper.Map<IssueEditViewModel>(issue);
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
                // first, get from the database original issue
                var updatedIssue = db.Issues.AsNoTracking().First(x => x.Id == viewModel.Id);

                // and then rewrite updated fields
                // perform mapping without overriding whole object
                updatedIssue = Mapper.Map(viewModel, updatedIssue);
                db.Entry(updatedIssue).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = viewModel.Id });
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
            Issue issue = db.Issues.Find(id);
            db.Issues.Remove(issue);
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
            var issue = db.Issues.Find(issueId);
            issue.StateId = to;

            db.SaveChanges();

            if (HttpContext.Request.UrlReferrer != null)
            {
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
