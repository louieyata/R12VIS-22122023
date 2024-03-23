using R12VIS.Models;
using R12VIS.Models.CustomModels;
using R12VIS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using People = R12VIS.Models.People;

namespace R12VIS.Controllers
{

    [UserAuthenticationFilter]

    public class PeopleController : Controller
    {
        private DbContextR12 db = new DbContextR12();
        DbR12VISEntities _db = new DbR12VISEntities();

        // GET: People
        public ActionResult Index()
        {
            var ethnic_group_list = db.EthnicGroups.Select(a => new { a.Id, a.IndigenousMember });
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            var suffixes = _db.Suffixes.ToList();
            var religions = db.Religions.ToList();
            var educational_attainment_list = db.EducationalAttainment.ToList();
            var income_class = db.IncomeClass.ToList();
            var occupation = db.Occupation.ToList();

            SelectList siblingRank = new SelectList(new[]
            {
                new { Value = 1, Text = "First Born Child"},
                new { Value = 2, Text = "Second Born Child"},
                new { Value = 3, Text = "Third Born Child"},
                new { Value = 4, Text = "Fourth Born Child"},
                new { Value = 5, Text = "Fifth Born Child"},
                new { Value = 6, Text = "Sixth Born Child"},
                new { Value = 7, Text = "Seventh Born Child"},
            }, "value", "text", 1);

            ViewBag.SiblingRankList = siblingRank;
            ViewBag.Province = new SelectList(provinces, "value", "text");
            ViewBag.EthnicGroupList = new SelectList(ethnic_group_list, "Id", "IndigenousMember");
            ViewBag.SuffixList = new SelectList(suffixes, "id", "suffix_description", 1);
            ViewBag.ReligionList = new SelectList(religions, "ID", "Description");
            ViewBag.EducationalAttainmentList = new SelectList(educational_attainment_list, "ID", "Description");
            ViewBag.IncomeClassList = new SelectList(income_class, "ID", "Description");
            ViewBag.OccupationList = new SelectList(occupation, "ID", "Description");

            return View();
        }

        // GET: People/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var person = db.Persons.Where(a => a.ID == id).FirstOrDefault();
        //    if (person == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    _PersonProfile _PersonProfile = new _PersonProfile()
        //    {
        //        Person = person,
        //        Vaccinations = db.Vaccinations
        //       .Include(x => x.Person)
        //       .Include(x => x.Dose)
        //       .Include(x => x.Vaccine)
        //       .Include(x => x.Person.Province)
        //       .Include(x => x.Person.CityMunicipality)
        //       .Include(x => x.PriorityGroup)
        //       .Where(x => x.PersonID == person.ID)
        //       .ToList()
        //    };
        //    return View(_PersonProfile);
        //}

        public ActionResult PersonalDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var person = db.Persons
                .Where(a => a.ID == id)
                .FirstOrDefault();

            if (person == null)
            {
                return HttpNotFound();
            }

            // Fetch unique categories based on PriorityGroup.Category
            var uniqueCategories = db.Vaccinations
               .Include(v => v.PriorityGroup)
               .Where(a => a.PersonID == id)
                .Select(v => v.PriorityGroup.Category)
                .Distinct();

            _PersonProfile _PersonProfile = new _PersonProfile()
            {
                Person = person,
                Categories = string.Join(", ", uniqueCategories),
                Vaccinations = db.Vaccinations
                       .Include(x => x.Person)
                       .Include(x => x.Dose)
                       .Include(x => x.Vaccine)
                       .Include(x => x.Person.Province)
                       .Include(x => x.Person.CityMunicipality)
                       .Include(x => x.Person.Barangay)
                       .Include(x => x.PriorityGroup)
                       .Include(x => x.Person.Religion)
                       .Include(x => x.Person.Occupation)
                       .Include(x => x.Person.EducationalAttainment)
                       .Include(x => x.Person.IncomeClass)
                       .Where(x => x.PersonID == person.ID)
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,UniquePersonID,FirstName,MiddleName,LastName,Suffix,ContactNumber,GuardianName,Gender,isPWD,EthnicGroupID,BirthDate,ProvinceID,CityMunicipalityID,BarangayID")] Person person)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Persons.Add(person);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.BarangayID = new SelectList(db.Barangays, "barangay_id", "barangay_name", person.BarangayID);
        //    ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", person.CityMunicipalityID);
        //    ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember", person.EthnicGroupID);
        //    ViewBag.ProvinceID = new SelectList(db.Provinces, "province_id", "province_name", person.ProvinceID);
        //    return View(person);
        //}

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var person = _db.People.Where(a => a.ID == id).FirstOrDefault();
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,UniquePersonID,FirstName,MiddleName,LastName,Suffix,ContactNumber,GuardianName,Gender,isPWD,EthnicGroupID,BirthDate,ProvinceID,CityMunicipalityID,BarangayID")] Person person)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(person).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.BarangayID = new SelectList(db.Barangays, "barangay_id", "barangay_name", person.BarangayID);
        //    ViewBag.CityMunicipalityID = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", person.CityMunicipalityID);
        //    ViewBag.EthnicGroupID = new SelectList(db.EthnicGroups, "Id", "IndigenousMember", person.EthnicGroupID);
        //    ViewBag.ProvinceID = new SelectList(db.Provinces, "province_id", "province_name", person.ProvinceID);
        //    return View(person);
        //}

        public JsonResult GetPerson(int person_id)
        {
            var jsonResult = Json(JsonRequestBehavior.AllowGet);
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var person = db.Persons.Where(a => a.ID == person_id).FirstOrDefault();
                if (person == null)
                {
                    jsonResult = Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    jsonResult = Json(new { data = person, isSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.People person = _db.People.Find(id);
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
            var person = _db.People.Where(a => a.ID == id).FirstOrDefault();
            _db.People.Remove(person);
            _db.SaveChanges();
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

        #region "Mark Updates"

        public JsonResult GetIndividual(int person_id)
        {
            try
            {
                var person = _db.People.Where(a => a.ID == person_id).Select(a => new
                {
                    isSuccess = true,
                    a.ID,
                    a.FirstName,
                    a.MiddleName,
                    a.LastName,
                    Suffix = a.Suffix.ToUpper() != null ? _db.Suffixes.Where(b => b.suffix_description == a.Suffix.ToUpper()).Select(b => b.id).FirstOrDefault() : 1,
                    a.GuardianName,
                    a.Gender,
                    a.EthnicGroupID,
                    a.UniquePersonID,
                    a.ContactNumber,
                    a.isPWD,
                    a.BirthDate,
                    a.ProvinceID,
                    a.CityMunicipalityID,
                    a.BarangayID,
                    a.SiblingRank,
                    a.ReligionID,
                    a.EducationalAttainmentID,
                    a.IncomeClassID,
                    a.OccupationID

                }).FirstOrDefault();

                var jsonResult = Json(person, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                var jsonResult = Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SaveIndividual(ModelIndividual individual, bool isNew)
        {
            var jsonResult = Json(JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            try
            {
                People person = new People();

                if (!isNew)
                {
                    int id = individual.ID;
                    person = _db.People.Where(a => a.ID == id).FirstOrDefault();
                }
                if (individual.GuardianName != "N/A" && individual.GuardianName != null && individual.GuardianName != "")
                    person.GuardianName = individual.GuardianName.ToUpper();
                if (individual.EthnicGroupID != 0 && individual.EthnicGroupID != null)
                    person.EthnicGroupID = individual.EthnicGroupID;

                person.Suffix = individual.Suffix.ToString().ToUpper();
                person.UniquePersonID = individual.UniquePersonID.ToString().ToUpper();
                person.FirstName = individual.FirstName.ToString().ToUpper();
                person.MiddleName = individual.MiddleName.ToString().ToUpper() == "N/A" ? "NONE" : individual.MiddleName.ToString().ToUpper();
                person.LastName = individual.LastName.ToString().ToUpper();
                person.ContactNumber = individual.ContactNumber.ToString().ToUpper();
                person.Gender = individual.Gender;
                person.isPWD = individual.isPWD;
                person.BirthDate = individual.BirthDate;
                person.ProvinceID = individual.ProvinceID;
                person.CityMunicipalityID = individual.CityMunicipalityID;
                person.BarangayID = individual.BarangayID;

                person.SiblingRank = individual.SiblingRank;
                person.ReligionID = individual.Religion;
                person.EducationalAttainmentID = individual.EducationalAttainment;
                person.IncomeClassID = individual.IncomeClass;
                person.OccupationID = individual.Occupation;


                if (isNew)
                {
                    _db.People.Add(person);
                }

                _db.SaveChanges();

                jsonResult = Json(new { isSuccess = true }, JsonRequestBehavior.DenyGet);

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                jsonResult = Json(new { isSuccess = false, message = exceptionMessage }, JsonRequestBehavior.DenyGet);
            }


            return jsonResult;
        }

        public JsonResult PeopleDataTable()
        {
            try
            {
                // Your code to retrieve data
                var persons = db.Persons.Select(x => new
                {
                    x.ID,
                    x.LastName,
                    x.FirstName,
                    Address = x.Barangay.barangay_name.ToUpper() + ", " + x.CityMunicipality.CityMunicipalityName.ToUpper() + ", " + x.Province.province_name.ToUpper(),
                    Gender = x.Gender.ToUpper()
                })
                .ToList();

                // Create a success response with the data
                var successResponse = new { data = persons };
                var jsonResult = Json(successResponse, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                // Create an error response with the error message
                var errorResponse = new { error = ex.Message };
                var jsonResult = Json(errorResponse, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

        //public JsonResult SaveIndividual(string jsonData, bool isNew)
        //{
        //    var jsonResult = Json(new { });
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    try
        //    {
        //        dynamic data = JsonConvert.DeserializeObject(jsonData);

        //        People person = new People();

        //        if (!isNew)
        //        {
        //            int id = (int)data["hidPersonId"];
        //            person = _db.People.Where(a => a.ID == id).FirstOrDefault();
        //        }

        //        person.UniquePersonID = data["individual_unique_person_id"].ToString();
        //        person.FirstName = data["individual_first_name"].ToString();
        //        if (data["individual_middle_name"] != null && !string.IsNullOrEmpty(data["individual_middle_name"].ToString()))
        //            person.MiddleName = data["individual_middle_name"].ToString();
        //        else
        //            person.MiddleName = "NONE";
        //        person.LastName = data["individual_last_name"].ToString();
        //        person.Suffix = string.IsNullOrEmpty(data["individual_suffix"]) ? data["individual_suffix"].ToString() : null;
        //        person.ContactNumber = data["individual_contact_number"].ToString();
        //        person.GuardianName = string.IsNullOrEmpty(data["individual_guardian_name"]) ? data["individual_guardian_name"].ToString() : null;
        //        person.Gender = data["radio-sex"]?.ToString();
        //        person.isPWD = data["pwd-status"];
        //        person.EthnicGroupID = (int)data["ethnic_group_list"];
        //        person.BirthDate = (DateTime)data["individual_date_of_birth"];
        //        person.ProvinceID = (int)data["province_id"];
        //        person.CityMunicipalityID = (int)data["cm_id"];
        //        person.BarangayID = (int)data["barangay_id"];

        //        if (isNew)
        //        {
        //            _db.People.Add(person);
        //        }

        //        db.SaveChanges();

        //        jsonResult = Json(new { isSuccess = true }, JsonRequestBehavior.DenyGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        jsonResult = Json(new { isSuccess = false, message = ex.Message}, JsonRequestBehavior.DenyGet);
        //    }

        //    return jsonResult;
        //}

        #endregion "Mark Updates"
    }
}
