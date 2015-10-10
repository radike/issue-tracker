using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IssueTracker.DAL;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class IssuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Issues
        public ActionResult Index()
        {
            var issues = db.Issues.Include(i => i.ApplicationUser).Include(i => i.Project).Include(i => i.State);
            return View(issues.ToList());
        }

        // GET: Issues/Details/5
        public ActionResult Details(Guid? id)
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

        // GET: Issues/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            ViewBag.StateId = new SelectList(db.States, "Id", "Title");
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StateId,Description,ApplicationUserId,ProjectId")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                issue.Id = Guid.NewGuid();
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", issue.ApplicationUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", issue.ProjectId);
            ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);
            return View(issue);
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
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", issue.ApplicationUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", issue.ProjectId);
            ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StateId,Description,ApplicationUserId,ProjectId")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", issue.ApplicationUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", issue.ProjectId);
            ViewBag.StateId = new SelectList(db.States, "Id", "Title", issue.StateId);
            return View(issue);
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
    }
}
