using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using IssueTracker.ViewModels;
using System.Collections.Generic;
using Entities;
using IssueTracker.Data;
using IssueTracker.Data.Data_Repositories;
using IssueTracker.Data.Contracts.Repository_Interfaces;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage(Roles = "Administrators")]
    public class StateWorkflowsController : Controller
    {
        private IssueTrackerContext db = new IssueTrackerContext();
        private IStateWorkflowRepository stateWorkflowRepo = new StateWorkflowRepository();
        private IStateRepository stateRepo = new StateRepository();

        // GET: StateWorkflows
        public ActionResult Index()
        {
            var stateWorkflows = stateWorkflowRepo.Get().AsQueryable().Include(s => s.FromState).Include(s => s.ToState);

            return View(Mapper.Map<IEnumerable<StateWorkflowViewModel>>(stateWorkflows).ToList());
        }

        // GET: StateWorkflows/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stateWorkflow = stateWorkflowRepo.Get((Guid)id);

            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<StateWorkflowViewModel>(stateWorkflow));
        }

        // GET: StateWorkflows/Create
        public ActionResult Create()
        {
            ViewBag.FromStateId = new SelectList(stateRepo.Get(), "Id", "Title");
            ViewBag.ToStateId = new SelectList(stateRepo.Get(), "Id", "Title");

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
                    ViewBag.FromStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.FromStateId);
                    ViewBag.ToStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.ToStateId);

                    return View(viewModel);
                }

                viewModel.Id = Guid.NewGuid();

                stateWorkflowRepo.Add(Mapper.Map<StateWorkflow>(viewModel));

                return RedirectToAction("Index");
            }

            ViewBag.FromStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.FromStateId);
            ViewBag.ToStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.ToStateId);

            return View(viewModel);
        }

        // GET: StateWorkflows/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stateWorkflow = stateWorkflowRepo.Get((Guid)id);

            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }

            ViewBag.FromStateId = new SelectList(stateRepo.Get(), "Id", "Title", stateWorkflow.FromStateId);
            ViewBag.ToStateId = new SelectList(stateRepo.Get(), "Id", "Title", stateWorkflow.ToStateId);

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
                    ViewBag.FromStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.FromStateId);
                    ViewBag.ToStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.ToStateId);

                    return View(viewModel);
                }

                stateWorkflowRepo.Update(Mapper.Map<StateWorkflow>(viewModel));
                
                return RedirectToAction("Index");
            }

            ViewBag.FromStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.FromStateId);
            ViewBag.ToStateId = new SelectList(stateRepo.Get(), "Id", "Title", viewModel.ToStateId);

            return View(viewModel);
        }

        // GET: StateWorkflows/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var stateWorkflow = stateWorkflowRepo.Get((Guid)id);

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
            var stateWorkflow = stateWorkflowRepo.Get((Guid)id);

            stateWorkflowRepo.Remove(stateWorkflow);

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