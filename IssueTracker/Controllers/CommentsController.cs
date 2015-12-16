using System;
using System.Web.Mvc;
using IssueTracker.ViewModels;
using AutoMapper;
using IssueTracker.Models;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Entities;

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

        // GET: Comments/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.IssueId = id;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Text,IssueId")] CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                comment.Issue = _issueRepo.Get(comment.IssueId);
                comment.Id = Guid.NewGuid();
                comment.AuthorId = getLoggedUser().Id;
                comment.Posted = DateTime.Now;
                comment.IssueCreatedAt = comment.Issue.CreatedAt;
                comment.CreatedAt = DateTime.Now;

                _commentRepo.Add(Mapper.Map<Comment>(comment));
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }

            comment.Issue = _issueRepo.Get(comment.IssueId);
            return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(Guid id)
        {
            var comment = _commentRepo.Get(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (getLoggedUser().Id != comment.AuthorId)
            {
                TempData["ErrorMessage"] = "Only owners can edit their comments.";
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }
            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,Posted,IssueId")] CommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var issue = _issueRepo.Get(viewModel.IssueId);
                return RedirectToAction("Details", "Issues", new { id = issue.Code });
            }
            var oldEntity = _commentRepo.Get(viewModel.Id);
            if (oldEntity == null)
            {
                return HttpNotFound();
            }
            viewModel.Issue = _issueRepo.Get(viewModel.IssueId);
            viewModel.Posted = oldEntity.Posted;
            viewModel.AuthorId = oldEntity.Author.Id;
            viewModel.IssueCreatedAt = viewModel.Issue.CreatedAt;
            viewModel.CreatedAt = DateTime.Now;

            _commentRepo.Add(Mapper.Map<Comment>(viewModel));

            return RedirectToAction("Details", "Issues", new { id = viewModel.Issue.Code });
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(Guid id)
        {
            var comment = _commentRepo.Get(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole(UserRoles.Administrators))
            {
                TempData["ErrorMessage"] = "Only administrators can delete comments.";
                return RedirectToAction("Details", "Issues", new { id = comment.Issue.Code });
            }
            ViewBag.IssueCode = comment.Issue.Code;
            return View(Mapper.Map<CommentViewModel>(comment));
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var issue = _commentRepo.Get(id).Issue;
            _commentRepo.Remove(id);

            return RedirectToAction("Details", "Issues", new { id = issue.Code });
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
