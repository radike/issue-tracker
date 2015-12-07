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
using IssueTracker.Data.Entities;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Data_Repositories;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage(Roles = "Administrators")]
    public class StatesController : Controller
    {
        private IStateRepository _stateRepo;

        public StatesController(IStateRepository stateRepository)
        {
            _stateRepo = stateRepository;
        }
 
        // GET: States
        public ActionResult Index()
        {
            ViewBag.DefaultCulture = Thread.CurrentThread.CurrentCulture.Name;
            ViewBag.FinalStates = _stateRepo.GetFinalStateIds();

            return View(Mapper.Map<IEnumerable<StateViewModel>>(_stateRepo.GetAll().ToList()));
        }

        // GET: States/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = _stateRepo.Get((Guid)id);

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

                viewModel.OrderIndex = _stateRepo.GetAll().Max(x => (int?)x.OrderIndex) + 1 ?? 1;
                _stateRepo.Add(Mapper.Map<State>(viewModel));

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

            var state = _stateRepo.Get((Guid)id);

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
                _stateRepo.Update(Mapper.Map<State>(viewModel));

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

            var state = _stateRepo.Get((Guid)id);

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
                var state = _stateRepo.Get(id);
                _stateRepo.Remove(state);

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
                var movedStates = _stateRepo.GetAll()
                            .Where(c => (toPosition <= c.OrderIndex && c.OrderIndex <= fromPosition))
                            .ToList();

                foreach (var state in movedStates)
                {
                    state.OrderIndex++;
                }
            }
            else
            {
                var movedStates = _stateRepo.GetAll()
                            .Where(c => (fromPosition <= c.OrderIndex && c.OrderIndex <= toPosition))
                            .ToList();
                foreach (var state in movedStates)
                {
                    state.OrderIndex--;
                }
            }

            _stateRepo.GetAll().First(c => c.Id == id).OrderIndex = toPosition;
            _stateRepo.Save();
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Removes IsInitial flag on each state in database except one.
        /// </summary>
        /// <param name="id">Id of state, which flag is not removed</param>
        private void removeInitialState(Guid id)
        {
            foreach (var s in _stateRepo.GetAll().Where(s => s.IsInitial && s.Id != id))
            {
                s.IsInitial = false;
                _stateRepo.Update(s);
            }
        }
    }
}
