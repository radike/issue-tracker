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
            State state = db.States.Find(id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Colour,IsInitial")] State state)
        {
            if (ModelState.IsValid)
            {
                state.Id = Guid.NewGuid();

                // if there is already initial state, change it to this one
                if (state.IsInitial)
                {
                    RemoveInitialState();
                }

                db.States.Add(Mapper.Map<State>(state));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(state);
        }

        // GET: States/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = db.States.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<StateViewModel>(state));
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Colour,IsInitial")] StateViewModel state)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Mapper.Map<State>(state)).State = EntityState.Modified;

                // if there is already initial state, change it to this one
                if (state.IsInitial)
                {
                    RemoveInitialState();
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(state);
        }

        // GET: States/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = db.States.Find(id);
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
                State state = db.States.Find(id);
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
        private void RemoveInitialState()
        {
            foreach (var s in db.States.Where(s => s.IsInitial))
            {
                s.IsInitial = false;
            }
        }
    }
}
