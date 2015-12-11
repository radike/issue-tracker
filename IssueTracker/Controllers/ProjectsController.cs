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
using System.Text.RegularExpressions;
using IssueTracker.Locale;
using IssueTracker.Data.Services;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class ProjectsController : Controller
    {
        private const int ProjectsPerPage = 20;
        private const int IssuesPerProjectPage = 10;

        private IProjectService _service;
        private IApplicationUserRepository _userRepo;

        public ProjectsController(IProjectService service, IApplicationUserRepository userRepository)
        {
            _service = service;
            _userRepo = userRepository;
        }

        // GET: Projects
        public ActionResult Index(int? page)
        {
            return List(null, page);
        }

        public ActionResult List(string id, int? page)
        {
            ViewBag.ErrorMessageNotOwner = TempData["ErrorMessageNotOwner"] as string;
            ViewBag.LoggedUserId = User.Identity.GetUserId();
            ViewBag.IsUserAdmin = User.IsInRole(UserRoles.Administrators.ToString());

            Guid userId = new Guid(ViewBag.LoggedUserId);
            IEnumerable<Project> projects;
            switch (id)
            {
                case "All":
                    projects = _service.GetProjects();
                    break;
                default:
                    projects = _service.GetProjectsForUser(userId);
                    break;
            }

            var viewModel = Mapper.Map<IEnumerable<ProjectViewModel>>(projects);
            var pageNumber = page ?? 1;

            return View("Index", viewModel.ToPagedList(pageNumber, ProjectsPerPage));
        }

        // GET: Projects/Details/XYZ
        public ActionResult Details(String id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = _service.GetProject(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var pageNumber = page ?? 1;

            var viewModel = Mapper.Map<ProjectViewModel>(project);
            viewModel.IssuesPage = viewModel.Issues.ToPagedList(pageNumber, IssuesPerProjectPage);

            ViewBag.CanEdit = UserIsProjectOwnerOrHasAdminRights(project);

            return View(viewModel);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.UsersList = new MultiSelectList(_userRepo.GetAll(), "Id", "Email");
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Code,SelectedUsers,OwnerId")] ProjectViewModel project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }
            if (ProjectCodeHasInvalidFormat(project.Code))
            {
                ViewBag.ErrorInvalidFormatCode = ProjectStrings.ErrorMessageInvalidCode;
                ViewBag.ErrorInvalidFormatCode = "Entered code has invalid format. Only characters are allowed.";
                ViewBag.UsersList = new MultiSelectList(_userRepo.GetAll(), "Id", "Email");
                return View(project);
            }
            if (_service.ProjectCodeIsNotUnique(project.Code))
            {
                ViewBag.ErrorUniqueCode = ProjectStrings.ErrorMessageNotUniqueCode;
                ViewBag.UsersList = new MultiSelectList(_userRepo.GetAll(), "Id", "Email");
                return View(project);
            }

            _service.CreateProject(Mapper.Map<Project>(project));
            return RedirectToAction("Index");
        }

        private static bool ProjectCodeHasInvalidFormat(string s)
        {
            var rgx = new Regex(@"^[a-zA-Z]+$");// e.g.: CODE-19
            return !rgx.IsMatch(s);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = _service.GetProject(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            if (!UserIsProjectOwnerOrHasAdminRights(project))
            {
                TempData["ErrorMessageNotOwner"] = ProjectStrings.ErrorMessageEditNonadmin;
                return RedirectToAction("Index");
            }

            var viewModel = Mapper.Map<ProjectViewModel>(project);
            viewModel.Id = project.Id;
            viewModel.OwnerId = project.Owner.Id;
            viewModel.SelectedUsers = project.Users.Select(u => u.Id).ToList();

            ViewBag.UsersList = _userRepo.GetAll();

            return View(viewModel);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,Code,SelectedUsers,OwnerId")] ProjectViewModel viewModel)
        {
            Guid? id = _service.GetProjectId(viewModel.Code);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            viewModel.Id = id.Value;
            if (!ModelState.IsValid)
            {
                var project = _service.GetProject(viewModel.Code);
                IEnumerable<ApplicationUser> userList = _userRepo.GetAll();
                viewModel.OwnerId = project.Owner.Id;
                viewModel.SelectedUsers = project.Users.Select(u => u.Id).ToList();
                ViewBag.UsersList = userList;
                return View(viewModel);
            }
            Project entity = Mapper.Map<Project>(viewModel);
            _service.EditProject(entity);

            return RedirectToAction("Index");
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.IsInRole(UserRoles.Administrators.ToString()))
            {
                TempData["ErrorMessageNotOwner"] = ProjectStrings.ErrorMessageDeleteNonadmin;
                return RedirectToAction("Index");
            }

            var project = _service.GetProject(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var viewModel = Mapper.Map<ProjectViewModel>(project);
            return View(viewModel);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            _service.DeleteProject(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool UserIsProjectOwnerOrHasAdminRights(Project project)
        {
            return User.IsInRole(UserRoles.Administrators.ToString())
                || (project.OwnerId == Guid.Parse(User.Identity.GetUserId()));
        }
    }
}
