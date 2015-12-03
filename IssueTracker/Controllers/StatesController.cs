using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using IssueTracker.ViewModels;
using System.Collections.Generic;
using System.Threading;
using IssueTracker.Data;
using Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Data_Repositories;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage(Roles = "Administrators")]
    public class StatesController : Controller
    {
        private IssueTrackerContext db = new IssueTrackerContext();
        private IStateRepository stateRepo = new StateRepository();
 
        // GET: States
        public ActionResult Index()
        {
            ViewBag.DefaultCulture = Thread.CurrentThread.CurrentCulture.Name;

            return View(Mapper.Map<IEnumerable<StateViewModel>>(stateRepo.Get().ToList()));
        }

        // GET: States/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = stateRepo.Get((Guid)id);

            if (state == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<StateViewModel>(state));
        }

        // GET: States/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Colour,IsInitial")] StateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Id = Guid.NewGuid();

                // if there is already initial state, change it to this one
                if (viewModel.IsInitial)
                {
                    removeInitialState(viewModel.Id);
                }

                viewModel.OrderIndex = stateRepo.Get().Max(x => (int?)x.OrderIndex) + 1 ?? 1;
                stateRepo.Add(Mapper.Map<State>(viewModel));

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: States/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = stateRepo.Get((Guid)id);

            if (state == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<StateViewModel>(state));
        }

        // POST: States/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Colour,IsInitial,OrderIndex")] StateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                stateRepo.Update(Mapper.Map<State>(viewModel));

                // if there is already initial state, change it to this one
                if (viewModel.IsInitial)
                {
                    removeInitialState(viewModel.Id);
                }
                
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: States/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = stateRepo.Get((Guid)id);

            if (state == null)
            {
                return HttpNotFound();
            }

            ViewBag.ErrorSQL = TempData["ErrorSQL"] as string;

            return View(Mapper.Map<StateViewModel>(state));
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var state = stateRepo.Get(id);
                stateRepo.Remove(state);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorSQL"] = "There is some workflow transition or issue using this state. The removal was terminated.";

                return RedirectToAction("Delete", "States", new { id = id });
            }

        }

        /// <summary>
        /// Updates reordering of the states in States/Index table.
        /// </summary>
        /// <param name="id">Id of the state</param>
        /// <param name="fromPosition">Initial position of the state</param>
        /// <param name="toPosition">New position of the state</param>
        /// <param name="direction">Back or forward direction</param>
        public void UpdateOrder(Guid id, int fromPosition, int toPosition, string direction)
        {
            if (direction == "back")
            {
                var movedStates = stateRepo.Get()
                            .Where(c => (toPosition <= c.OrderIndex && c.OrderIndex <= fromPosition))
                            .ToList();

                foreach (var state in movedStates)
                {
                    state.OrderIndex++;
                }
            }
            else
            {
                var movedStates = stateRepo.Get()
                            .Where(c => (fromPosition <= c.OrderIndex && c.OrderIndex <= toPosition))
                            .ToList();
                foreach (var state in movedStates)
                {
                    state.OrderIndex--;
                }
            }

            stateRepo.Get().First(c => c.Id == id).OrderIndex = toPosition;
            db.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Removes IsInitial flag on each state in database except one.
        /// </summary>
        /// <param name="id">Id of state, which flag is not removed</param>
        private void removeInitialState(Guid id)
        {
            foreach (var s in stateRepo.Get().Where(s => s.IsInitial))
            {
                if (s.Id != id)
                {
                   s.IsInitial = false;
                }
            }
        }
    }
}
