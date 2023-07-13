using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R12VIS.Models;
using R12VIS.Models.ViewModel;

namespace R12VIS.Controllers
{
    public class PeopleController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: People
        public ActionResult Index()
        {
            //User user = new User();
            var persons = db.Persons
                .Include(x=>x.Province)
                .Include(x=>x.CityMunicipality)
                .Include(x => x.Barangay)
                .Include(x => x.EthnicGroup)
                .ToList();
            //List<_VaccinedPersons> personList = persons.Select(x => new _VaccinedPersons
            //{
            //    Id = x.ID,
            //    Name = x.Person.LastName + ", " + x.Person.FirstName,
            //    Address = x.Person.Barangay.barangay_name + ", " + x.Person.CityMunicipality.CityMunicipalityName + ", " + x.Person.Province.province_name,
            //    Dose = x.Dose,
            //    Vaccine = x.Vaccine,
            //    PriorityGroup = x.PriorityGroup.Category

            //}).ToList();
            return View(persons);
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            _PersonProfile _PersonProfile = new _PersonProfile()
            {
                Person = person,
               Vaccinations = db.Vaccinations
               .Include(x => x.Person)
               .Include(x => x.Dose)
               .Include(x=>x.Vaccine)
               .Include(x=>x.Person.Province)
               .Include(x=>x.Person.CityMunicipality)
               .Include(x=>x.PriorityGroup)
               .Where(x=>x.PersonID == person.ID)
               .ToList()
            };
            return View(_PersonProfile);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.BarangayID = new SelectList(db.Barangays, "barangay_id", "barangay_name");
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName");
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "province_id", "province_name");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UniquePersonID,FirstName,MiddleName,LastName,Suffix,ContactNumber,GuardianName,Gender,isPWD,EthnicGroupID,BirthDate,ProvinceID,CityMunicipalityID,BarangayID")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BarangayID = new SelectList(db.Barangays, "barangay_id", "barangay_name", person.BarangayID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", person.CityMunicipalityID);
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember", person.EthnicGroupID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "province_id", "province_name", person.ProvinceID);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.BarangayID = new SelectList(db.Barangays, "barangay_id", "barangay_name", person.BarangayID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", person.CityMunicipalityID);
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember", person.EthnicGroupID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "province_id", "province_name", person.ProvinceID);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UniquePersonID,FirstName,MiddleName,LastName,Suffix,ContactNumber,GuardianName,Gender,isPWD,EthnicGroupID,BirthDate,ProvinceID,CityMunicipalityID,BarangayID")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BarangayID = new SelectList(db.Barangays, "barangay_id", "barangay_name", person.BarangayID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", person.CityMunicipalityID);
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember", person.EthnicGroupID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "province_id", "province_name", person.ProvinceID);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public PartialViewResult VaccineRecord(_PersonProfile _PersonProfile)
        {

            //if (_PersonProfile.Person.ID < 1)
            //{
            //    List<Vaccination> emptyList = new List<Vaccination>();
            //    _PersonProfile.Vaccinations = emptyList;
            //}
            List<Vaccination> vaccineRecords = db.Vaccinations
                .Include(x => x.Person)
                .Where(x => x.Person.ID == _PersonProfile.Person.ID)
                .ToList();

            _PersonProfile.Vaccinations = vaccineRecords;

            return PartialView(_PersonProfile);
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
