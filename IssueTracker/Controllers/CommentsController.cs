using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using IssueTracker.Data.Entities;
using IssueTracker.ViewModels;
using AutoMapper;
using IssueTracker.Data;
using IssueTracker.Models;
using IssueTracker.Data.Contracts.Repository_Interfaces;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class CommentsController : Controller
    {
        private ICommentRepository _commentRepo;
        private IIssueRepository _issueRepo;
        private IApplicationUserRepository _userRepo;

        public CommentsController(ICommentRepository commentRepository, IIssueRepository issueRepository, IApplicationUserRepository applicationUserRepository)
        {
            _commentRepo = commentRepository;
            _issueRepo = issueRepository;
            _userRepo = applicationUserRepository;
        }
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
                comment.Issue = _issueRepo.FindBy(x => x.Id == comment.IssueId).OrderByDescending(x => x.Created).First();

                comment.Id = Guid.NewGuid();
                comment.Posted = DateTime.Now;
                comment.AuthorId = getLoggedUser().Id.ToString();
                comment.IssueCreatedAt = comment.Issue.CreatedAt;

                _commentRepo.Add(Mapper.Map<Comment>(comment));
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }

            if (comment.Text.IsEmpty())
            {
                comment.Issue = _issueRepo.FindBy(x => x.Id == comment.IssueId).OrderByDescending(x => x.Created).First();

                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }

            ViewBag.IssueId = new SelectList(_issueRepo.GetAll(), "Id", "Name", comment.IssueId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = _commentRepo.FindBy(x => x.Id == id).OrderByDescending(x => x.CreatedAt).First();

            if (comment == null)
            {
                return HttpNotFound();
            }

            if (getLoggedUser().Id != comment.AuthorId)
            {
                TempData["ErrorMessage"] = "Only owners can edit their comments.";
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }

            ViewBag.IssueId = new SelectList(_issueRepo.GetAll(), "Id", "Name", comment.IssueId);

            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,Posted,IssueId")] CommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Issue = _issueRepo.FindBy(x => x.Id == viewModel.IssueId).OrderByDescending(x => x.Created).First();

                // create a new entity
                var entityNew = _commentRepo.FindBy(x => x.Id == viewModel.Id).OrderByDescending(x => x.CreatedAt).First();
                if (entityNew != null)
                {
                    // edit comment text
                    entityNew.Text = viewModel.Text;
                    // change CreatedAt
                    entityNew.CreatedAt = DateTime.Now;
                    // save the entity
                    _commentRepo.Add(entityNew);
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

            var comment = _commentRepo.FindBy(x => x.Id == id).OrderByDescending(x => x.CreatedAt).Include(x => x.Issue).First();

            if (comment == null)
            {
                return HttpNotFound();
            }

            if (!User.IsInRole(UserRoles.Administrators.ToString()))
            {
                TempData["ErrorMessage"] = "Only administrators can delete comments.";
                return RedirectToAction("Details","Issues", new { id = comment.Issue.Code });
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

            var comments = _commentRepo.FindBy(x => x.Id == id);
            foreach (var comment in comments)
            {
                commentIssueIdTemp = comment.IssueId;

                comment.Active = false;
            }

            _commentRepo.Save();

            var commentIssueCodeTemp = _issueRepo.FindBy(x => x.Id == commentIssueIdTemp).OrderByDescending(x => x.CreatedAt).First();

            return RedirectToAction("Details", "Issues", new { id = commentIssueCodeTemp.Code });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private ApplicationUser getLoggedUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userRepo.FindSingleBy(dbUser => dbUser.Email == User.Identity.Name);
                return user;
            }
            return null;
        }

    }
}
