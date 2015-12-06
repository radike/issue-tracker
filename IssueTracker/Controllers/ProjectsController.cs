using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using IssueTracker.Data.Entities;
using IssueTracker.ViewModels;
using AutoMapper;
using IssueTracker.Abstractions;
using Microsoft.AspNet.Identity;
using IssueTracker.Data;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Data_Repositories;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class ProjectsController : Controller
    {
        private static IssueTrackerContext db = new IssueTrackerContext();
        private IProjectRepository projectRepo = new ProjectRepository(db);
        private IIssueRepository issueRepo = new IssueRepository(db);

        private const int ProjectsPerPage = 20;
        private const int IssuesPerProjectPage = 10;

        // GET: Projects
        public ActionResult Index(int? page)
        {
            var projectsTemp = projectRepo.GetAll().AsQueryable()
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .ToList();

            var projects = Mapper.Map<IEnumerable<ProjectViewModel>>(projectsTemp);
            var pageNumber = page ?? 1;

            ViewBag.ErrorMessageNotOwner = TempData["ErrorMessageNotOwner"] as string;
            ViewBag.LoggedUserId = User.Identity.GetUserId();
            ViewBag.IsUserAdmin = User.IsInRole(UserRoles.Administrators.ToString());

            return View(projects.ToPagedList(pageNumber, ProjectsPerPage));
        }

        // GET: Projects/Details/XYZ
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = projectRepo.GetAll().AsQueryable().Where(p => p.Code == id && p.Active).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            
            if (project == null)
            {
                return HttpNotFound();
            }

            project.Issues = issueRepo.GetAll()
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .Where(n => n.ProjectId == project.Id)
                .ToList();

            var pageNumber = page ?? 1;

            var viewModel = Mapper.Map<ProjectViewModel>(project);
            viewModel.IssuesPage = viewModel.Issues.ToPagedList(pageNumber, IssuesPerProjectPage);
            ViewBag.CanEdit = UserIsProjectOwnerOrHasAdminRights(viewModel);

            return View(viewModel);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.UsersList = new MultiSelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Code,SelectedUsers,OwnerId")] ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                // if the code already exists
                if (Enumerable.Any(projectRepo.GetAll(), p => p.Code.Equals(project.Code.ToUpper())))
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

                addOwnerToUsers(project);
                
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

            var project = projectRepo.GetAll().AsQueryable().Where(p => p.Id == id && p.Active).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            if (project == null)
            {
                return HttpNotFound();
            }

            var viewModel = Mapper.Map<ProjectViewModel>(project);

            if (!UserIsProjectOwnerOrHasAdminRights(viewModel))
            {
                TempData["ErrorMessageNotOwner"] = "Only project owners and administrators can edit projects.";
                return RedirectToAction("Index");
            }

            viewModel.SelectedUsers = viewModel.Users.Select(u => u.Id.ToString()).ToList();
            ViewBag.UsersList = db.Users;

            return View(viewModel);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Code,SelectedUsers,OwnerId")] ProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                addOwnerToUsers(viewModel);

                // create a new entity
                var entityNew = projectRepo.GetAll().AsQueryable().AsNoTracking().Where(x => x.Id == viewModel.Id).OrderByDescending(x => x.CreatedAt).First();
                // map viewModel to the entity
                entityNew = Mapper.Map(viewModel, entityNew);
                // change CreatedAt
                entityNew.CreatedAt = DateTime.Now;
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

            var project = projectRepo.GetAll().AsQueryable().Where(p => p.Id == id && p.Active).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            if (project == null)
            {
                return HttpNotFound();
            }

            var viewModel = Mapper.Map<ProjectViewModel>(project);

            if (!User.IsInRole(UserRoles.Administrators.ToString()))
            {
                TempData["ErrorMessageNotOwner"] = "Only project administrators can delete projects.";
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var projects = db.Projects.Where(x => x.Id == id);

            foreach (var project in projects)
            {
                project.Active = false;
                db.Entry(project).State = EntityState.Modified;

            }

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

        public bool UserIsProjectOwnerOrHasAdminRights(ProjectViewModel project)
        {
            return User.IsInRole(UserRoles.Administrators.ToString())
                || (project.OwnerId != null && project.OwnerId.Equals(User.Identity.GetUserId()));
        }

        private static void addOwnerToUsers(ProjectViewModel project)
        {
            project.SelectedUsers = project.SelectedUsers?.Union(new[] { project.OwnerId }) ?? new[] { project.OwnerId };
        }
    }
}
