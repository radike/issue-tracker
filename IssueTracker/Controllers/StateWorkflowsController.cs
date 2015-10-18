using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using IssueTracker.DAL;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class StateWorkflowsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StateWorkflows
        public ActionResult Index()
        {
            var stateWorkflows = db.StateWorkflows.Include(s => s.FromState).Include(s => s.ToState);
            return View(stateWorkflows.ToList());
        }

        // GET: StateWorkflows/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StateWorkflow stateWorkflow = db.StateWorkflows.Find(id);
            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }
            return View(stateWorkflow);
        }

        // GET: StateWorkflows/Create
        public ActionResult Create()
        {
            ViewBag.FromStateId = new SelectList(db.States, "Id", "Title");
            ViewBag.ToStateId = new SelectList(db.States, "Id", "Title");
            return View();
        }

        // POST: StateWorkflows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FromStateId,ToStateId")] StateWorkflow stateWorkflow)
        {
            if (ModelState.IsValid)
            {
                stateWorkflow.Id = Guid.NewGuid();
                db.StateWorkflows.Add(stateWorkflow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FromStateId = new SelectList(db.States, "Id", "Title", stateWorkflow.FromStateId);
            ViewBag.ToStateId = new SelectList(db.States, "Id", "Title", stateWorkflow.ToStateId);
            return View(stateWorkflow);
        }

        // GET: StateWorkflows/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StateWorkflow stateWorkflow = db.StateWorkflows.Find(id);
            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }
            ViewBag.FromStateId = new SelectList(db.States, "Id", "Title", stateWorkflow.FromStateId);
            ViewBag.ToStateId = new SelectList(db.States, "Id", "Title", stateWorkflow.ToStateId);
            return View(stateWorkflow);
        }

        // POST: StateWorkflows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FromStateId,ToStateId")] StateWorkflow stateWorkflow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stateWorkflow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FromStateId = new SelectList(db.States, "Id", "Title", stateWorkflow.FromStateId);
            ViewBag.ToStateId = new SelectList(db.States, "Id", "Title", stateWorkflow.ToStateId);
            return View(stateWorkflow);
        }

        // GET: StateWorkflows/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StateWorkflow stateWorkflow = db.StateWorkflows.Find(id);
            if (stateWorkflow == null)
            {
                return HttpNotFound();
            }
            return View(stateWorkflow);
        }

        // POST: StateWorkflows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StateWorkflow stateWorkflow = db.StateWorkflows.Find(id);
            db.StateWorkflows.Remove(stateWorkflow);
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
