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
    public class DosesController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: Doses
        public ActionResult Index()
        {
            return View(db.Dose.ToList());
        }

        // GET: Doses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dose dose = db.Dose.Find(id);
            if (dose == null)
            {
                return HttpNotFound();
            }
            return View(dose);
        }

        // GET: Doses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VaccineDose")] Dose dose)
        {
            if (ModelState.IsValid)
            {
                db.Dose.Add(dose);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dose);
        }

        // GET: Doses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dose dose = db.Dose.Find(id);
            if (dose == null)
            {
                return HttpNotFound();
            }
            return View(dose);
        }

        // POST: Doses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VaccineDose")] Dose dose)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dose).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dose);
        }

        // GET: Doses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dose dose = db.Dose.Find(id);
            if (dose == null)
            {
                return HttpNotFound();
            }
            return View(dose);
        }

        // POST: Doses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dose dose = db.Dose.Find(id);
            db.Dose.Remove(dose);
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
