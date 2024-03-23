using Newtonsoft.Json;
using R12VIS.Models;
using R12VIS.Models.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    [UserAuthenticationFilter]

    public class VaccinationsCSController : Controller
    {
        DbContextR12 db = new DbContextR12();
        DbMRSpecialVacsEntities vpddb = new DbMRSpecialVacsEntities();
        DbR12VISEntities visdb = new DbR12VISEntities();
        DbSeniorCitizenVacsEntities scdb = new DbSeniorCitizenVacsEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PreVaccinationCS()
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
            ViewBag.Vaccines = new SelectList(scdb.Vaccine_SC.OrderBy(p => p.vaccine_description).ToList(), "vaccine_id", "vaccine_description");
            ViewBag.IncomeClassList = new SelectList(income_class, "ID", "Description");
            ViewBag.OccupationList = new SelectList(occupation, "ID", "Description");

            return View();
        }
        public JsonResult GetSCPerson(int person_id)
        {
            var jsonResult = Json(JsonRequestBehavior.AllowGet);
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var person = visdb.People.Where(a => a.ID == person_id).Select(a => new
                {
                    a.ID,
                    a.EthnicGroupID,
                    a.FirstName,
                    a.MiddleName,
                    a.LastName,
                    a.Suffix,
                    a.Gender,
                    a.ProvinceID,
                    a.CityMunicipalityID,
                    a.BarangayID,
                    a.ReligionID,
                    a.OccupationID,
                    a.EducationalAttainmentID,
                    a.IncomeClassID,
                    a.SiblingRank,
                    a.BirthDate,
                    a.isPWD
                }).FirstOrDefault();
                var senior_details = scdb.SeniorCitizen_details.Where(a => a.sc_person_id == person_id).Select(a => new
                {
                    a.sc_id,
                    a.sc_person_id,
                    a.sc_is_sc_member,
                    a.sc_sc_organization,
                    a.sc_previous_occupation,
                    a.sc_contact_person_name,
                    a.sc_contact_person_relationship,
                    a.sc_contact_person_contact_number,
                    a.sc_physician,
                    a.sc_physician_contact_number,
                    a.sc_living_arragement,
                    a.sc_blood_type,
                    a.sc_disablity,
                    a.sc_medical_alerts,
                    a.sc_poverty_reduction_member_id_number,
                    a.sc_is_pensioner,
                    a.sc_pensioner,
                    a.sc_philhealth_pin,
                    a.sc_sss_gsis_id_number
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

        public JsonResult SaveVaccination(string jsonData)
        {
            var jsonResult = Json(new { });
            jsonResult.MaxJsonLength = int.MaxValue;

            try
            {
                dynamic data = JsonConvert.DeserializeObject(jsonData);

                string firstName = data["individual_first_name"];
                string lastName = data["individual_last_name"];
                string suffix = string.IsNullOrWhiteSpace(data["individual_suffix"]?.ToString()) ? null : data["individual_suffix"].ToString();

                // Get sex value with a function
                bool male = data["individual_sex_male"];
                string sexValue = GetSexValue(male);

                // Get living arrangement with a function
                bool alone = data["individual_sc_living_arrangement_alone"];
                bool spouse = data["individual_sc_living_arrangement_spouse"];
                bool children = data["individual_sc_living_arrangement_children"];
                bool others = data["individual_sc_living_arrangement_others"];
                string others_name = data["individual_sc_living_arrangement_others_name"];
                string livingArrangementValue = GetLivingArrangementValue(alone, spouse, children, others, others_name);

                bool sss = data["individual_sc_pension_sss"];
                bool gsis = data["individual_sc_pension_gsis"];
                bool pvao = data["individual_sc_pension_pvao"];
                bool others_p = data["individual_sc_pension_others"];
                string others_p_name = data["individual_sc_pension_others_name"];
                string pensionProviderValue = GetPensionerValue(sss, gsis, pvao, others_p, others_p_name);

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
                    People person = new People();

                    person.EthnicGroupID = int.TryParse(data["ethnic_group_list"]?.ToString(), out ethnicGroupId) ? ethnicGroupId : (int?)null;
                    person.Suffix = suffix;
                    person.MiddleName = string.IsNullOrEmpty(data["individual_middle_name"]?.ToString()) ? "NONE" : data["individual_middle_name"].ToString().ToUpper();

                    // Not nullable values
                    person.Gender = sexValue;
                    person.FirstName = data["individual_first_name"].ToString().ToUpper();
                    person.LastName = data["individual_last_name"].ToString().ToUpper();
                    person.isPWD = data.ContainsKey("individual_pwd") && (bool)data["individual_pwd"];
                    person.SiblingRank = data["sibling_rank_list"];
                    person.ProvinceID = data["province_id"];
                    person.CityMunicipalityID = data["cm_id"];
                    person.BarangayID = data["barangay_id"];
                    person.ReligionID = data["religion_list"];
                    person.EducationalAttainmentID = data["educational_attainment_list"];
                    person.IncomeClassID = data["income_class_list"];
                    person.OccupationID = data["mrsia_occupation_list"];
                    person.BirthDate = birthDate;

                    People existingPerson = visdb.People
                        .Where(a =>
                        a.FirstName == firstName &&
                        a.LastName == lastName &&
                        a.Gender == sexValue &&
                        (string.IsNullOrEmpty(suffix) || (a.Suffix != "N/A" && a.Suffix != "NONE" && a.Suffix == suffix)) &&
                        a.BirthDate == birthDate)
                        .FirstOrDefault();

                    if (existingPerson == null)
                    {
                        visdb.People.Add(person);
                    }
                    else person = existingPerson;

                    SeniorCitizen_details scperson = new SeniorCitizen_details();

                    scperson.sc_is_sc_member = data.ContainsKey("individual_sc_member") && (bool)data["individual_sc_member"];
                    if ((bool)scperson.sc_is_sc_member)
                        scperson.sc_sc_organization = data["individual_sc_organization_name"];
                    scperson.sc_previous_occupation = data["individual_sc_previous_occupation"];
                    scperson.sc_contact_person_name = data["individual_sc_contact_person_name"];
                    scperson.sc_contact_person_relationship = data["individual_sc_contact_person_relationship"];
                    scperson.sc_contact_person_contact_number = data["individual_sc_contact_person_number"];
                    scperson.sc_physician = data["individual_sc_physician_name"];
                    scperson.sc_physician_contact_number = data["individual_sc_physician_contact_number"];
                    scperson.sc_living_arragement = livingArrangementValue;
                    scperson.sc_blood_type = data["individual_sc_blood_type"];
                    scperson.sc_disablity = data["individual_sc_disability"];
                    scperson.sc_poverty_reduction_member_id_number = data["individual_sc_poverty_reduction_id_number"];
                    scperson.sc_medical_alerts = data["individual_sc_medical_alerts"];
                    scperson.sc_is_pensioner = data["individual_sc_is_pensioner"];
                    if ((bool)scperson.sc_is_pensioner)
                        scperson.sc_pensioner = pensionProviderValue;
                    scperson.sc_philhealth_pin = data["individual_sc_philhealth_number"];
                    scperson.sc_sss_gsis_id_number = data["individual_sc_sss_gsis_number"];

                    SeniorCitizen_details existingSeniorCitizen = scdb.SeniorCitizen_details.Where(a => a.sc_person_id == person.ID).FirstOrDefault();

                    if (existingSeniorCitizen == null)
                    {
                        scdb.SeniorCitizen_details.Add(scperson);
                    }
                    else scperson = existingSeniorCitizen;

                    if (IsDuplicateVaccination(person.ID, (int)data["vaccination_vaccine_id"]))
                    {
                        jsonResult = Json(new { isSuccess = false, message = "Vaccine data already exists" }, JsonRequestBehavior.DenyGet);
                        return jsonResult;
                    }

                    Vaccination_SC vaccination = new Vaccination_SC();

                    vaccination.vaccination_person_id = person.ID;
                    vaccination.vaccination_region_id = data["vaccination_region_id"];
                    vaccination.vaccination_provcine_id = data["vaccination_province_id"];
                    vaccination.vaccination_city_municipality_id = data["vaccination_cm_id"];
                    vaccination.vaccination_barangay_id = data["vaccination_barangay_id"];
                    vaccination.vaccination_vaccinator = data["vaccination_vaccinator_name"];
                    vaccination.vaccination_vaccine_id = data["vaccination_vaccine_id"];
                    vaccination.vaccination_date = vaccinationDate;

                    scdb.Vaccination_SC.Add(vaccination);

                    scdb.SaveChanges();
                    visdb.SaveChanges();

                    jsonResult = Json(new { isSuccess = true, birthdate = birthDate, vaccination_date = vaccinationDate }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                jsonResult = Json(new { isSuccess = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return jsonResult;
        }
        private string GetSexValue(bool male)
        {
            return (male) ? "m" : "f";
        }

        private string GetLivingArrangementValue(bool alone, bool spouse, bool children, bool others, string others_name = "")
        {
            string living_arrangement = "";

            if (alone)
                living_arrangement += "Living alone";

            if (spouse)
            {
                if (!string.IsNullOrEmpty(living_arrangement))
                    living_arrangement += ", ";
                living_arrangement += "With spouse";
            }

            if (children)
            {
                if (!string.IsNullOrEmpty(living_arrangement))
                    living_arrangement += ", ";
                living_arrangement += "With children";
            }

            if (others)
            {
                if (!string.IsNullOrEmpty(living_arrangement))
                    living_arrangement += ", ";
                living_arrangement += others_name;
            }

            return living_arrangement;
        }

        private string GetPensionerValue(bool sss, bool gsis, bool pvao, bool others, string others_name = "")
        {
            string pensioner = "";

            if (sss)
                pensioner += "SSS";

            if (gsis)
            {
                if (!string.IsNullOrEmpty(pensioner))
                    pensioner += ", ";
                pensioner += "GSIS";
            }

            if (pvao)
            {
                if (!string.IsNullOrEmpty(pensioner))
                    pensioner += ", ";
                pensioner += "Philippine Veterans Affairs Office (PVAO)";
            }

            if (others)
            {
                if (!string.IsNullOrEmpty(pensioner))
                    pensioner += ", ";
                pensioner += others_name;
            }

            return pensioner;
        }

        private bool IsDuplicateVaccination(int personId, int vaccineId)
        {
            return scdb.Vaccination_SC.Any(a => a.vaccination_person_id == personId && a.vaccination_vaccine_id == vaccineId);
        }
        public JsonResult VaccinationsDataTable()
        {
            try
            {
                var vaccinationIds = scdb.Vaccination_SC.Select(v => v.vaccination_person_id).ToList();

                var people = visdb.People
                    .Where(p => vaccinationIds.Contains(p.ID))
                    .ToList();

                var vaccinations = scdb.Vaccination_SC
                    .Where(v => vaccinationIds.Contains(v.vaccination_person_id))
                    .ToList()
                    .Select(v => new
                    {
                        v.vaccination_id,
                        Person = $"{people.FirstOrDefault(p => p.ID == v.vaccination_person_id)?.LastName}, {people.FirstOrDefault(p => p.ID == v.vaccination_person_id)?.FirstName}{(!string.IsNullOrEmpty(people.FirstOrDefault(p => p.ID == v.vaccination_person_id)?.MiddleName) && people.FirstOrDefault(p => p.ID == v.vaccination_person_id)?.MiddleName != "NONE" ? " " + people.FirstOrDefault(p => p.ID == v.vaccination_person_id)?.MiddleName : "")}",
                        Vaccine = v.Vaccine_SC.vaccine_description,
                        VaccineDate = v.vaccination_date,
                        Vaccinator = v.vaccination_vaccinator
                    })
                    .ToList();


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