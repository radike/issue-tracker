using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using IssueTracker.Data;
using IssueTracker.Entities;
using IssueTracker.ViewModels;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class UsersController : Controller
    {
        private readonly IssueTrackerContext _db = new IssueTrackerContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(_db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = _db.Users.Find(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // GET: Users/Create
        [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(applicationUser);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = _db.Users.Find(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
        public ActionResult Edit([Bind(Include = "Id,Email,PhoneNumber,UserName")] UserEditViewModel viewModel)
        {
            var entityNew = _db.Users.Find(viewModel.Id);

            if (ModelState.IsValid)
            {
                entityNew = Mapper.Map(viewModel, entityNew);

                _db.Entry(entityNew).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entityNew);
        }

        // GET: Users/Delete/5
        [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = _db.Users.Find(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            var applicationUser = _db.Users.Find(id);
            _db.Users.Remove(applicationUser);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}