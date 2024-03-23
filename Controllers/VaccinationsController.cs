using R12VIS.Models;
using R12VIS.Models.ViewModel;
using System;
using System.Collections.Generic;
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

            return View();

            //var vaccinations = db.Vaccinations.Select(v => new {
            //    v.Adverse,
            //    v.Deferral,
            //    v.Dose,
            //    v.Person,
            //    v.PriorityGroup,
            //    v.Vaccine,
            //    v.Person.EthnicGroup,
            //    v.Person.Province,
            //    v.Person.CityMunicipality,
            //    v.Person.Barangay
            //}).ToList();
            //return View(vaccinations); /**/
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
            ViewBag.ProvinceID = new SelectList(db.Provinces.Where(a => a.RegionID == 1).OrderBy(p => p.province_name), "province_id", "province_name", vaccination.Person.ProvinceID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName), "city_municipality_id", "CityMunicipalityName", vaccination.Person.CityMunicipalityID);
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100), "barangay_id", "barangay_name", vaccination.Person.BarangayID);
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason", vaccination.DeferralID);
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer", vaccination.VaccineID);
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition", vaccination.AdverseID);
            ViewBag.Religion = new SelectList(db.Religions.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.ReligionID);
            ViewBag.EducationalAttainment = new SelectList(db.EducationalAttainment.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.EducationalAttainment);
            ViewBag.IncomeClass = new SelectList(db.IncomeClass.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.IncomeClass);
            ViewBag.Occupation = new SelectList(db.Occupation.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.Occupation);

            SelectList siblingRank = new SelectList(new[]
            {
                new { Value = 1, Text = "First Born Child"},
                new { Value = 2, Text = "Second Born Child"},
                new { Value = 3, Text = "Third Born Child"},
                new { Value = 4, Text = "Fourth Born Child"},
                new { Value = 5, Text = "Fifth Born Child"},
                new { Value = 6, Text = "Sixth Born Child"},
                new { Value = 7, Text = "Seventh Born Child"},
            }, "Value", "Text", vaccination.Person.SiblingRank);
            ViewBag.SiblingRank = siblingRank;

            SelectList gender = new SelectList(new[]
            {
                new {Value = "M", Text = "Male"},
                new {Value = "F", Text = "Female"}
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
            ViewBag.ProvinceID = new SelectList(db.Provinces.Where(a => a.RegionID == 1).OrderBy(p => p.province_name), "province_id", "province_name", vaccination.Person.ProvinceID);
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName), "city_municipality_id", "CityMunicipalityName", vaccination.Person.CityMunicipalityID);
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100), "barangay_id", "barangay_name", vaccination.Person.BarangayID);
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason", vaccination.DeferralID);
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer", vaccination.VaccineID);
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose", vaccination.DoseID);
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition", vaccination.AdverseID);
            ViewBag.Religion = new SelectList(db.Religions.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.ReligionID);
            ViewBag.EducationalAttainment = new SelectList(db.EducationalAttainment.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.EducationalAttainment);
            ViewBag.IncomeClass = new SelectList(db.IncomeClass.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.IncomeClass);
            ViewBag.Occupation = new SelectList(db.Occupation.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.Occupation);

            SelectList siblingRank = new SelectList(new[]
           {
                new { Value = 1, Text = "First Born Child"},
                new { Value = 2, Text = "Second Born Child"},
                new { Value = 3, Text = "Third Born Child"},
                new { Value = 4, Text = "Fourth Born Child"},
                new { Value = 5, Text = "Fifth Born Child"},
                new { Value = 6, Text = "Sixth Born Child"},
                new { Value = 7, Text = "Seventh Born Child"},
            }, "Value", "Text", vaccination.Person.SiblingRank);
            ViewBag.SiblingRank = siblingRank;

            SelectList gender = new SelectList(new[]
            {
                new {Value = "M", Text = "Male"},
                new {Value = "F", Text = "Female"}
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
                new {Value = "M", Text = "Male"},
                new {Value = "F", Text = "Female"}
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
            SelectList siblingRank = new SelectList(new[]
           {
                 new { Value = 1, Text = "First Born Child"},
                new { Value = 2, Text = "Second Born Child"},
                new { Value = 3, Text = "Third Born Child"},
                new { Value = 4, Text = "Fourth Born Child"},
                new { Value = 5, Text = "Fifth Born Child"},
                new { Value = 6, Text = "Sixth Born Child"},
                new { Value = 7, Text = "Seventh Born Child"},
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
                    ViewBag.ProvinceID = new SelectList(db.Provinces.Where(a => a.RegionID == 1).OrderBy(p => p.province_name).Where(x => x.province_id == person.ProvinceID), "province_id", "province_name");
                    ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName).Where(x => x.city_municipality_id == person.CityMunicipalityID), "city_municipality_id", "CityMunicipalityName");
                    ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(100).Where(x => x.barangay_id == person.BarangayID), "barangay_id", "barangay_name");
                    ViewBag.Gender = new SelectList(gender.Where(x => x.Value.ToLower() == person.Gender.ToLower()), "Value", "Text");
                    ViewBag.Suffix = new SelectList(suffix.Where(x => x.Value.ToLower() == person.Suffix?.ToLower()), "Value", "Text");
                    ViewBag.SiblingRank = new SelectList(siblingRank.Where(x => x.Value == person.SiblingRank.ToString()), "Value", "Text");
                    ViewBag.Religion = new SelectList(db.Religions.Where(a => a.ID == person.ReligionID), "ID", "Description"); ;
                    ViewBag.EducationalAttainment = new SelectList(db.EducationalAttainment.Where(a => a.ID == person.EducationalAttainmentID), "ID", "Description"); ;
                    ViewBag.IncomeClass = new SelectList(db.IncomeClass.Where(a => a.ID == person.IncomeClassID), "ID", "Description"); ;
                    ViewBag.Occupation = new SelectList(db.Occupation.Where(a => a.ID == person.OccupationID), "ID", "Description"); ;

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
            ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups.OrderBy(p => p.IndigenousMember), "Id", "IndigenousMember");
            ViewBag.ProvinceID = new SelectList(db.Provinces.Where(a => a.RegionID == 1).OrderBy(p => p.province_name), "province_id", "province_name");
            ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities.OrderBy(p => p.CityMunicipalityName).Take(1), "city_municipality_id", "CityMunicipalityName");
            ViewBag.BarangayID = new SelectList(db.Barangays.OrderBy(p => p.barangay_name).Take(1), "barangay_id", "barangay_name");
            ViewBag.DeferralID = new SelectList(db.Deferrals.OrderBy(p => p.Reason).Take(100), "Id", "Reason");
            ViewBag.VaccineID = new SelectList(db.Vaccines.OrderBy(p => p.VaccineManufacturer), "ID", "VaccineManufacturer");
            ViewBag.DoseID = new SelectList(db.Dose.OrderBy(p => p.ID), "ID", "VaccineDose");
            ViewBag.AdverseID = new SelectList(db.Adverses.OrderBy(p => p.ID), "ID", "Condition");
            ViewBag.Gender = gender;
            ViewBag.Suffix = suffix;
            ViewBag.SiblingRank = siblingRank;
            ViewBag.Religion = new SelectList(db.Religions.OrderBy(p => p.ID), "ID", "Description");
            ViewBag.EducationalAttainment = new SelectList(db.EducationalAttainment.OrderBy(p => p.ID), "ID", "Description");
            ViewBag.IncomeClass = new SelectList(db.IncomeClass.OrderBy(p => p.ID), "ID", "Description");
            ViewBag.Occupation = new SelectList(db.Occupation.OrderBy(p => p.ID), "ID", "Description");

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
                var isPersonHasTheSameVaccination = CheckVaccinationDuplicate(vaccination);
                if (isPersonHasTheSameVaccination) //Toaster if person has the same vaccination dose
                {
                    TempData["ToastMessage"] = vaccination.Person.FirstName + " " + vaccination.Person.LastName + " is already vaccinated with selected Dose";
                    TempData["ToastClass"] = "text-bg-warning";
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
            ViewBag.Religion = new SelectList(db.Religions.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.ReligionID);
            ViewBag.EducationalAttainment = new SelectList(db.EducationalAttainment.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.EducationalAttainment);
            ViewBag.IncomeClass = new SelectList(db.IncomeClass.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.IncomeClass);
            ViewBag.Occupation = new SelectList(db.Occupation.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.Occupation);

            SelectList siblingRank = new SelectList(new[]
            {
                new { Value = 1, Text = "First Born Child"},
                new { Value = 2, Text = "Second Born Child"},
                new { Value = 3, Text = "Third Born Child"},
                new { Value = 4, Text = "Fourth Born Child"},
                new { Value = 5, Text = "Fifth Born Child"},
                new { Value = 6, Text = "Sixth Born Child"},
                new { Value = 7, Text = "Seventh Born Child"},
                }, "Value", "Text", vaccination.Person.SiblingRank);
            ViewBag.SiblingRank = siblingRank;

            SelectList gender = new SelectList(new[]
            {
                new {Value = "M", Text = "Male"},
                new {Value = "F", Text = "Female"}
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


        // MARK MODIFIED
        //[HttpPost]
        //public ActionResult PreVaccination(Vaccination vaccination)
        //{
        //    User user = Session["User"] as User;

        //    if (user == null)
        //    {
        //        return RedirectToAction("Login", "Users");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        vaccination.VaccinatorName = vaccination.VaccinatorName?.ToUpper();
        //        vaccination.Comorbidity = vaccination.Comorbidity?.ToUpper();
        //        vaccination.CreatedBy = $"{user.FirstName} {user.LastName}";
        //        vaccination.UserID = user.ID;

        //        // Check if the person already exists
        //        Person existingPerson = db.Persons.FirstOrDefault(p =>
        //        p.FirstName == vaccination.Person.FirstName && p.LastName == vaccination.Person.LastName && p.BirthDate == vaccination.Person.BirthDate);

        //        if (existingPerson != null)
        //        {
        //            // Update the existing person
        //            existingPerson.FirstName = vaccination.Person.FirstName?.ToUpper();
        //            existingPerson.MiddleName = vaccination.Person.MiddleName?.ToUpper();
        //            existingPerson.LastName = vaccination.Person.LastName?.ToUpper();
        //            existingPerson.Suffix = vaccination.Person.Suffix;
        //            existingPerson.SiblingRank = vaccination.Person.SiblingRank; ;
        //            existingPerson.ReligionID = vaccination.Person.ReligionID; ;
        //            existingPerson.EducationalAttainmentID = vaccination.Person.EducationalAttainmentID; ;
        //            existingPerson.IncomeClassID = vaccination.Person.IncomeClassID; ;
        //            existingPerson.OccupationID = vaccination.Person.OccupationID; ;

        //            // Update other properties as needed

        //            db.Entry(existingPerson).State = EntityState.Modified;
        //            db.SaveChanges();

        //            vaccination.Person = existingPerson;
        //        }
        //        else
        //        {
        //            // Create a new person
        //            vaccination.Person.FirstName = vaccination.Person.FirstName?.ToUpper();
        //            vaccination.Person.MiddleName = vaccination.Person.MiddleName?.ToUpper();
        //            vaccination.Person.LastName = vaccination.Person.LastName?.ToUpper();
        //            vaccination.Person.Suffix = vaccination.Person.Suffix;
        //            vaccination.Person.SiblingRank = vaccination.Person.SiblingRank; ;
        //            vaccination.Person.ReligionID = vaccination.Person.ReligionID; ;
        //            vaccination.Person.EducationalAttainmentID = vaccination.Person.EducationalAttainmentID; ;
        //            vaccination.Person.IncomeClassID = vaccination.Person.IncomeClassID; ;
        //            vaccination.Person.OccupationID = vaccination.Person.OccupationID; ;
        //            // Set other properties for a new person

        //            db.Persons.Add(vaccination.Person);
        //            db.SaveChanges();
        //        }

        //        var isPersonHasTheSameVaccination = CheckVaccinationDuplicate(vaccination);

        //        if (isPersonHasTheSameVaccination)
        //        {
        //            TempData["ToastMessage"] = $"{vaccination.Person.FirstName} {vaccination.Person.LastName} is already vaccinated with the selected Dose";
        //            TempData["ToastClass"] = "text-bg-warning";
        //        }
        //        else
        //        {

        //            vaccination.Person.FirstName = vaccination.Person.FirstName?.ToUpper();
        //            vaccination.Person.MiddleName = vaccination.Person.MiddleName?.ToUpper();
        //            vaccination.Person.LastName = vaccination.Person.LastName?.ToUpper();
        //            db.Vaccinations.Add(vaccination);
        //            db.SaveChanges();

        //            return RedirectToAction("Index");
        //        }
        //    }

        //    // Populate ViewBag and return the view
        //    PopulateDropdowns(vaccination);
        //    return View(vaccination);
        //}
        private void PopulateDropdowns(Vaccination vaccination)
        {
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
            ViewBag.Religion = new SelectList(db.Religions.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.ReligionID);
            ViewBag.EducationalAttainment = new SelectList(db.EducationalAttainment.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.EducationalAttainment);
            ViewBag.IncomeClass = new SelectList(db.IncomeClass.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.IncomeClass);
            ViewBag.Occupation = new SelectList(db.Occupation.OrderBy(p => p.ID), "ID", "Description", vaccination.Person.Occupation);

            SelectList siblingRank = new SelectList(new[]
            {
                    new { Value = 1, Text = "First Born Child"},
                    new { Value = 2, Text = "Second Born Child"},
                    new { Value = 3, Text = "Third Born Child"},
                    new { Value = 4, Text = "Fourth Born Child"},
                    new { Value = 5, Text = "Fifth Born Child"},
                    new { Value = 6, Text = "Sixth Born Child"},
                    new { Value = 7, Text = "Seventh Born Child"},
                    }, "Value", "Text", vaccination.Person.SiblingRank);
            ViewBag.SiblingRank = siblingRank;

            SelectList gender = new SelectList(new[]
            {
                    new {Value = "M", Text = "Male"},
                    new {Value = "F", Text = "Female"}
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
            // Populate other ViewBag properties for dropdowns
        }
        private bool CheckVaccinationDuplicate(Vaccination vaccination)
        {

            var isAny = db.Vaccinations.Where(x => x.Person.ID == vaccination.Person.ID && x.DoseID == vaccination.DoseID).Any();
            return isAny;
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
        public JsonResult GetAllowedDose(int vaccineID)
        {
            var dose = new List<Dose>();
            if (vaccineID == 7)
            {
                dose = db.Dose.Where(p => p.ID != 1).ToList();
            }
            else
            {
                dose = db.Dose.ToList();
            }
            return Json(dose, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BlankPage()
        {
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult VaccinationsDataTable()
        {
            try
            {
                var vaccinations = db.Vaccinations.Select(v => new
                {
                    v.ID,
                    Person = v.Person.LastName + ", " + v.Person.FirstName + (v.Person.MiddleName != "" || v.Person.MiddleName != "NONE" || v.Person.MiddleName != null ? (" " + v.Person.MiddleName) : ""),
                    Vaccine = v.Vaccine.VaccineBrand,
                    Dose = v.Dose.VaccineDose,
                    Priority = v.PriorityGroup.Category,
                    Ethnic = v.Person.EthnicGroup.IndigenousMember
                }).ToList();

                var jsonResult = Json(new { data = vaccinations }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
