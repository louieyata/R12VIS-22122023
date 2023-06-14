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
    public class DeferralsController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: Deferrals
        public ActionResult Index()
        {
            return View(db.Deferrals.ToList());
        }

        // GET: Deferrals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deferral deferral = db.Deferrals.Find(id);
            if (deferral == null)
            {
                return HttpNotFound();
            }
            return View(deferral);
        }

        // GET: Deferrals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Deferrals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Reason")] Deferral deferral)
        {
            if (ModelState.IsValid)
            {
                db.Deferrals.Add(deferral);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deferral);
        }

        // GET: Deferrals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deferral deferral = db.Deferrals.Find(id);
            if (deferral == null)
            {
                return HttpNotFound();
            }
            return View(deferral);
        }

        // POST: Deferrals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Reason")] Deferral deferral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deferral).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deferral);
        }

        // GET: Deferrals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deferral deferral = db.Deferrals.Find(id);
            if (deferral == null)
            {
                return HttpNotFound();
            }
            return View(deferral);
        }

        // POST: Deferrals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deferral deferral = db.Deferrals.Find(id);
            db.Deferrals.Remove(deferral);
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
