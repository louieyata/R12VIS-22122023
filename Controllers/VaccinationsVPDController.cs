using Newtonsoft.Json;
using R12VIS.Models;
using R12VIS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    [UserAuthenticationFilter]
    public class VaccinationsVPDController : Controller
    {
        DbContextR12 db = new DbContextR12();
        DbMRSpecialVacsEntities vpddb = new DbMRSpecialVacsEntities();
        DbR12VISEntities visdb = new DbR12VISEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PreVaccinationVPD()
        {
            var ethnic_group_list = db.EthnicGroups.Select(a => new { a.Id, a.IndigenousMember });
            var regions = db.Regions.Select(a => new { value = a.ID, text = a.RegionName }).ToList();
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            var suffixes = visdb.Suffixes.ToList();
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
            }, "value", "text");

            ViewBag.SiblingRankList = siblingRank;
            ViewBag.Region = new SelectList(regions, "value", "text", 1);
            ViewBag.Province = new SelectList(provinces, "value", "text");
            ViewBag.EthnicGroupList = new SelectList(ethnic_group_list, "Id", "IndigenousMember", 0);
            ViewBag.SuffixList = new SelectList(suffixes, "id", "suffix_description", 0);
            ViewBag.ReligionList = new SelectList(religions, "ID", "Description");
            ViewBag.EducationalAttainmentList = new SelectList(educational_attainment_list, "ID", "Description");
            ViewBag.IncomeClassList = new SelectList(income_class, "ID", "Description");
            ViewBag.OccupationList = new SelectList(occupation, "ID", "Description");

            return View();
        }

        public JsonResult GetVPDPerson(int person_id)
        {
            var jsonResult = Json(JsonRequestBehavior.AllowGet);
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var person = vpddb.Person_VPD.Where(a => a.person_id == person_id).Select(a => new
                {
                    a.person_id,
                    a.person_ethnic_group_id,
                    a.person_first_name,
                    a.person_middle_name,
                    a.person_last_name,
                    a.person_suffix,
                    a.person_sex,
                    a.person_address,
                    a.person_region_address,
                    a.person_province_address,
                    a.person_city_municipality_address,
                    a.person_barangay_address,
                    a.person_religion_id,
                    a.person_parent_occupation_id,
                    a.person_parent_educational_attainment_id,
                    a.person_parent_income_class_id,
                    a.person_sibling_rank,
                    a.person_birth_date,
                    a.person_is_pwd
                }).FirstOrDefault();

                if (person == null)
                {
                    jsonResult = Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    jsonResult = Json(new { person = person, isSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Get Vaccinations Available
        public JsonResult GetVaccinesAvailable(DateTime date_of_birth, DateTime vaccination_date, int vaccination_type_id, int person_id = 0)
        {
            var jsonResult = Json(JsonRequestBehavior.AllowGet);
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var number_of_days = CalculateDays(date_of_birth, vaccination_date);
                List<int> vaccineIdsToRetrieve = new List<int>();

                if (person_id != 0)
                {
                    //var person = vpddb.Person_MRSIA.Where(a => a.person_id == person_id).FirstOrDefault();
                    var vaccine_record = vpddb.Vaccination_VPD.Where(a => a.vaccination_person_id == person_id).ToList();
                    var last_vaccination_date = new DateTime();
                    int number_of_days_from_vaccination = 0;
                    if (vaccination_type_id == 1)
                    {
                        if (number_of_days <= 1795)
                        {
                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 6))
                            {
                                last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 6).Select(a => a.vaccination_date).FirstOrDefault();
                                number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                if (vaccine_record.Any(v => v.vaccination_vaccine_id == 7))
                                {
                                    last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 7).Select(a => a.vaccination_date).FirstOrDefault();
                                    number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                    if (!vaccine_record.Any(v => v.vaccination_vaccine_id == 8))
                                        if (number_of_days_from_vaccination >= 30)
                                            vaccineIdsToRetrieve.Add(8);
                                }
                                else
                                {
                                    if (number_of_days_from_vaccination >= 30)
                                        vaccineIdsToRetrieve.Add(7);
                                }
                            }
                            else
                                vaccineIdsToRetrieve.Add(6);
                        }
                        if (number_of_days >= 274 && number_of_days <= 1795)
                        {
                            last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 14).Select(a => a.vaccination_date).FirstOrDefault();
                            number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 14))
                            {
                                if (number_of_days_from_vaccination >= 90)
                                    vaccineIdsToRetrieve.Add(15);
                            }
                            else
                                vaccineIdsToRetrieve.Add(14);
                        }

                    }
                    if (vaccination_type_id == 2)
                    {
                        if (number_of_days <= 7 && !vaccine_record.Any(v => v.vaccination_vaccine_id == 1))
                            vaccineIdsToRetrieve.Add(1);
                        if (number_of_days <= 365 && !vaccine_record.Any(v => v.vaccination_vaccine_id == 2))
                            vaccineIdsToRetrieve.Add(2);
                        if (number_of_days >= 45)
                        {
                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 3))
                            {
                                last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 3).Select(a => a.vaccination_date).FirstOrDefault();
                                number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                if (vaccine_record.Any(v => v.vaccination_vaccine_id == 4))
                                {
                                    last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 4).Select(a => a.vaccination_date).FirstOrDefault();
                                    number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                    if (!vaccine_record.Any(v => v.vaccination_vaccine_id == 5))
                                        if (number_of_days_from_vaccination >= 30)
                                            vaccineIdsToRetrieve.Add(5);
                                }
                                else
                                {
                                    if (number_of_days_from_vaccination >= 30)
                                        vaccineIdsToRetrieve.Add(4);
                                }
                            }
                            else
                                vaccineIdsToRetrieve.Add(3);

                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 6))
                            {
                                last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 6).Select(a => a.vaccination_date).FirstOrDefault();
                                number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                if (vaccine_record.Any(v => v.vaccination_vaccine_id == 7))
                                {
                                    last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 7).Select(a => a.vaccination_date).FirstOrDefault();
                                    number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                    if (!vaccine_record.Any(v => v.vaccination_vaccine_id == 8))
                                        if (number_of_days_from_vaccination >= 30)
                                            vaccineIdsToRetrieve.Add(8);
                                }
                                else
                                {
                                    if (number_of_days_from_vaccination >= 30)
                                        vaccineIdsToRetrieve.Add(7);
                                }
                            }
                            else
                                vaccineIdsToRetrieve.Add(6);
                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 11))
                            {
                                last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 11).Select(a => a.vaccination_date).FirstOrDefault();
                                number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                if (vaccine_record.Any(v => v.vaccination_vaccine_id == 12))
                                {
                                    last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 12).Select(a => a.vaccination_date).FirstOrDefault();
                                    number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                                    if (!vaccine_record.Any(v => v.vaccination_vaccine_id == 13))
                                        if (number_of_days_from_vaccination >= 30)
                                            vaccineIdsToRetrieve.Add(13);
                                }
                                else
                                {
                                    if (number_of_days_from_vaccination >= 30)
                                        vaccineIdsToRetrieve.Add(12);
                                }
                            }
                            else
                                vaccineIdsToRetrieve.Add(11);
                        }

                        if (number_of_days >= 105)
                        {
                            last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 9).Select(a => a.vaccination_date).FirstOrDefault();
                            number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 9))
                            {
                                if (number_of_days_from_vaccination >= 165)
                                    vaccineIdsToRetrieve.Add(10);
                            }
                            else
                                vaccineIdsToRetrieve.Add(9);
                        }

                        if (number_of_days >= 270)
                        {
                            last_vaccination_date = vaccine_record.Where(a => a.vaccination_vaccine_id == 14).Select(a => a.vaccination_date).FirstOrDefault();
                            number_of_days_from_vaccination = CalculateDays(last_vaccination_date, vaccination_date);
                            if (vaccine_record.Any(v => v.vaccination_vaccine_id == 14))
                            {
                                if (number_of_days_from_vaccination >= 90)
                                    vaccineIdsToRetrieve.Add(15);
                            }
                            else
                                vaccineIdsToRetrieve.Add(14);
                        }
                    }
                }
                else
                {
                    if (vaccination_type_id == 1)
                    {
                        vaccineIdsToRetrieve.Add(16);
                        vaccineIdsToRetrieve.Add(17);
                        vaccineIdsToRetrieve.Add(18);
                    }
                    if (vaccination_type_id == 2)
                    {
                        if (number_of_days <= 7)
                            vaccineIdsToRetrieve.Add(1);
                        if (number_of_days <= 365)
                            vaccineIdsToRetrieve.Add(2);
                        if (number_of_days >= 45)
                        {
                            vaccineIdsToRetrieve.Add(3);
                            vaccineIdsToRetrieve.Add(6);
                            vaccineIdsToRetrieve.Add(11);
                        }
                        if (number_of_days >= 105)
                            vaccineIdsToRetrieve.Add(9);
                        if (number_of_days >= 270)
                            vaccineIdsToRetrieve.Add(14);
                    }
                }
                var vaccines = vpddb.Vaccine_VPD
                    .Where(v => vaccineIdsToRetrieve
                    .Contains(v.vaccine_id))
                    .Select(a => new
                    {
                        a.vaccine_id,
                        a.vaccine_description
                    }).ToList();

                jsonResult = Json(new { isSuccess = true, data = vaccines }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static int CalculateDays(DateTime date_of_birth, DateTime vaccination_date)
        {
            // Calculate the difference in days
            TimeSpan difference = vaccination_date - date_of_birth;

            return difference.Days;
        }

        #endregion Get Vaccinations Available

        #region enhanced getting of available Vaccines

        //public JsonResult GetVaccinesAvailable(DateTime date_of_birth, DateTime vaccination_date, int person_id = 0)
        //{
        //    var jsonResult = Json(JsonRequestBehavior.AllowGet);
        //    db.Configuration.ProxyCreationEnabled = false;

        //    try
        //    {
        //        var number_of_days = CalculateDays(date_of_birth, vaccination_date);
        //        List<int> vaccineIdsToRetrieve = new List<int>();

        //        if (person_id != 0)
        //        {
        //            RetrieveVaccineIdsBasedOnPerson(number_of_days, person_id, vaccineIdsToRetrieve, vaccination_date);
        //        }
        //        else
        //        {
        //            RetrieveVaccineIdsWithoutPerson(number_of_days, vaccineIdsToRetrieve);
        //        }

        //        var vaccines = vpddb.Vaccine_MRSIA
        //            .Where(v => vaccineIdsToRetrieve.Contains(v.vaccine_id))
        //            .Select(a => new
        //            {
        //                a.vaccine_id,
        //                a.vaccine_description
        //            }).ToList();

        //        jsonResult = Json(new { isSuccess = true, data = vaccines }, JsonRequestBehavior.AllowGet);
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private void RetrieveVaccineIdsBasedOnPerson(int number_of_days, int person_id, List<int> vaccineIdsToRetrieve, DateTime vaccination_date)
        //{
        //    var vaccine_record = vpddb.Vaccination_MRSIA.Where(a => a.vaccination_person_id == person_id).ToList();
        //    DateTime last_vaccination_date = new DateTime();
        //    int number_of_days_from_vaccination = 0;

        //    // Add conditions based on vaccination record
        //    if (number_of_days <= 7 && !VaccineRecordContains(vaccine_record, 1))
        //        vaccineIdsToRetrieve.Add(1);

        //    // Add more conditions as needed...

        //    // Add conditions based on number_of_days
        //    if (number_of_days >= 105)
        //        AddVaccineIfEligible(vaccine_record, 9, 10, 165, vaccineIdsToRetrieve, vaccination_date);

        //    if (number_of_days >= 270)
        //        AddVaccineIfEligible(vaccine_record, 14, 15, 90, vaccineIdsToRetrieve, vaccination_date);
        //}
        //private void RetrieveVaccineIdsWithoutPerson(int number_of_days, List<int> vaccineIdsToRetrieve)
        //{
        //    // Add conditions based on number_of_days
        //    if (number_of_days <= 7)
        //        vaccineIdsToRetrieve.Add(1);

        //    // Add more conditions as needed...

        //    if (number_of_days >= 105)
        //        vaccineIdsToRetrieve.Add(9);

        //    if (number_of_days >= 270)
        //        vaccineIdsToRetrieve.Add(14);
        //}
        //private void AddVaccineIfEligible(List<Vaccination_MRSIA> vaccine_record, int currentVaccineId, int nextVaccineId, int daysThreshold, List<int> vaccineIdsToRetrieve, DateTime vaccination_date)
        //{
        //    if (vaccine_record.Any(v => v.vaccination_vaccine_id == currentVaccineId))
        //    {
        //        var lastVaccinationDate = vaccine_record.Where(a => a.vaccination_vaccine_id == currentVaccineId).Select(a => a.vaccination_date).FirstOrDefault();
        //        var daysFromLastVaccination = CalculateDays(lastVaccinationDate, vaccination_date);

        //        if (!vaccine_record.Any(v => v.vaccination_vaccine_id == nextVaccineId) && daysFromLastVaccination >= daysThreshold)
        //            vaccineIdsToRetrieve.Add(nextVaccineId);
        //    }
        //    else
        //    {
        //        vaccineIdsToRetrieve.Add(currentVaccineId);
        //    }
        //}
        //private bool VaccineRecordContains(List<Vaccination_MRSIA> vaccine_record, int vaccineId)
        //{
        //    return vaccine_record.Any(v => v.vaccination_vaccine_id == vaccineId);
        //}

        #endregion enhanced getting of available Vaccines

        public JsonResult SaveVaccination(string jsonData)
        {
            var jsonResult = Json(new { });
            jsonResult.MaxJsonLength = int.MaxValue;

            try
            {
                dynamic data = JsonConvert.DeserializeObject(jsonData);

                string firstName = data["individual_first_name"];
                string lastName = data["individual_last_name"];
                string suffix = string.IsNullOrWhiteSpace(data["individual_suffix"]?.ToString()) ? "N/A" : data["individual_suffix"].ToString();
                bool male = data["individual_sex_male"];
                string sexValue = GetSexValue(male);
                bool mrsia = data["vaccination_mrsia"];
                int vaccinationTypeValue = GetVaccinationTypeValue(mrsia);
                int ethnicGroupId;
                DateTime vaccinationDate;
                DateTime.TryParse(data["vaccination_date_of_vaccination"]?.ToString(), out vaccinationDate);
                DateTime birthDate;
                DateTime.TryParse(data["individual_date_of_birth"]?.ToString(), out birthDate);

                if (data["individual_date_of_birth"] > data["vaccination_date_of_vaccination"])
                {
                    jsonResult = Json(new { isSuccess = false, message = "Error: Vaccination date cannote be before the date of birth." }, JsonRequestBehavior.DenyGet);
                    return jsonResult;
                }
                if (DateTime.TryParse(data["vaccination_date_of_vaccination"]?.ToString(), out vaccinationDate))
                {
                    Person_VPD person = new Person_VPD();

                    person.person_ethnic_group_id = int.TryParse(data["ethnic_group_list"]?.ToString(), out ethnicGroupId) ? ethnicGroupId : 0;
                    person.person_suffix = suffix;
                    person.person_middle_name = string.IsNullOrEmpty(data["individual_middle_name"]?.ToString()) ? "NONE" : data["individual_middle_name"].ToString().ToUpper();

                    // Not nullable values
                    person.person_sex = sexValue;
                    person.person_first_name = data["individual_first_name"].ToString().ToUpper();
                    person.person_last_name = data["individual_last_name"].ToString().ToUpper();
                    person.person_is_pwd = data.ContainsKey("individual_pwd") && (bool)data["individual_pwd"];
                    person.person_sibling_rank = data["sibling_rank_list"];
                    person.person_region_address = data["region_id"];
                    person.person_province_address = data["province_id"];
                    person.person_city_municipality_address = data["cm_id"];
                    person.person_barangay_address = data["barangay_id"];
                    person.person_religion_id = data["religion_list"];
                    person.person_parent_educational_attainment_id = data["educational_attainment_list"];
                    person.person_parent_income_class_id = data["income_class_list"];
                    person.person_parent_occupation_id = data["mrsia_occupation_list"];
                    person.person_birth_date = birthDate;

                    Person_VPD existingPerson = vpddb.Person_VPD
                        .Where(a =>
                        a.person_first_name == firstName &&
                        a.person_last_name == lastName &&
                        a.person_sex == sexValue &&
                        (string.IsNullOrEmpty(suffix) || (a.person_suffix != "N/A" && a.person_suffix != "NONE" && a.person_suffix == suffix)) &&
                        a.person_birth_date == birthDate)
                        .FirstOrDefault();

                    if (existingPerson == null)
                    {
                        vpddb.Person_VPD.Add(person);
                        vpddb.SaveChanges(); // Save changes to the database
                    }
                    else person = existingPerson;

                    if (IsDuplicateVaccination(person.person_id, (int)data["vaccination_vaccine_id"]))
                    {
                        jsonResult = Json(new { isSuccess = false, message = "Vaccination data already exists" }, JsonRequestBehavior.DenyGet);
                        return jsonResult;
                    }

                    Vaccination_VPD vaccination = new Vaccination_VPD();

                    vaccination.vaccination_person_id = person.person_id;
                    vaccination.vaccination_region_id = data["vaccination_region_id"];
                    vaccination.vaccination_province_id = data["vaccination_province_id"];
                    vaccination.vaccination_city_municipality_id = data["vaccination_cm_id"];
                    vaccination.vaccination_barangay_id = data["vaccination_barangay_id"];
                    vaccination.vaccination_vaccinator = data["vaccination_vaccinator_name"];
                    vaccination.vaccination_vaccine_id = data["vaccination_vaccine_id"];
                    vaccination.vaccination_date = vaccinationDate;

                    // Vaccination type 1 = MRSIA, 2 = ROUTINE
                    vaccination.vaccination_type_id = vaccinationTypeValue;

                    vpddb.Vaccination_VPD.Add(vaccination);
                    vpddb.SaveChanges();

                    jsonResult = Json(new { isSuccess = true, birthdate = birthDate, vaccination_date = vaccinationDate, message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                jsonResult = Json(new { isSuccess = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            return jsonResult;
        }
        private string GetSexValue(bool male)
        {
            return (male) ? "m" : "f";
        }
        private int GetVaccinationTypeValue(bool mrsia)
        {

            return (mrsia) ? 1 : 2;
        }

        private bool IsDuplicateVaccination(int personId, int vaccineId)
        {
            return vpddb.Vaccination_VPD.Any(a => a.vaccination_person_id == personId && a.vaccination_vaccine_id == vaccineId);
        }
        public JsonResult VaccinationsDataTable()
        {
            try
            {
                var vaccinations = vpddb.Vaccination_VPD.Select(v => new
                {
                    v.vaccination_id,
                    Person = v.Person_VPD.person_last_name + ", " + v.Person_VPD.person_first_name +
                    (!string.IsNullOrEmpty(v.Person_VPD.person_middle_name) && v.Person_VPD.person_middle_name != "NONE" ? (" " + v.Person_VPD.person_middle_name) : ""),
                    Vaccine = v.Vaccine_VPD.vaccine_description,
                    VaccineDate = v.vaccination_date,
                    Vaccinator = v.vaccination_vaccinator
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
        public JsonResult PeopleDataTable()
        {
            try
            {
                var vaccinations = vpddb.Person_VPD.Select(v => new
                {
                    v.person_id,
                    v.person_first_name,
                    v.person_last_name,
                    v.person_birth_date,
                    person_sex = v.person_sex.ToString().ToUpper()
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