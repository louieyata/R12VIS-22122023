using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R12VIS.Models;

namespace R12VIS.Controllers
{
    public class PriorityGroupsController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: PriorityGroups
        public ActionResult Index()
        {
            return View(db.PriorityGroups.ToList());
        }

        // GET: PriorityGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityGroup priorityGroup = db.PriorityGroups.Find(id);
            if (priorityGroup == null)
            {
                return HttpNotFound();
            }
            return View(priorityGroup);
        }

        // GET: PriorityGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PriorityGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Category,Description")] PriorityGroup priorityGroup)
        {
            if (ModelState.IsValid)
            {
                db.PriorityGroups.Add(priorityGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(priorityGroup);
        }

        // GET: PriorityGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityGroup priorityGroup = db.PriorityGroups.Find(id);
            if (priorityGroup == null)
            {
                return HttpNotFound();
            }
            return View(priorityGroup);
        }

        // POST: PriorityGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Category,Description")] PriorityGroup priorityGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priorityGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(priorityGroup);
        }

        // GET: PriorityGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriorityGroup priorityGroup = db.PriorityGroups.Find(id);
            if (priorityGroup == null)
            {
                return HttpNotFound();
            }
            return View(priorityGroup);
        }

        // POST: PriorityGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriorityGroup priorityGroup = db.PriorityGroups.Find(id);
            db.PriorityGroups.Remove(priorityGroup);
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
