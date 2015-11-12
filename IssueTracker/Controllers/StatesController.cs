using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IssueTracker.DAL;
using IssueTracker.Entities;
using AutoMapper;
using IssueTracker.ViewModels;
using System.Collections.Generic;

namespace IssueTracker.Controllers
{
    public class StatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: States
        public ActionResult Index()
        {
            return View(Mapper.Map<IEnumerable<StateViewModel>>(db.States).ToList());
        }

        // GET: States/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var state = db.States.Find(id);

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
                    removeInitialState();
                }

                viewModel.OrderIndex = db.States.Max(x => (int?)x.OrderIndex) + 1 ?? 1;
                db.States.Add(Mapper.Map<State>(viewModel));
                db.SaveChanges();

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

            var state = db.States.Find(id);

            if (state == null)
            {
                return HttpNotFound();
            }

            return View(Mapper.Map<StateViewModel>(state));
        }

        // POST: States/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Colour,IsInitial")] StateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Mapper.Map<State>(viewModel)).State = EntityState.Modified;

                // if there is already initial state, change it to this one
                if (viewModel.IsInitial)
                {
                    removeInitialState();
                }

                db.SaveChanges();

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

            var state = db.States.Find(id);

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
                var state = db.States.Find(id);
                db.States.Remove(state);
                db.SaveChanges();

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
                var movedStates = db.States
                            .Where(c => (toPosition <= c.OrderIndex && c.OrderIndex <= fromPosition))
                            .ToList();

                foreach (var state in movedStates)
                {
                    state.OrderIndex++;
                }
            }
            else
            {
                var movedStates = db.States
                            .Where(c => (fromPosition <= c.OrderIndex && c.OrderIndex <= toPosition))
                            .ToList();
                foreach (var state in movedStates)
                {
                    state.OrderIndex--;
                }
            }

            db.States.First(c => c.Id == id).OrderIndex = toPosition;
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
        /// Removes IsInitial flag on each state in database
        /// </summary>
        private void removeInitialState()
        {
            foreach (var s in db.States.Where(s => s.IsInitial))
            {
                s.IsInitial = false;
            }
        }
    }
}
