using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using IssueTracker.ViewModels;
using System.Collections.Generic;
using IssueTracker.Data.Entities;
using IssueTracker.Data;
using IssueTracker.Data.Data_Repositories;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage(Roles = UserRoles.Administrators)]
    public class StateWorkflowsController : Controller
    {
        private IStateWorkflowRepository _stateWorkflowRepo;
        private IStateRepository _stateRepo;
        
        public StateWorkflowsController(IStateWorkflowRepository stateWorkflowRepository, IStateRepository stateRepository)
        {
            _stateWorkflowRepo = stateWorkflowRepository;
            _stateRepo = stateRepository;
        }
        // GET: StateWorkflows
        public ActionResult Index()
        {
            var stateWorkflows = _stateWorkflowRepo.GetAll().AsQueryable().Include(s => s.FromState).Include(s => s.ToState);
            foreach (var stateWorkflow in stateWorkflows)
            {
                stateWorkflow.FromState = _stateRepo.Get(stateWorkflow.FromStateId);
                stateWorkflow.ToState = _stateRepo.Get(stateWorkflow.ToStateId);
            }
            return View(Mapper.Map<IEnumerable<StateWorkflowViewModel>>(stateWorkflows).ToList());
        }

        // GET: StateWorkflows/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stateWorkflow = _stateWorkflowRepo.Get((Guid)id);

            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<StateWorkflowViewModel>(stateWorkflow));
        }

        // GET: StateWorkflows/Create
        public ActionResult Create()
        {
            ViewBag.FromStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title");
            ViewBag.ToStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title");

            return View();
        }

        // POST: StateWorkflows/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FromStateId,ToStateId")] StateWorkflowViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.FromStateId == viewModel.ToStateId)
                {
                    ViewBag.ErrorSameFromAndTo = "You have created invalid transition. From and To cannot be same.";
                    ViewBag.FromStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.FromStateId);
                    ViewBag.ToStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.ToStateId);

                    return View(viewModel);
                }

                viewModel.Id = Guid.NewGuid();

                _stateWorkflowRepo.Add(Mapper.Map<StateWorkflow>(viewModel));

                return RedirectToAction("Index");
            }

            ViewBag.FromStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.FromStateId);
            ViewBag.ToStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.ToStateId);

            return View(viewModel);
        }

        // GET: StateWorkflows/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stateWorkflow = _stateWorkflowRepo.Get((Guid)id);

            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }

            ViewBag.FromStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", stateWorkflow.FromStateId);
            ViewBag.ToStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", stateWorkflow.ToStateId);

            return View(Mapper.Map<StateWorkflowViewModel>(stateWorkflow));
        }

        // POST: StateWorkflows/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FromStateId,ToStateId")] StateWorkflowViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.FromStateId == viewModel.ToStateId)
                {
                    ViewBag.ErrorSameFromAndTo = "You have created invalid transition. From and To cannot be same.";
                    ViewBag.FromStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.FromStateId);
                    ViewBag.ToStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.ToStateId);

                    return View(viewModel);
                }

                _stateWorkflowRepo.Update(Mapper.Map<StateWorkflow>(viewModel));
                
                return RedirectToAction("Index");
            }

            ViewBag.FromStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.FromStateId);
            ViewBag.ToStateId = new SelectList(_stateRepo.GetAll(), "Id", "Title", viewModel.ToStateId);

            return View(viewModel);
        }

        // GET: StateWorkflows/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stateWorkflow = _stateWorkflowRepo.Get((Guid)id);

            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<StateWorkflowViewModel>(stateWorkflow));
        }

        // POST: StateWorkflows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var stateWorkflow = _stateWorkflowRepo.Get((Guid)id);

            _stateWorkflowRepo.Remove(stateWorkflow);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Creates JSON of states and worflows within the states for GOJS visualization.
        /// </summary>
        /// <returns>JSON of states and workflows</returns>
        public JsonResult WorkflowVisualization()
        {
            var stateWorkflows = _stateWorkflowRepo.GetAll().Select(e => new {
                e.FromStateId,
                e.ToStateId
            });
            var states = _stateRepo.GetAll().Select(e => new {
                e.Id,
                e.Title,
                e.Colour
            });

            var stateDictionary = new Dictionary<Guid, string>();
            var nds = new List<object>();
            var lnks = new List<object>();

            foreach (var state in states)
            {
                stateDictionary.Add(state.Id, state.Title);
                nds.Add(new { key = state.Title, color = state.Colour });
            }

            foreach (var stateWorkflow in stateWorkflows)
            {
                string from;
                string to;

                stateDictionary.TryGetValue(stateWorkflow.FromStateId, out from);
                stateDictionary.TryGetValue(stateWorkflow.ToStateId, out to);
                
                lnks.Add(new {from, to});
            }

            var result = new { nodes = nds, links = lnks};

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}