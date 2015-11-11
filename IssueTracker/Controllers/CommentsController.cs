using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using IssueTracker.DAL;
using IssueTracker.Entities;
using IssueTracker.ViewModels;
using AutoMapper;

namespace IssueTracker.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /*
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
        */
        // GET: Comments/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.IssueId = id;
            return View();
        }
        
        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,Posted,IssueId")] CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                comment.Issue = db.Issues.Where(x => x.Id == comment.IssueId).OrderByDescending(x => x.Created).First();

                comment.Id = Guid.NewGuid();
                comment.Posted = DateTime.Now;
                comment.AuthorId = getLoggedUser().Id;
                comment.IssueCreatedAt = comment.Issue.CreatedAt;

                db.Comments.Add(Mapper.Map<Comment>(comment));
                db.SaveChanges();
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }

            if (comment.Text.IsEmpty())
            {
                comment.Issue = db.Issues.Where(x => x.Id == comment.IssueId).OrderByDescending(x => x.Created).First();

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

            var comment = db.Comments.Where(x => x.Id == id).OrderByDescending(x => x.CreatedAt).First();

            if (comment == null)
            {
                return HttpNotFound();
            }

            ViewBag.IssueId = new SelectList(db.Issues, "Id", "Name", comment.IssueId);

            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,Posted,IssueId")] CommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Issue = db.Issues.Where(x => x.Id == viewModel.IssueId).OrderByDescending(x => x.Created).First();

                // create a new entity
                var entityNew = db.Comments.AsNoTracking().Where(x => x.Id == viewModel.Id).OrderByDescending(x => x.CreatedAt).First();
                if (entityNew != null)
                {
                    // edit comment text
                    entityNew.Text = viewModel.Text;
                    // change CreatedAt
                    entityNew.CreatedAt = DateTime.Now;
                    // save the entity
                    db.Comments.Add(entityNew);
                    db.SaveChanges();
                }

                return RedirectToAction("Details", "Issues", new {id = viewModel.Issue.Code});
                
            }
            // todo: otestovat
            //viewModel.Issue = db.Issues.Find(viewModel.IssueId);
            //ViewBag.IssueId = new SelectList(db.Issues, "Id", "Name", viewModel.IssueId);

            return RedirectToAction("Details", "Issues", new {id = viewModel.Issue.Code});
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = db.Comments.Where(x => x.Id == id).OrderByDescending(x => x.CreatedAt).Include(x => x.Issue).First();

            if (comment == null)
            {
                return HttpNotFound();
            }

            ViewBag.IssueCode = comment.Issue.Code;

            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var commentIssueIdTemp = new Guid();

            var comments = db.Comments.Where(x => x.Id == id);
            foreach (var comment in comments)
            {
                commentIssueIdTemp = comment.IssueId;

                comment.Active = false;
                db.Entry(comment).State = EntityState.Modified;
            }

            db.SaveChanges();

            var commentIssueCodeTemp = db.Issues.Where(x => x.Id == commentIssueIdTemp).OrderByDescending(x => x.CreatedAt).First();

            return RedirectToAction("Details", "Issues", new { id = commentIssueCodeTemp.Code });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private ApplicationUser getLoggedUser()
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
