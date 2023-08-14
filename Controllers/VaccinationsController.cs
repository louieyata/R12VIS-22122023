using R12VIS.Models;
using R12VIS.Models.ViewModel;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    [UserAuthenticationFilter]
    public class VaccinationsController : Controller
    {
        private DbContextR12 db = new DbContextR12();


        // GET: Vaccinations
        public ActionResult Index()
        {
            var vaccinations = db.Vaccinations.Include(v => v.Adverse)
                .Include(v => v.Deferral)
                .Include(v => v.Dose)
                .Include(v => v.Person)
                .Include(v => v.PriorityGroup)
                .Include(v => v.Vaccine)
                .Include(v => v.Person.EthnicGroup)
                .Include(v => v.Person.Province)
                .Include(v => v.Person.CityMunicipality)
                .Include(v => v.Person.Barangay);
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
            Vaccination vaccination = db.Vaccinations
                .Include(x => x.Person)
                .Where(x => x.ID == id)
                .FirstOrDefault();
            if (vaccination == null)
            {
                return HttpNotFound();
            }
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category", vaccination.PriorityGroupID);
            var ethnicGroups = db.EthnicGroups.ToList();
            ethnicGroups.Insert(0, new EthnicGroup { Id = 0, IndigenousMember = "" });
            ViewBag.EthnicGroupID = new SelectList(ethnicGroups, "Id", "IndigenousMember", vaccination.Person.EthnicGroupID);
            ViewBag.ProvinceID = new SelectList(db.Provinces.OrderBy(p => p.province_name), "province_id", "province_name", vaccination.Person.ProvinceID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName), "city_municipality_id", "CityMunicipalityName", vaccination.Person.CityMunicipalityID);
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100), "barangay_id", "barangay_name", vaccination.Person.BarangayID);
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason", vaccination.DeferralID);
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer", vaccination.VaccineID);
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition", vaccination.AdverseID);


            SelectList gender = new SelectList(new[]
            {
                new {Value = "Male", Text = "Male"},
                new {Value = "Female", Text = "Female"}
            }, "Value", "Text", vaccination.Person.Gender);

            SelectList suffix = new SelectList(new[]
            {
                new {Value =String.Empty, Text ="NA"},
                new { Value = "II", Text = "II"},
                new { Value = "III", Text = "III"},
                new { Value = "IV", Text = "IV"},
                new { Value = "V", Text = "V"},
                new { Value = "SR.", Text = "SR."},
                new { Value = "JR.", Text = "JR."},
            }, "Value", "Text", vaccination.Person.Suffix);

            ViewBag.Suffix = suffix;
            ViewBag.Gender = gender;

            return View(vaccination);
        }

        // POST: Vaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                vaccination.Person.FirstName = vaccination.Person.FirstName?.ToUpper();
                vaccination.Person.LastName = vaccination.Person.LastName?.ToUpper();
                vaccination.Person.MiddleName = vaccination.Person.MiddleName?.ToUpper();
                vaccination.Person.Suffix = vaccination.Person.Suffix?.ToUpper();
                vaccination.VaccinatorName = vaccination.VaccinatorName?.ToUpper();
                vaccination.Comorbidity = vaccination.Comorbidity?.ToUpper();
                db.Entry(vaccination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category", vaccination.PriorityGroupID);
            var ethnicGroups = db.EthnicGroups.ToList();
            ethnicGroups.Insert(0, new EthnicGroup { Id = 0, IndigenousMember = "" });
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember", vaccination.Person.EthnicGroupID);
            ViewBag.ProvinceID = new SelectList(db.Provinces.OrderBy(p => p.province_name), "province_id", "province_name", vaccination.Person.ProvinceID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName), "city_municipality_id", "CityMunicipalityName", vaccination.Person.CityMunicipalityID);
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100), "barangay_id", "barangay_name", vaccination.Person.BarangayID);
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason", vaccination.DeferralID);
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer", vaccination.VaccineID);
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition", vaccination.AdverseID);


            SelectList gender = new SelectList(new[]
            {
                new {Value = "Male", Text = "Male"},
                new {Value = "Female", Text = "Female"}
            }, "Value", "Text", vaccination.Person.Gender);

            SelectList suffix = new SelectList(new[]
           {
                new { Value = String.Empty, Text ="NA"},
                new { Value = "II", Text = "II"},
                new { Value = "III", Text = "III"},
                new { Value = "IV", Text = "IV"},
                new { Value = "V", Text = "V"},
                new { Value = "SR.", Text = "SR."},
                new { Value = "JR.", Text = "JR."},
            }, "Value", "Text", vaccination.Person.Suffix);

            ViewBag.Suffix = suffix;

            ViewBag.Gender = gender;
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
            SelectList gender = new SelectList(new[]
            {
                new {Value = "Male", Text = "Male"},
                new {Value = "Female", Text = "Female"}
            }, "Value", "Text");
            SelectList suffix = new SelectList(new[]
            {
                new {Value =String.Empty, Text ="NA"},
                new { Value = "II", Text = "II"},
                new { Value = "III", Text = "III"},
                new { Value = "IV", Text = "IV"},
                new { Value = "V", Text = "V"},
                new { Value = "SR.", Text = "SR."},
                new { Value = "JR.", Text = "JR."},
            }, "Value", "Text");

            Person person = new Person();

            if (id != null || id > 0)
            {
                person = db.Persons.Find(id);
                if (person == null)
                {
                    return new HttpNotFoundResult();
                }
                else
                {

                    ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups.OrderBy(p => p.IndigenousMember).Where(x => x.Id == person.EthnicGroupID), "Id", "IndigenousMember");
                    ViewBag.ProvinceID = new SelectList(db.Provinces.OrderBy(p => p.province_name).Where(x => x.province_id == person.ProvinceID), "province_id", "province_name");
                    ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName).Where(x => x.city_municipality_id == person.CityMunicipalityID), "city_municipality_id", "CityMunicipalityName");
                    ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100).Where(x => x.barangay_id == person.BarangayID), "barangay_id", "barangay_name");
                    ViewBag.Gender = new SelectList(gender.Where(x => x.Value.ToLower() == person.Gender.ToLower()), "Value", "Text");
                    ViewBag.Suffix = new SelectList(suffix.Where(x => x.Value.ToLower() == person.Suffix?.ToLower()), "Value", "Text");

                    ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category");
                    ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason");
                    ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer");
                    ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose");
                    ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition");



                    Vaccination vaccination1 = new Vaccination()
                    {
                        PersonID = person.ID,
                        Person = person
                    };

                    return View(vaccination1);
                }
            }

            Vaccination vaccination = new Vaccination();

            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category");
            //var ethnicGroups = db.EthnicGroups.ToList();
            //ethnicGroups.Insert(0, new EthnicGroup { Id = 0, IndigenousMember = "NA" });
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups.OrderBy(p => p.IndigenousMember), "Id", "IndigenousMember");
            ViewBag.ProvinceID = new SelectList(db.Provinces.OrderBy(p => p.province_name), "province_id", "province_name");
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName).Take(1), "city_municipality_id", "CityMunicipalityName");
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(1), "barangay_id", "barangay_name");
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason");
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer");
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose");
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition");
            ViewBag.Gender = gender;
            ViewBag.Suffix = suffix;

            return View(vaccination);
        }
        [HttpPost]
        public ActionResult PreVaccination(Vaccination vaccination)
        {
            User user = Session["User"] as User;
            Person person = new Person();
            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }
            if (ModelState.IsValid)
            {
                vaccination.VaccinatorName = vaccination.VaccinatorName?.ToUpper();
                vaccination.Comorbidity = vaccination.Comorbidity?.ToUpper();
                vaccination.CreatedBy = $"{user.FirstName} {user.LastName}";
                vaccination.UserID = user.ID;

                Person personExist = CheckPersonIfExisitng(vaccination.Person);
                if (personExist != null)
                {
                    vaccination.Person = personExist;
                }
                vaccination.Person.FirstName = vaccination.Person.FirstName?.ToUpper();
                vaccination.Person.MiddleName = vaccination.Person.MiddleName?.ToUpper();
                vaccination.Person.LastName = vaccination.Person.LastName?.ToUpper();
                db.Vaccinations.Add(vaccination);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PriorityGroupID = new SelectList(db.PriorityGroups, "ID", "Category", vaccination.PriorityGroupID);
            var ethnicGroups = db.EthnicGroups.ToList();
            ethnicGroups.Insert(0, new EthnicGroup { Id = 0, IndigenousMember = "" });
            ViewBag.EthnicGroupID = new SelectList(ethnicGroups, "Id", "IndigenousMember", vaccination.Person.EthnicGroupID);
            ViewBag.ProvinceID = new SelectList(db.Provinces.OrderBy(p => p.province_name), "province_id", "province_name", vaccination.Person.ProvinceID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName), "city_municipality_id", "CityMunicipalityName", vaccination.Person.CityMunicipalityID);
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100), "barangay_id", "barangay_name", vaccination.Person.BarangayID);
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason", vaccination.DeferralID);
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer", vaccination.VaccineID);
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.VaccineDose), "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition", vaccination.AdverseID);


            SelectList gender = new SelectList(new[]
            {
                new {Value = "Male", Text = "Male"},
                new {Value = "Female", Text = "Female"}
            }, "Value", "Text", vaccination.Person.Gender);
            SelectList suffix = new SelectList(new[]
           {
                new {Value =String.Empty, Text ="NA"},
                new { Value = "II", Text = "II"},
                new { Value = "III", Text = "III"},
                new { Value = "IV", Text = "IV"},
                new { Value = "V", Text = "V"},
                new { Value = "SR.", Text = "SR."},
                new { Value = "JR.", Text = "JR."},
            }, "Value", "Text", vaccination.Person.Suffix);

            ViewBag.Suffix = suffix;
            ViewBag.Gender = gender;
            return View(vaccination);
        }

        private Person CheckPersonIfExisitng(Person person)
        {
            var personExist = db.Persons
                .Where(x => x.FirstName == person.FirstName)
                .Where(x => x.LastName == person.LastName)
                .Where(x => x.BirthDate == person.BirthDate)
                .Where(x => x.CityMunicipalityID == person.CityMunicipalityID)
                .FirstOrDefault();
            return personExist;
        }

        //create a fucntion that will take a nullable string firstname, middleName,lastname,city/municityid of person and return a list of person in json format
        public JsonResult GetPersons(string firstName, string middleName, string lastName, int? cityMunicipalityID)
        {
            var personsQuery = db.Persons.AsQueryable();

            // Check and apply filtering conditions for each parameter only if they are not null or empty
            if (!string.IsNullOrEmpty(firstName))
            {
                personsQuery = personsQuery.Where(p => p.FirstName.Equals(firstName));
            }

            if (!string.IsNullOrEmpty(middleName))
            {
                personsQuery = personsQuery.Where(p => p.MiddleName.Contains(middleName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                personsQuery = personsQuery.Where(p => p.LastName.Contains(lastName));
            }

            if (cityMunicipalityID.HasValue)
            {
                personsQuery = personsQuery.Where(p => p.CityMunicipalityID == cityMunicipalityID);
            }

            var persons = personsQuery.ToList();
            return Json(persons, JsonRequestBehavior.AllowGet);
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
