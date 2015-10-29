using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web.Mvc;
using IssueTracker.DAL;
using IssueTracker.Models;
using PagedList;

namespace IssueTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private const int ProjectsPerPage = 20;
        private const int IssuesPerProjectPage = 10;

        // GET: Projects
        public ActionResult Index(int? page)
        {
            ViewBag.ErrorSQL = TempData["ErrorSQL"] as string;

            var projects = db.Projects.ToList();
            int pageNumber = page ?? 1;

            return View(projects.ToPagedList(pageNumber, ProjectsPerPage));
        }

        // GET: Projects/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Where(p => p.Id == id).Include(p => p.Issues.Select(i => i.State)).First();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.UsersList = new MultiSelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,SelectedUsers")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid();

                if (project.SelectedUsers != null)
                {
                    project.Users = db.Users.Where(u => project.SelectedUsers.Contains(u.Id.ToString())).ToList();
                }

                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var userIds = project.Users.Select(u => u.Id.ToString()).ToList();
            var usersSelectList = db.Users.Select(u => new SelectListItem
            {
                Text = u.Email,
                Value = u.Id,
                Selected = userIds.Contains(u.Id)
            });
            ViewBag.UsersList = usersSelectList;


            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,SelectedUsers")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Attach(project);
                db.Entry(project).Collection(p => p.Users).Load(); // Users need to be loaded in order to change them

                if (project.SelectedUsers != null)
                {
                    project.Users = db.Users.Where(u => project.SelectedUsers.Contains(u.Id.ToString())).ToList();
                }
                else
                {
                    project.Users = null;
                }

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
