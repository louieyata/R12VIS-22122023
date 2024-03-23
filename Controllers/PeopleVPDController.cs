using Newtonsoft.Json;
using R12VIS.Models;
using R12VIS.Models.ViewModel;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    public class PeopleVPDController : Controller
    {
        DbContextR12 db = new DbContextR12();
        DbMRSpecialVacsEntities vpddb = new DbMRSpecialVacsEntities();
        DbR12VISEntities visdb = new DbR12VISEntities();
        public ActionResult Index()
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
            }, "value", "text", 1);

            ViewBag.SiblingRankList = siblingRank;
            ViewBag.Region = new SelectList(regions, "value", "text", 1);
            ViewBag.Province = new SelectList(provinces, "value", "text");
            ViewBag.EthnicGroupList = new SelectList(ethnic_group_list, "Id", "IndigenousMember", 0);
            ViewBag.SuffixList = new SelectList(suffixes, "id", "suffix_description", 1);
            ViewBag.ReligionList = new SelectList(religions, "ID", "Description");
            ViewBag.EducationalAttainmentList = new SelectList(educational_attainment_list, "ID", "Description");
            ViewBag.IncomeClassList = new SelectList(income_class, "ID", "Description");
            ViewBag.OccupationList = new SelectList(occupation, "ID", "Description");

            return View();
        }

        public ActionResult PersonalDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var person = vpddb.Person_VPD
                .Where(a => a.person_id == id)
                .FirstOrDefault();

            if (person == null)
            {
                return HttpNotFound();
            }

            _PersonVPDProfile _PersonProfile = new _PersonVPDProfile()
            {
                Person = person,
                Vaccinations = vpddb.Vaccination_VPD.Where(a => a.vaccination_person_id == person.person_id).ToList(),
                Province = db.Provinces.Where(a => a.province_id == person.person_province_address).Select(a => a.province_name).FirstOrDefault(),
                CityMunicipality = db.CityMunicipalities.Where(a => a.city_municipality_id == person.person_city_municipality_address).Select(a => a.CityMunicipalityName).FirstOrDefault(),
                Barangay = db.Barangays.Where(a => a.barangay_id == person.person_barangay_address).Select(a => a.barangay_name).FirstOrDefault(),
                Religion = db.Religions.Where(a => a.ID == person.person_religion_id).Select(a => a.Description).FirstOrDefault(),
                Occupation = db.Occupation.Where(a => a.ID == person.person_parent_occupation_id).Select(a => a.Description).FirstOrDefault(),
                EducationalAttainment = db.EducationalAttainment.Where(a => a.ID == person.person_parent_educational_attainment_id).Select(a => a.Description).FirstOrDefault(),
                IncomeClass = db.IncomeClass.Where(a => a.ID == person.person_parent_income_class_id).Select(a => a.Description).FirstOrDefault()

            };
            return View(_PersonProfile);
        }

        public JsonResult GetIndividual(int person_id)
        {
            try
            {
                var personData = vpddb.Person_VPD
                    .Where(a => a.person_id == person_id)
                    .Select(a => new
                    {
                        a.person_id,
                        a.person_first_name,
                        a.person_middle_name,
                        a.person_last_name,
                        a.person_suffix,
                        a.person_sex,
                        a.person_ethnic_group_id,
                        a.person_is_pwd,
                        a.person_birth_date,
                        a.person_province_address,
                        a.person_city_municipality_address,
                        a.person_barangay_address,
                        a.person_sibling_rank,
                        a.person_religion_id,
                        a.person_parent_educational_attainment_id,
                        a.person_parent_income_class_id,
                        a.person_parent_occupation_id
                    })
                    .FirstOrDefault();

                if (personData != null)
                {
                    // Use another context (visdb) for additional data
                    var suffixId = personData.person_suffix.ToUpper() != null
                        ? visdb.Suffixes
                            .Where(b => b.suffix_description == personData.person_suffix.ToUpper())
                            .Select(b => b.id)
                            .FirstOrDefault()
                        : 1;

                    var result = new
                    {
                        isSuccess = true,
                        personData.person_id,
                        personData.person_first_name,
                        personData.person_middle_name,
                        personData.person_last_name,
                        Suffix = suffixId,
                        personData.person_sex,
                        personData.person_ethnic_group_id,
                        personData.person_is_pwd,
                        personData.person_birth_date,
                        personData.person_province_address,
                        personData.person_city_municipality_address,
                        personData.person_barangay_address,
                        personData.person_sibling_rank,
                        personData.person_religion_id,
                        personData.person_parent_educational_attainment_id,
                        personData.person_parent_income_class_id,
                        personData.person_parent_occupation_id
                    };

                    var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                else
                {
                    var jsonResult = Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
            catch (Exception)
            {
                var jsonResult = Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

        public JsonResult SavePersonVPD(string jsonData, int person_id = 0)
        {
            var jsonResult = Json(JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            try
            {
                dynamic data = JsonConvert.DeserializeObject(jsonData);

                string firstName = data["individual_first_name"];
                string lastName = data["individual_last_name"];
                string suffix = string.IsNullOrWhiteSpace(data["individual_suffix"]?.ToString()) ? null : data["individual_suffix"].ToString();
                bool male = data["individual_sex_male"];
                string sexValue = GetSexValue(male);
                int ethnicGroupId;
                DateTime birthDate;
                DateTime.TryParse(data["individual_date_of_birth"]?.ToString(), out birthDate);

                Person_VPD person = new Person_VPD();
                if (person_id > 0)
                {
                    person = vpddb.Person_VPD.Where(a => a.person_id == person_id).FirstOrDefault();
                }

                // Nullable values
                person.person_ethnic_group_id = int.TryParse(data["ethnic_group_list"]?.ToString(), out ethnicGroupId) ? ethnicGroupId : (int?)null;
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

                if (person_id == 0)
                {
                    vpddb.Person_VPD.Add(person);
                }

                vpddb.SaveChanges();

                jsonResult = Json(new { isSuccess = true }, JsonRequestBehavior.AllowGet);
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

        public JsonResult PeopleDataTable()
        {
            try
            {
                var persons = vpddb.Person_VPD
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.person_id,
                        x.person_last_name,
                        x.person_first_name,
                        Address = GetAddressString(x.person_barangay_address, x.person_city_municipality_address, x.person_province_address),
                        Gender = x.person_sex.ToUpper()
                    })
                    .ToList();
                var successResponse = new { data = persons };
                var jsonResult = Json(successResponse, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                var errorResponse = new { error = ex.Message };
                var jsonResult = Json(errorResponse, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }

        private string GetAddressString(int? barangayId, int? cityMunicipalityId, int? provinceId)
        {
            var barangayName = db.Barangays.FirstOrDefault(a => a.barangay_id == barangayId)?.barangay_name?.ToUpper();
            var cityMunicipalityName = db.CityMunicipalities.FirstOrDefault(a => a.city_municipality_id == cityMunicipalityId)?.CityMunicipalityName?.ToUpper();
            var provinceName = db.Provinces.FirstOrDefault(a => a.province_id == provinceId)?.province_name?.ToUpper();

            return $"{barangayName}, {cityMunicipalityName}, {provinceName}";
        }
    }
}