using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using AutoMapper;
using IssueTracker.Data.Entities;
using IssueTracker.ViewModels;
using PagedList;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using System.Text.RegularExpressions;
using IssueTracker.Entities;
using IssueTracker.Models;
using IssueTracker.Data.Services;
using IssueTracker.Services;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class IssuesController : Controller
    {
        private IIssueService _issueService;
        private IStateService _stateService;
        private IStateWorkflowRepository stateWorkflowRepo;
        private IApplicationUserRepository applicationUserRepo;
        private IStateRepository stateRepo;
        private IProjectService _projectService;

        private const int ProjectsPerPage = 20;

        public IssuesController(IStateWorkflowRepository stateWorkflowRepository, IApplicationUserRepository applicationUserRepository, 
            IStateRepository stateRepository, IIssueService issueService, IStateService stateService, IProjectService projectService)
        {            
            _projectService = projectService;
            _issueService = issueService;
            _stateService = stateService;
            stateWorkflowRepo = stateWorkflowRepository;
            applicationUserRepo = applicationUserRepository;
            stateRepo = stateRepository;
        }

        // GET: Issues
        public ActionResult Index(int? page, string sort, string searchName, string searchTitle, 
            Guid? searchAssignee, Guid? searchReporter, Guid? searchProject, Guid? searchState)
        {
            // viewbag items are used in the header to sort the records
            ViewBag.CreatedSort = String.IsNullOrEmpty(sort) ? "created_desc" : String.Empty;
            ViewBag.SummarySort = sort == "summary" ? "summary_desc" : "summary";
            ViewBag.ReporterSort = sort == "reporter" ? "reporter_desc" : "reporter";
            ViewBag.ProjectSort = sort == "project" ? "project_desc" : "project";
            ViewBag.AssigneeSort = sort == "assignee" ? "assignee_desc" : "assignee";
            ViewBag.StatusSort = sort == "status" ? "status_desc" : "status";

            ViewBag.SearchProject = new SelectList(_projectService.GetProjects(), "Id", "Title");
            ViewBag.SearchAssignee = ViewBag.SearchReporter = new SelectList(applicationUserRepo.GetAll(), "Id", "Email");
            ViewBag.SearchState = new SelectList(_stateService.GetStatesOrderedByIndex(), "Id", "Title");

            var issuesTemp = _issueService.GetAllIssues().ToList();

            // search
            issuesTemp = searchIssues(issuesTemp, searchName, searchTitle, searchAssignee, searchReporter, searchProject, searchState);
            
            var issues = Mapper.Map<IEnumerable<IssueIndexViewModel>>(issuesTemp);
            issues = GetSortedIssues(issues, sort);

            var pageNumber = page ?? 1;
            return View(issues.ToPagedList(pageNumber, ProjectsPerPage));
        }

        private static List<Issue> searchIssues (List<Issue> issues, string searchName, string searchTitle, 
            Guid? searchAssignee, Guid? searchReporter, Guid? searchProject, Guid? searchState)
        {
            if (!String.IsNullOrEmpty(searchName))
            {
                issues = issues.Where(s => s.Name.ToLower().Contains(searchName.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(searchTitle))
            {
                issues = issues.Where(s => (s.Project.Code + s.CodeNumber + ": " + s.Name).ToLower().Contains(searchTitle.ToLower())).ToList();
            }

            if (searchAssignee != null)
            {
                issues = issues.Where(s => s.AssigneeId == searchAssignee).ToList();
            }

            if (searchReporter != null)
            {
                issues = issues.Where(s => s.ReporterId == searchReporter).ToList();
            }

            if (searchProject != null)
            {
                issues = issues.Where(s => s.ProjectId == searchProject).ToList();
            }

            if (searchState != null)
            {
                issues = issues.Where(s => s.StateId == searchState).ToList();
            }

            return issues;
        }

        private UsersByEmailComparer usersComparer = new UsersByEmailComparer();
        private ProjectsByTitleComparer projectsComparer = new ProjectsByTitleComparer();
        private StatesByTitleComparer statesComparer = new StatesByTitleComparer();

        private IEnumerable<IssueIndexViewModel> GetSortedIssues(IEnumerable<IssueIndexViewModel> issues, string sortKey)
        {
            switch (sortKey)
            {
                case "summary":
                    return issues.OrderBy(ii => ii.Name);
                case "summary_desc":
                    return issues.OrderByDescending(ii => ii.Name);
                case "reporter_desc":
                    return issues.OrderByDescending(ii => ii.Reporter, usersComparer);
                case "reporter":
                    return issues.OrderBy(ii => ii.Reporter, usersComparer);
                case "status_desc":
                    return issues.OrderByDescending(ii => ii.State, statesComparer);
                case "status":
                    return issues.OrderBy(ii => ii.State, statesComparer);
                case "assignee_desc":
                    return issues.OrderByDescending(ii => ii.Assignee, usersComparer);
                case "assignee":
                    return issues.OrderBy(ii => ii.Assignee, usersComparer);
                case "project_desc":
                    return issues.OrderByDescending(ii => ii.Project, projectsComparer);
                case "project":
                    return issues.OrderBy(ii => ii.Project, projectsComparer);
                case "created_desc":
                    return issues.OrderByDescending(ii => ii.Created);
                default:
                    return issues.OrderBy(ii => ii.Created);
            }
        }

        private class UsersByEmailComparer : IComparer<ApplicationUser>
        {
            public int Compare(ApplicationUser x, ApplicationUser y)
            {
                return x.Email.CompareTo(y.Email);
            }
        }

        private class ProjectsByTitleComparer : IComparer<ProjectViewModel>
        {
            public int Compare(ProjectViewModel x, ProjectViewModel y)
            {
                return x.Title.CompareTo(y.Title);
            }
        }

        private class StatesByTitleComparer : IComparer<StateViewModel>
        {
            public int Compare(StateViewModel x, StateViewModel y)
            {
                return x.Title.CompareTo(y.Title);
            }
        }

        // GET: Issues/Details/5
        public ActionResult Details(string id, IssueSubDetail? sub)
        {
            IssueCode code = IssueCode.Parse(id);
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var issue = _issueService.GetByProjectCodeAndIssueNumber(code.ProjectCode, code.IssueNumber);

            var viewModel = new IssueDetailViewModel
            {
                Issue = Mapper.Map<IssueIndexViewModel>(issue)
            };

            if (viewModel.Issue == null)
            {
                return HttpNotFound();
            }

            // possible workflows
            var workflows = stateWorkflowRepo.GetPossibleWorkflows(viewModel.Issue.State.Id);
            foreach (var stateWorkflow in workflows)
            {
                stateWorkflow.ToState = stateRepo.Get(stateWorkflow.ToStateId);
            }
            viewModel.StateWorkflows = Mapper.Map<IEnumerable<StateWorkflowViewModel>>(workflows);

            // comments from all versions of the issue
            var comments = _issueService.GetCommentsForIssue(issue.Id);

            viewModel.Comments = Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            foreach (var comment in viewModel.Comments)
            {
                comment.User = applicationUserRepo.Get(comment.AuthorId);
            }

            viewModel.Changes = getHistory(viewModel.Issue.Id, comments);

            ViewBag.Sub = sub.ToString() == "" ? "Comments" : sub.ToString();
            ViewBag.LoggedUser = getLoggedUser();
            ViewBag.IsUserAdmin = User.IsInRole(UserRoles.Administrators);
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;

            return View(viewModel);
        }

        /// <summary>
        /// Creates history of changes and added comments within given issue.
        /// </summary>
        /// <param name="id">Id of the issue</param>
        /// <param name="comments">Comments from all versions of the issue</param>
        /// <returns>List of all changes and comments</returns>
        private List<IssueChange> getHistory(Guid id, IEnumerable<Comment> comments)
        {
            var allVersions = _issueService.GetAllVersions(id);
            var changes = new List<IssueChange>();

            // find all changes within the issue
            Issue previousVersion = null;
            foreach (var version in allVersions)
            {
                if (previousVersion != null)
                {
                    changes.AddRange(findChange(previousVersion, version));
                }
                previousVersion = version;
            }

            // add comments into history
            changes.AddRange(comments.Select(comment => new IssueChange(comment.CreatedAt, IssueChangeType.Comment, "", comment.Text)));

            return changes.OrderBy(x => x.Changed).ToList();
        }

        /// <summary>
        /// Finds changes within issue between given version and its previous variant (k and k-1).
        /// </summary>
        /// <param name="previous">Previous version of issue</param>
        /// <param name="current">Any version of the issue</param>
        /// <returns>List of found changes</returns>
        private IEnumerable<IssueChange> findChange(Issue previous, Issue current)
        {
            var foundChanges = new List<IssueChange>();

            if (previous.Name != current.Name)
            {
                foundChanges.Add(new IssueChange(current.CreatedAt, IssueChangeType.Title, previous.Name, current.Name));
            }
            else if (previous.StateId != current.StateId)
            {
                foundChanges.Add(new IssueChange(current.CreatedAt, IssueChangeType.State, previous.State.Title, current.State.Title));
            }
            else if (previous.Type != current.Type)
            {
                foundChanges.Add(new IssueChange(current.CreatedAt, IssueChangeType.Type, previous.Type.ToString(), current.Type.ToString()));
            }
            else if (previous.AssigneeId != current.AssigneeId)
            {
                foundChanges.Add(new IssueChange(current.CreatedAt, IssueChangeType.Assignee, previous.Assignee.Email, current.Assignee.Email));
            }
            else if (previous.Description != current.Description)
            {
                foundChanges.Add(new IssueChange(current.CreatedAt, IssueChangeType.Description, previous.Description, current.Description));
            }

            return foundChanges;
        } 


        // GET: Issues/Create
        public ActionResult Create()
        {
            ViewBag.ErrorSQL = TempData["ErrorSQL"] as string;
            
            ViewBag.AssigneeId = new SelectList(applicationUserRepo.GetAll(), "Id", "Email");
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "Id", "Title");
            ViewBag.ReporterId = getLoggedUser().Id;

            return View();
        }

        // POST: Issues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssueCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var initialState = getInitialState();
                if (initialState == null)
                {
                    TempData["ErrorSQL"] = Locale.IssueStrings.ErrorMessageNoIniState;

                    return RedirectToAction("Create");
                }

                var projectTemp = _projectService.GetProject(viewModel.ProjectId);

                var issue = Mapper.Map<Issue>(viewModel);
                issue.StateId = initialState.Id;
                issue.ReporterId = getLoggedUser().Id;
                issue.Created = DateTime.Now;
                issue.ProjectCreatedAt = projectTemp.CreatedAt;
                issue.Id = Guid.NewGuid();
                issue.CodeNumber = _issueService.GetNewCodeNumber();

                _issueService.Add(issue);

                return RedirectToAction("Details", new { id = issue.Code });
            }

            ViewBag.AssigneeId = new SelectList(applicationUserRepo.GetAll(), "Id", "Email", viewModel.AssigneeId);
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "Id", "Title", viewModel.ProjectId);

            return View(viewModel);
        }

        private State getInitialState()
        {
            try
            {
                return stateRepo.GetInitialState();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private ApplicationUser getLoggedUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = applicationUserRepo.GetAll().First(dbUser => dbUser.Email == User.Identity.Name);
                return user;
            }
            return null;
        }
        
        // GET: Issues/Edit/5
        public ActionResult Edit(String id)
        {
            IssueCode code = IssueCode.Parse(id);
            if (code == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var issue = _issueService.GetByProjectCodeAndIssueNumber(code.ProjectCode, code.IssueNumber);

            if (issue == null)
                return HttpNotFound();

            var activeProjects = _projectService.GetProjects();

            var viewModel = Mapper.Map<IssueEditViewModel>(issue);

            ViewBag.AssigneeId = new SelectList(applicationUserRepo.GetAll(), "Id", "Email", issue.AssigneeId);
            ViewBag.ProjectId = new SelectList(activeProjects, "Id", "Title", issue.ProjectId);
            ViewBag.StateId = new SelectList(_stateService.GetStatesOrderedByIndex(), "Id", "Title", issue.StateId);

            return View(viewModel);
        }

        // POST: Issues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Description,AssigneeId,ProjectId")] IssueEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IssueCode code = IssueCode.Parse(viewModel.Code);
                if (code == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                // create a new entity
                var entityNew = _issueService.GetNewEntityForEditing(code.ProjectCode, code.IssueNumber);
                entityNew.Reporter = null;
                entityNew.Assignee = null;
                entityNew.State = null;
                entityNew.Comments = null;
                entityNew.Project = null;

                // in case the project was changed
                if (viewModel.ProjectId != entityNew.ProjectId)
                {
                    var projectTemp = _projectService.GetProject(viewModel.ProjectId);
                    entityNew.ProjectCreatedAt = projectTemp.CreatedAt;
                }
                // map viewModel to the entity
                entityNew = Mapper.Map(viewModel, entityNew);
                // change CreatedAt
                entityNew.CreatedAt = DateTime.Now;
                // save the entity
                _issueService.Add(entityNew);

                return RedirectToAction("Details", new { id = viewModel.Code });

            }

            ViewBag.AssigneeId = new SelectList(applicationUserRepo.GetAll(), "Id", "Email", viewModel.AssigneeId);
            ViewBag.ProjectId = new SelectList(_projectService.GetProjects(), "Id", "Title", viewModel.ProjectId);

            return View(viewModel);
        }

        public ActionResult ChangeStatus(Guid issueId, Guid to)
        {
            // create a new entity
            var entityNew = _issueService.GetNewEntityForEditing(issueId);
            if (entityNew != null)
            {
                // change status
                entityNew.StateId = to;
                // change CreatedAt
                entityNew.CreatedAt = DateTime.Now;
                // save the entity
                entityNew.Reporter = null;
                entityNew.Assignee = null;
                entityNew.State = null;
                entityNew.Comments = null;
                entityNew.Project = null;
                _issueService.Add(entityNew);
            }

            if (HttpContext.Request.UrlReferrer != null)
            {
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }

            return RedirectToAction("Index");
        }

        private class IssueCode {

            public string ProjectCode;
            public int IssueNumber;

            public static IssueCode Parse(String code)
            {
                if (code == null || !MatchIssueCodePattern(code))
                    return null;

                var splittedCode = code.Split('-');
                var projectCode = splittedCode[0];
                var issueNumber = int.Parse(splittedCode[1]);

                if (projectCode == null || issueNumber == 0)
                    return null;
                return new IssueCode{ProjectCode = projectCode, IssueNumber = issueNumber};
            }

            private static bool MatchIssueCodePattern(string s)
            {
                var rgx = new Regex(@"^[A-Z]+[-][0-9]+$"); // E.g.: CODE-19
                return rgx.IsMatch(s);
            }

            private IssueCode()
            {
                ;
            }
        }

        public class IssueChange
        {
            public DateTime Changed { get; set; }
            public IssueChangeType Type { get; set; }
            [AllowHtml]
            public string From { get; set; }
            [AllowHtml]
            public string To { get; set; }

            public IssueChange(DateTime changed, IssueChangeType type, string from, string to)
            {
                Changed = changed;
                Type = type;
                From = from;
                To = to;
            } 
        }
    }
}
