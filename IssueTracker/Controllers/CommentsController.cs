using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using IssueTracker.DAL;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Issue);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            comment.User = db.Users.Find(comment.AuthorId);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.IssueId = id;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,Posted,IssueId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
               // comment.IssueId = new Guid("06138F5E-21AA-486F-927A-F233113FF4C5");
                comment.Id = Guid.NewGuid();
                comment.Posted = DateTime.Now;
                comment.AuthorId = GetLoggedUser().Id;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            }

            if (comment.Text.IsEmpty())
            {
                return RedirectToAction("Details", "Issues", new {id = comment.IssueId});
            }

            ViewBag.IssueId = new SelectList(db.Issues, "Id", "Name", comment.IssueId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.IssueId = new SelectList(db.Issues, "Id", "Name", comment.IssueId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,Posted,IssueId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorId = GetLoggedUser().Id;
                comment.Posted = DateTime.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
                //return RedirectToAction("Index");
            }
            ViewBag.IssueId = new SelectList(db.Issues, "Id", "Name", comment.IssueId);
            return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            //return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Issues", new { id = comment.IssueId });
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

    }
}
