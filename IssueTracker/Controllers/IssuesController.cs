using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using IssueTracker.DAL;
using IssueTracker.Models;
using IssueTracker.ViewModels;

namespace IssueTracker.Controllers
{
    public class IssuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Issues
        public ActionResult Index()
        {
            var issues = db.Issues.Include(i => i.Reporter).Include(i => i.Assignee).Include(i => i.Project).Include(i => i.State);
            return View(issues.ToList());
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
                Issue = db.Issues.Where(i => i.Id == id).Include(i => i.State).First()
            };

            if (viewModel.Issue == null)
            {
                return HttpNotFound();
            }

            viewModel.StateWorkflows = db.StateWorkflows.ToList().Where(c => c.FromState == viewModel.Issue.State);

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
        public ActionResult Create([Bind(Include = "Id,Name,Description,ReporterId,AssigneeId,ProjectId")] Issue issue)
        {
            ModelState.Clear();
            issue.ReporterId = GetLoggedUser().Id;
            issue.Created = DateTime.UtcNow;
            if (TryValidateModel(issue))
            {
                issue.Id = Guid.NewGuid();
                foreach (var s in db.States.Where(s => s.IsInitial))
                {
                    issue.State = s;
                    issue.StateId = s.Id;
                }
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssigneeId = new SelectList(db.Users, "Id", "Email", issue.AssigneeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", issue.ProjectId);
            //ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);
            return View(issue);
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

            // configure AutoMapper
            Mapper.CreateMap<Issue, IssueEditViewModel>();
            // perform mapping
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
                var issueUpdated = db.Issues.AsNoTracking().First(x => x.Id == viewModel.Id);

                // and then rewrite updated fields
                // configure AutoMapper
                Mapper.CreateMap<IssueEditViewModel, Issue>();
                // perform mapping without overriding whole object
                issueUpdated = Mapper.Map(viewModel, issueUpdated);

                db.Entry(issueUpdated).State = EntityState.Modified;
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
            return View(issue);
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
