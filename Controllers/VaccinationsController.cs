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
    public class VaccinationsController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: Vaccinations
        public ActionResult Index()
        {
            var vaccinations = db.Vaccinations.Include(v => v.Adverse).Include(v => v.Deferral).Include(v => v.Dose).Include(v => v.Person).Include(v => v.PriorityGroup).Include(v => v.Vaccine);
            return View(vaccinations.ToList());
        }

        // GET: Vaccinations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccination vaccination = db.Vaccinations.Find(id);
            if (vaccination == null)
            {
                return HttpNotFound();
            }
            return View(vaccination);
        }

        // GET: Vaccinations/Create
        public ActionResult Create()
        {
            ViewBag.AdverseID = new SelectList(db.Adverses, "ID", "Event");
            ViewBag.DeferralID = new SelectList(db.Deferrals, "Id", "Reason");
            ViewBag.DoseID = new SelectList(db.Dose, "ID", "VaccineDose");
            ViewBag.PersonID = new SelectList(db.Persons, "ID", "UniquePersonID");
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category");
            ViewBag.VaccineID = new SelectList(db.Vaccines, "ID", "VaccineManufacturer");
            return View();
        }

        // POST: Vaccinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PriorityGroupID,PersonID,ProvinceID,CityMunicipalityID,BarangayID,DeferralID,VaccinationDate,VaccineID,BatchNumber,LotNumber,BakunaCenterCBCRID,VaccinatorName,DoseID,AdverseID")] Vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                db.Vaccinations.Add(vaccination);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdverseID = new SelectList(db.Adverses, "ID", "Event", vaccination.AdverseID);
            ViewBag.DeferralID = new SelectList(db.Deferrals, "Id", "Reason", vaccination.DeferralID);
            ViewBag.DoseID = new SelectList(db.Dose, "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.PersonID = new SelectList(db.Persons, "ID", "UniquePersonID", vaccination.PersonID);
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category", vaccination.PriorityGroupID);
            ViewBag.VaccineID = new SelectList(db.Vaccines, "ID", "VaccineManufacturer", vaccination.VaccineID);
            return View(vaccination);
        }

        // GET: Vaccinations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccination vaccination = db.Vaccinations.Find(id);
            if (vaccination == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdverseID = new SelectList(db.Adverses, "ID", "Event", vaccination.AdverseID);
            ViewBag.DeferralID = new SelectList(db.Deferrals, "Id", "Reason", vaccination.DeferralID);
            ViewBag.DoseID = new SelectList(db.Dose, "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.PersonID = new SelectList(db.Persons, "ID", "UniquePersonID", vaccination.PersonID);
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category", vaccination.PriorityGroupID);
            ViewBag.VaccineID = new SelectList(db.Vaccines, "ID", "VaccineManufacturer", vaccination.VaccineID);
            return View(vaccination);
        }

        // POST: Vaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PriorityGroupID,PersonID,ProvinceID,CityMunicipalityID,BarangayID,DeferralID,VaccinationDate,VaccineID,BatchNumber,LotNumber,BakunaCenterCBCRID,VaccinatorName,DoseID,AdverseID")] Vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vaccination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdverseID = new SelectList(db.Adverses, "ID", "Event", vaccination.AdverseID);
            ViewBag.DeferralID = new SelectList(db.Deferrals, "Id", "Reason", vaccination.DeferralID);
            ViewBag.DoseID = new SelectList(db.Dose, "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.PersonID = new SelectList(db.Persons, "ID", "UniquePersonID", vaccination.PersonID);
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category", vaccination.PriorityGroupID);
            ViewBag.VaccineID = new SelectList(db.Vaccines, "ID", "VaccineManufacturer", vaccination.VaccineID);
            return View(vaccination);
        }

        // GET: Vaccinations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccination vaccination = db.Vaccinations.Find(id);
            if (vaccination == null)
            {
                return HttpNotFound();
            }
            return View(vaccination);
        }

        // POST: Vaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vaccination vaccination = db.Vaccinations.Find(id);
            db.Vaccinations.Remove(vaccination);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PreVaccination(int? id)
        {
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category");
            var ethnicGroups = db.EthnicGroups.ToList();
            ethnicGroups.Insert(0, new EthnicGroup { Id = 0, IndigenousMember = "" });
            ViewBag.EthnicGroupID = new SelectList(ethnicGroups, "Id", "IndigenousMember");
            ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name"); return View();
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
