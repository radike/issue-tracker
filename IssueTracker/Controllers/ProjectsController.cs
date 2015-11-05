using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IssueTracker.DAL;
using PagedList;
using IssueTracker.Entities;
using IssueTracker.ViewModels;
using AutoMapper;
using IssueTracker.Abstractions;

namespace IssueTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private const int ProjectsPerPage = 20;

        // GET: Projects
        public ActionResult Index(int? page)
        {
            var projectsTemp = db.Projects.Where(x => x.DeletedAt == null).OrderBy(x => x.Title);
            var projects = Mapper.Map<IEnumerable<ProjectViewModel>>(projectsTemp);
            var pageNumber = page ?? 1;

            return View(projects.ToPagedList(pageNumber, ProjectsPerPage));
        }

        // GET: Projects/Details/XYZ
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = db.Projects.Where(x => x.Code == id && x.DeletedAt == null).Include(p => p.Issues.Select(i => i.State)).First();
            if (project == null)
            {
                return HttpNotFound();
            }
            project.Issues = project.Issues.Where(x => x.DeletedAt == null).ToList();

            return View(Mapper.Map<ProjectViewModel>(project));
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
        public ActionResult Create([Bind(Include = "Id,Title,Code,SelectedUsers,OwnerId")] ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                // if the code already exists
                if (Enumerable.Any(db.Projects, p => p.Code.Equals(project.Code.ToUpper())))
                {
                    ViewBag.ErrorUniqueCode = "Entered code is already associated with another project.";
                    ViewBag.UsersList = new MultiSelectList(db.Users, "Id", "Email");
                    return View(project);
                }
                
                // if the code has invalid format
                if (!Helper.CheckProjectCodePattern(project.Code))
                {
                    ViewBag.ErrorInvalidFormatCode = "Entered code has invalid format. Only characters are allowed.";
                    ViewBag.UsersList = new MultiSelectList(db.Users, "Id", "Email");
                    return View(project);
                }

                project.Id = Guid.NewGuid();
                project.Code = project.Code.ToUpper();

                // add selected owner among users, if not already there
                if (project.SelectedUsers == null)
                {
                    project.SelectedUsers = new [] { project.OwnerId };
                }
                else if (project.SelectedUsers != null && !project.SelectedUsers.Contains(project.OwnerId))
                {
                    project.SelectedUsers = project.SelectedUsers.Concat(new[] { project.OwnerId });
                }
                
                project.Users = db.Users.Where(u => project.SelectedUsers.Contains(u.Id.ToString())).ToList();

                db.Projects.Add(Mapper.Map<Project>(project));
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

            var project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            project.SelectedUsers = project.Users.Select(u => u.Id.ToString()).ToList();

            ViewBag.UsersList = db.Users;

            Mapper.CreateMap<Project, ProjectViewModel>();
            return View(Mapper.Map<ProjectViewModel>(project));
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,SelectedUsers,OwnerId")] ProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // deactivate original entity 
                var entityToDeactivate = db.Projects.AsNoTracking().First(x => x.Code == viewModel.Code && x.DeletedAt == null);
                entityToDeactivate.DeletedAt = DateTime.Now;
                var oldId = entityToDeactivate.Id;
                db.Entry(entityToDeactivate).State = EntityState.Modified;
                db.SaveChanges();

                // add selected owner among users, if not already there
                if (viewModel.SelectedUsers == null)
                {
                    viewModel.SelectedUsers = new[] { viewModel.OwnerId };
                }
                else if (viewModel.SelectedUsers != null && !viewModel.SelectedUsers.Contains(viewModel.OwnerId))
                {
                    viewModel.SelectedUsers = viewModel.SelectedUsers.Concat(new[] { viewModel.OwnerId });
                }

                // create a new entity
                var entityNew = db.Projects.AsNoTracking().FirstOrDefault(x => x.Id == oldId);
                // map viewModel to the entity
                Mapper.CreateMap<ProjectViewModel, Project>();
                entityNew = Mapper.Map(viewModel, entityNew);
                // create a new Id
                entityNew.Id = Guid.NewGuid();
                // attach the entity in order to load the selected users
                db.Projects.Attach(entityNew);
                db.Entry(entityNew).Collection(p => p.Users).Load();
                entityNew.Users = db.Users.Where(u => viewModel.SelectedUsers.Contains(u.Id.ToString())).ToList();
                // save the entity
                db.Projects.Add(entityNew);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
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
            return View(Mapper.Map<ProjectViewModel>(project));
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var project = db.Projects.Find(id);
            project.DeletedAt = DateTime.Now;
            db.Entry(project).State = EntityState.Modified;
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
