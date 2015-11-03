using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using IssueTracker.DAL;
using IssueTracker.Entities;
using IssueTracker.ViewModels;
using System.Collections.Generic;
using AutoMapper;

namespace IssueTracker.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Where(c => c.DeletedAt == null).Include(c => c.Issue);
            return View(Mapper.Map<IEnumerable<CommentViewModel>>(comments).ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            comment.User = db.Users.Find(comment.AuthorId);

            return View(Mapper.Map<CommentViewModel>(comment));
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
        public ActionResult Create([Bind(Include = "Id,Text,Posted,IssueId")] CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                comment.Issue = db.Issues.Find(comment.IssueId);

                comment.Id = Guid.NewGuid();
                comment.Posted = DateTime.Now;
                comment.AuthorId = GetLoggedUser().Id;
                comment.CodeNumber = db.Comments.Max(x => (int?)x.CodeNumber) + 1 ?? 1;

                db.Comments.Add(Mapper.Map<Comment>(comment));
                db.SaveChanges();
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }

            if (comment.Text.IsEmpty())
            {
                comment.Issue = db.Issues.Find(comment.IssueId);

                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
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
            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,Posted,IssueId")] CommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Issue = db.Issues.Find(viewModel.IssueId);

                // deactivate original entity
                var entityToDeactivate = db.Comments.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.Id);
                if (entityToDeactivate != null)
                {
                    entityToDeactivate.DeletedAt = DateTime.Now;
                    db.Entry(entityToDeactivate).State = EntityState.Modified;
                    db.SaveChanges();

                    // create a new entity
                    var entityNew = db.Comments.AsNoTracking().FirstOrDefault(x => x.Id == viewModel.Id);
                    if (entityNew != null)
                    {
                        // edit comment text
                        entityNew.Text = viewModel.Text;
                        // create a new Id and set the issue to active
                        entityNew.Id = Guid.NewGuid();
                        entityNew.DeletedAt = null;
                        // save the entity
                        db.Comments.Add(entityNew);
                    }
                    db.SaveChanges();

                    return RedirectToAction("Details", "Issues", new {id = viewModel.Issue.Code});
                }
            }

            viewModel.Issue = db.Issues.Find(viewModel.IssueId);
            ViewBag.IssueId = new SelectList(db.Issues, "Id", "Name", viewModel.IssueId);

            return RedirectToAction("Details", "Issues", new {id = viewModel.Issue.Code});
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var comment = db.Comments.Find(id);
            comment.DeletedAt = DateTime.Now;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code});
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
                var user = db.Users.First(dbUser => dbUser.Email == User.Identity.Name);
                return user;
            }
            return null;
        }

    }
}
