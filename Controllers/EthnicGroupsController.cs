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
    public class EthnicGroupsController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: EthnicGroups
        public ActionResult Index()
        {
            return View(db.EthnicGroups.ToList());
        }

        // GET: EthnicGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthnicGroup ethnicGroup = db.EthnicGroups.Find(id);
            if (ethnicGroup == null)
            {
                return HttpNotFound();
            }
            return View(ethnicGroup);
        }

        // GET: EthnicGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EthnicGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IndigenousMember")] EthnicGroup ethnicGroup)
        {
            if (ModelState.IsValid)
            {
                db.EthnicGroups.Add(ethnicGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ethnicGroup);
        }

        // GET: EthnicGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthnicGroup ethnicGroup = db.EthnicGroups.Find(id);
            if (ethnicGroup == null)
            {
                return HttpNotFound();
            }
            return View(ethnicGroup);
        }

        // POST: EthnicGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IndigenousMember")] EthnicGroup ethnicGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ethnicGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ethnicGroup);
        }

        // GET: EthnicGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthnicGroup ethnicGroup = db.EthnicGroups.Find(id);
            if (ethnicGroup == null)
            {
                return HttpNotFound();
            }
            return View(ethnicGroup);
        }

        // POST: EthnicGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EthnicGroup ethnicGroup = db.EthnicGroups.Find(id);
            db.EthnicGroups.Remove(ethnicGroup);
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
