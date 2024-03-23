using R12VIS.Models;
using R12VIS.Models.Dashboard;
using R12VIS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace R12VIS.Controllers
{
    [UserAuthenticationFilter]
    public class DashboardController : Controller
    {
        private DbContextR12 db = new DbContextR12();
        DbR12VISEntities db_ = new DbR12VISEntities();
        DbMRSpecialVacsEntities dbMRSpecialVacs = new DbMRSpecialVacsEntities();

        // GET: CityMunicipalities
        public ActionResult Index()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }

        public ActionResult DashboardMain()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name });
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }

        public ActionResult DashboardLastpage()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name });
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }

        public JsonResult GetBarChartData(int dose_id, int province_id = 0, int citymun_id = 0, int barangay_id = 0, string groupBy = "month") // , DateTime? startDate = null, DateTime? endDate = null
        {
            var vaccinations = db_.GetBarChartDataFunction(dose_id, province_id, citymun_id, barangay_id).ToList();

            var dv = db_.GetDashboardData(null, province_id, citymun_id, barangay_id);

            if (vaccinations != null || vaccinations.Count() > 0)
            {
                if (groupBy.ToLower() == "day")
                {
                    var chartDatas = vaccinations
                           .Where(x => x.VaccinationDate.HasValue)
                           .GroupBy(x => x.VaccinationDate) // Group by the Year and Month
                           .Select(x => new
                           {
                               Date = x.Key,
                               TotalEncoded = x.Count(),
                               AccumulativeTotal = vaccinations
                                   .Where(a => a.DoseID == dose_id
                                          && (a.VaccinationDate <= x.Key))
                                   .OrderBy(a => a.DateCreate)
                                   .Count()
                           })
                           .OrderBy(x => x.Date)
                           .ToList();
                    return Json(new { chartDatas, total = dv.Select(a => a.total).Sum(), isSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                else if (groupBy.ToLower() == "year")
                {
                    var chartDatas = vaccinations
                       .Where(x => x.VaccinationDate.HasValue)
                       .GroupBy(x => new { x.VaccinationDate.Value.Year }) // Group by the Year and Month
                       .Select(x => new
                       {
                           Date = new DateTime(x.Key.Year, 1, 1), // Create a DateTime object from Year and Month
                           TotalEncoded = x.Count(),
                           AccumulativeTotal = vaccinations
                               .Where(a => a.DoseID == dose_id
                                      && (a.VaccinationDate.Value.Year <= x.Key.Year))
                               .OrderBy(a => a.DateCreate)
                               .Count()
                       })
                       .OrderBy(x => x.Date)
                       .ToList();
                    return Json(new { chartDatas, total = dv.Select(a => a.total).Sum(), isSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var chartDatas = vaccinations
                        .Where(x => x.VaccinationDate.HasValue)
                        .GroupBy(x => new { x.VaccinationDate.Value.Year, x.VaccinationDate.Value.Month }) // Group by the Year and Month
                        .Select(x => new
                        {
                            Date = new DateTime(x.Key.Year, x.Key.Month, 1), // Create a DateTime object from Year and Month
                            TotalEncoded = x.Count(),
                            AccumulativeTotal = vaccinations
                                .Where(a => a.DoseID == dose_id
                                       && (a.VaccinationDate.Value.Year < x.Key.Year ||
                                           (a.VaccinationDate.Value.Year == x.Key.Year && a.VaccinationDate.Value.Month <= x.Key.Month)))
                                .OrderBy(a => a.DateCreate)
                                .Count()
                        })
                        .OrderBy(x => x.Date)
                        .ToList();
                    return Json(new { chartDatas, total = dv.Select(a => a.total).Sum(), isSuccess = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { Total = db.Vaccinations.Count(), isSuccess = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPieChartData(int dose_id, List<int> prioritygroup_ids, List<int> vaccine_ids, int province_id = 0, int citymun_id = 0, int barangay_id = 0)
        {
            string vaccines = null;
            var prioritygroup_id_list = string.Join(",", prioritygroup_ids);
            if (vaccine_ids != null)
                vaccines = string.Join(",", vaccine_ids);

            // FOR TOTAL
            var dv = db_.GetDashboardData(vaccines, province_id, citymun_id, barangay_id);

            var vaccinations = db_.GetPieChartDataFunction(dose_id, prioritygroup_id_list, vaccines, province_id, citymun_id, barangay_id).ToList();

            // TOTAL DAPAT NG vaccinations dito
            var totalVaccinated = dv.Select(a => a.total).Sum();
            var totalForPercent = vaccinations.Select(a => a.TotalPerVaccineBrand).Sum();

            var chartDatas = vaccinations.Select(x => new
            {
                VaccineBrandList = db.Vaccines.Where(a => a.ID == x.VaccineID).Select(a => a.VaccineManufacturer),
                TotalPerVaccineBrand = x.TotalPerVaccineBrand,
                VaccineBrand = x.VaccineBrand,
                Percentage = (x.TotalPerVaccineBrand / (double)totalForPercent) * 100
            }).OrderBy(a => a.TotalPerVaccineBrand).ToList();

            return Json(new { chartDatas, total = totalVaccinated }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VaccinesDataTable(List<int> vaccine_ids, int province_id = 0, int citymun_id = 0, int barangay_id = 0)
        {
            try
            {
                var dd = db.Vaccinations.ToList();
                var dose_list = db.Dose.ToList();
                string vaccines = null;
                if (vaccine_ids != null)
                    vaccines = string.Join(",", vaccine_ids);

                List<Dashboard1Data> dashboard_data_list = new List<Dashboard1Data>();

                foreach (var d in dose_list)
                {
                    var dv = db_.GetDashboardData(vaccines, province_id, citymun_id, barangay_id).Where(a => a.doseid == d.ID).FirstOrDefault();

                    if (dv != null)
                    {
                        Dashboard1Data dashdat = new Dashboard1Data();

                        dashdat.doseid = dv.doseid;
                        dashdat.dosename = dv.dosename;
                        dashdat.fivetoelevenyo = (int)dv.fivetoelevenyo;
                        dashdat.twelvetoseventeenyo = (int)dv.twelvetoseventeenyo;
                        dashdat.eighteentofiftynineyo = (int)dv.eighteentofiftynineyo;
                        dashdat.sixtyaboveyo = (int)dv.sixtyaboveyo;
                        dashdat.total = (int)dv.total;

                        dashboard_data_list.Add(dashdat);
                    }
                }

                Dashboard1Data dashtotal = new Dashboard1Data();
                dashtotal.doseid = 6;
                dashtotal.dosename = "TOTAL";
                dashtotal.fivetoelevenyo = dashboard_data_list.Select(a => a.fivetoelevenyo).Sum();
                dashtotal.twelvetoseventeenyo = dashboard_data_list.Select(a => a.twelvetoseventeenyo).Sum();
                dashtotal.eighteentofiftynineyo = dashboard_data_list.Select(a => a.eighteentofiftynineyo).Sum();
                dashtotal.sixtyaboveyo = dashboard_data_list.Select(a => a.sixtyaboveyo).Sum();
                dashtotal.total = dashboard_data_list.Select(a => a.total).Sum();

                dashboard_data_list.Add(dashtotal);

                var jsonResult = Json(new { data = dashboard_data_list.OrderBy(a => a.doseid) }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "City/Municipality/Barangay"
        public JsonResult CityMunicipalitiesDataTable(short provinceId)
        {
            try
            {
                var cityMunicipalities = db.CityMunicipalities.Where(a => a.province_id == provinceId).Select(a => new
                {
                    a.city_municipality_id,
                    a.CityMunicipalityName
                }).OrderBy(a => a.CityMunicipalityName).ToList();

                var jsonResult = Json(new { isSuccess = true, data = cityMunicipalities }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult BarangayDataTable(short citymunId)
        {
            try
            {
                var barangays = db.Barangays.Where(a => a.city_municipality_id == citymunId).Select(a => new
                {
                    a.barangay_id,
                    a.barangay_name
                }).OrderBy(a => a.barangay_name).ToList();

                var jsonResult = Json(new { isSuccess = true, data = barangays }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion "City/Municipality/Barangay"

        #region "PRIORITY GROUP DASHBOARD"

        public ActionResult DashboardPriorityGroup()
        {

            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name });
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }
        public JsonResult PriorityGroupDataTable(List<int> vaccine_ids, int province_id = 0, int citymun_id = 0, int barangay_id = 0)
        {
            try
            {
                string vaccines = null;
                if (vaccine_ids != null)
                    vaccines = string.Join(",", vaccine_ids);

                var dashboardData = db_.PriorityGroupDashboardData(vaccines, province_id, citymun_id, barangay_id).OrderBy(data => data.categoryname).ToList();

                // Create a dictionary to aggregate data for categories A1 and A3
                var aggregatedData = new Dictionary<string, Dashboard2Data>();

                foreach (var data in dashboardData)
                {
                    string categoryName = data.categoryname;

                    // Check if the category is A1 or A3 and use "A1" and "A3" as keys
                    if (categoryName == "A1" || categoryName == "A1.8" || categoryName == "A1.9" || categoryName == "Additional A1")
                    {
                        categoryName = "A1";
                    }
                    else if (categoryName == "A3 - Immunocompromised" || categoryName == "A3 - Immunocompetent" || categoryName == "Expanded A3" || categoryName == "Pediatric A3 (12-17 years old)" || categoryName == "Pediatric A3 (5-11 years old)")
                    {
                        categoryName = "A3";
                    }

                    if (!aggregatedData.ContainsKey(categoryName))
                    {
                        // Create a new entry in the dictionary if it doesn't exist
                        aggregatedData[categoryName] = new Dashboard2Data
                        {
                            categoryname = categoryName,
                            firstdose = 0,
                            seconddose = 0,
                            firstbooster = 0,
                            secondbooster = 0,
                            thirdbooster = 0,
                            total = 0,
                        };
                    }

                    // Aggregate the data
                    aggregatedData[categoryName].firstdose += (int)data.firstdose;
                    aggregatedData[categoryName].seconddose += (int)data.seconddose;
                    aggregatedData[categoryName].firstbooster += (int)data.firstbooster;
                    aggregatedData[categoryName].secondbooster += (int)data.secondbooster;
                    aggregatedData[categoryName].thirdbooster += (int)data.thirdbooster;
                    aggregatedData[categoryName].total += (int)data.total;
                }

                // Convert the dictionary values to a list
                var dashboard_data_list = aggregatedData.Values.ToList();

                Dashboard2Data dashtotal = new Dashboard2Data();
                dashtotal.categoryid = 15;
                dashtotal.categoryname = "TOTAL";
                dashtotal.firstdose = dashboard_data_list.Select(a => a.firstdose).Sum();
                dashtotal.seconddose = dashboard_data_list.Select(a => a.seconddose).Sum();
                dashtotal.firstbooster = dashboard_data_list.Select(a => a.firstbooster).Sum();
                dashtotal.secondbooster = dashboard_data_list.Select(a => a.secondbooster).Sum();
                dashtotal.thirdbooster = dashboard_data_list.Select(a => a.thirdbooster).Sum();
                dashtotal.total = dashboard_data_list.Select(a => a.total).Sum();

                dashboard_data_list.Add(dashtotal);

                var jsonResult = Json(new { data = dashboard_data_list.OrderBy(a => a.categoryid), total = dashtotal.total }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion "PRIORITY GROUP DASHBOARD"

        #region MRSIA
        public ActionResult DashboardMRSIA()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }

        public JsonResult MRSIADataTable(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetDashboardMRSIAData(region_id).OrderBy(data => data.Province);

                // Create a list to store the extracted data
                var extractedData = new List<DashboardMRSIAData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new DashboardMRSIAData();
                    data.Province = item.Province;
                    data.TARGET1 = item.TARGET1;
                    data.OPV = item.OPV;
                    data.PERCENT1 = item.PERCENT1;
                    data.TARGET2 = item.TARGET2;
                    data.MRV = item.MRV;
                    data.PERCENT2 = item.PERCENT2;
                    data.VitaminA = item.VitaminA;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new DashboardMRSIAData
                {
                    Province = "TOTAL",
                    TARGET1 = extractedData.Sum(item => item.TARGET1),
                    OPV = extractedData.Sum(item => item.OPV),
                    PERCENT1 = extractedData.Average(item => item.PERCENT1),
                    TARGET2 = extractedData.Sum(item => item.TARGET2),
                    MRV = extractedData.Sum(item => item.MRV),
                    PERCENT2 = extractedData.Average(item => item.PERCENT2),
                    VitaminA = extractedData.Sum(item => item.VitaminA)
                };

                // Add the total row to the extracted data
                extractedData.Add(totalRow);

                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult MRSIADataTableCityMun(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetDashboardMRSIADataCityMun(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<DashboardMRSIADataCityMun>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new DashboardMRSIADataCityMun();
                    data.CityMunName = item.CityMunName;
                    data.TARGET1 = item.TARGET1;
                    data.OPV = item.OPV;
                    data.PERCENT1 = item.PERCENT1;
                    data.TARGET2 = item.TARGET2;
                    data.MRV = item.MRV;
                    data.PERCENT2 = item.PERCENT2;
                    data.VitaminA = item.VitaminA;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new DashboardMRSIADataCityMun
                {
                    CityMunName = "TOTAL",
                    TARGET1 = extractedData.Sum(item => item.TARGET1),
                    OPV = extractedData.Sum(item => item.OPV),
                    PERCENT1 = extractedData.Average(item => item.PERCENT1),
                    TARGET2 = extractedData.Sum(item => item.TARGET2),
                    MRV = extractedData.Sum(item => item.MRV),
                    PERCENT2 = extractedData.Average(item => item.PERCENT2),
                    VitaminA = extractedData.Sum(item => item.VitaminA)
                };

                // Add the total row to the extracted data
                extractedData.Add(totalRow);

                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;


                /*// Return the extracted data as a JSON result
                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult MRSIADataTableBarangay(int cm_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetDashboardMRSIADataBarangay(cm_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<DashboardMRSIADataBarangay>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new DashboardMRSIADataBarangay();
                    data.BarangayName = item.BarangayName;
                    data.TARGET1 = item.TARGET1;
                    data.OPV = item.OPV;
                    data.PERCENT1 = item.PERCENT1;
                    data.TARGET2 = item.TARGET2;
                    data.MRV = item.MRV;
                    data.PERCENT2 = item.PERCENT2;
                    data.VitaminA = item.VitaminA;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new DashboardMRSIADataBarangay
                {
                    BarangayName = "TOTAL",
                    TARGET1 = extractedData.Sum(item => item.TARGET1),
                    OPV = extractedData.Sum(item => item.OPV),
                    PERCENT1 = extractedData.Average(item => item.PERCENT1),
                    TARGET2 = extractedData.Sum(item => item.TARGET2),
                    MRV = extractedData.Sum(item => item.MRV),
                    PERCENT2 = extractedData.Average(item => item.PERCENT2),
                    //VitaminA = extractedData.Sum(item => item.VitaminA)
                };

                // Add the total row to the extracted data
                extractedData.Add(totalRow);

                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult BarchartMRSIAOccupationData(int region_id = 0, int province_id = 0, int cm_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetBarChartOccupationData(region_id, province_id, cm_id).OrderBy(data => data.descriptionName);

                // Create a list to store the extracted data
                var extractedData = new List<BarChartMRSIAOccupationData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new BarChartMRSIAOccupationData();
                    data.descriptionName = item.descriptionName;
                    data.OPV = item.OPV;
                    data.MRV = item.MRV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }
                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult BarchartMRSIAIncomeClassData(int region_id = 0, int province_id = 0, int cm_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetBarChartIncomeClassData(region_id, province_id, cm_id).OrderBy(data => data.descriptionName);

                // Create a list to store the extracted data
                var extractedData = new List<BarChartMRSIAIncomeData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new BarChartMRSIAIncomeData();
                    data.descriptionName = item.descriptionName;
                    data.OPV = item.OPV;
                    data.MRV = item.MRV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                /*var totalRow = new BarChartMRSIAOccupationData
                {
                    descriptionName = "TOTAL",
                    OPV = extractedData.Sum(item => item.OPV),
                    MRV = extractedData.Sum(item => item.MRV)
                };*/

                // Add the total row to the extracted data
                //extractedData.Add(totalRow);

                // Return the extracted data as a JSON result
                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult BarchartMRSIAReligionData(int region_id = 0, int province_id = 0, int cm_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetBarChartReligionData(region_id, province_id, cm_id).OrderBy(data => data.descriptionName);

                // Create a list to store the extracted data
                var extractedData = new List<BarChartMRSIAReligionData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new BarChartMRSIAReligionData();
                    data.descriptionName = item.descriptionName;
                    data.OPV = item.OPV;
                    data.MRV = item.MRV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Return the extracted data as a JSON result
                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult BarchartMRSIAPersonRankData(int region_id = 0, int province_id = 0, int cm_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetBarChartPersonRankData(region_id, province_id, cm_id).OrderBy(data => data.SiblingRank);

                // Create a list to store the extracted data
                var extractedData = new List<BarChartMRSIASiblingkRankData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new BarChartMRSIASiblingkRankData();
                    data.SiblingRank = item.SiblingRank;
                    data.OPV = item.OPV;
                    data.MRV = item.MRV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion MRSIA

        #region "MRSIA ROUTINE"

        public ActionResult Routine()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }

        public JsonResult RoutineProvinceDataTable(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetDashboardRoutineProvinceData(region_id).OrderBy(data => data.Province);

                // Create a list to store the extracted data
                var extractedData = new List<DashboardRoutineProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new DashboardRoutineProvinceData();
                    data.Province = item.Province;
                    data.TOTAL = item.TOTAL;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new DashboardRoutineProvinceData
                {
                    Province = "TOTAL",
                    TOTAL = extractedData.Sum(item => item.TOTAL),
                };

                // Add the total row to the extracted data
                extractedData.Add(totalRow);

                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineCityMunDataTable(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetDashboardRoutineCityMunData(province_id).OrderBy(data => data.CityMun);

                // Create a list to store the extracted data
                var extractedData = new List<DashboardRoutineCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new DashboardRoutineCityMunData();
                    data.CityMun = item.CityMun;
                    data.TOTAL = item.TOTAL;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new DashboardRoutineCityMunData
                {
                    CityMun = "TOTAL",
                    TOTAL = extractedData.Sum(item => item.TOTAL),
                };

                // Add the total row to the extracted data
                extractedData.Add(totalRow);

                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult RoutineBarangayDataTable(int cm_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetDashboardRoutineBarangayData(cm_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<DashboardRoutineBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new DashboardRoutineBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.TOTAL = item.TOTAL;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new DashboardRoutineBarangayData
                {
                    BarangayName = "TOTAL",
                    TOTAL = extractedData.Sum(item => item.TOTAL),
                };

                // Add the total row to the extracted data
                extractedData.Add(totalRow);

                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public JsonResult RoutineSchedules(int schedule_is_active = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetScheduleData(schedule_is_active).OrderBy(data => data.Schedule);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineScheduleData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineScheduleData();
                    data.No_ = (int?)item.No_;
                    data.First_Name = item.First_Name;
                    data.Last_Name = item.Last_Name;
                    data.Vaccine_Dose = item.Vaccine_Dose;
                    data.Schedule = (DateTime?)item.Schedule;


                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                // Calculate the total values
                var totalRow = new RoutineScheduleData
                {
                    No_ = extractedData.Count(),
                };


                var jsonResult = Json(new { isSuccess = true, data = extractedData, total = totalRow }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion "MRSIA ROUTINE"

        #region ROUTINE VAX
        public ActionResult Routine_1()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }

        public JsonResult RoutineAtBirthVaccinesProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineAtBirthVaccinesProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineAtBirthVaccinesProvinceData();
                    data.ProvinceName = item.ProvinceName;
                    data.BCG = item.BCG;
                    data.Hepatitis_B = item.Hepatitis_B;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }
                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public JsonResult RoutineAtBirthVaccinesProvinceData(int region_id = 0)
        //{
        //    try
        //    {
        //        // Execute the SQL query and retrieve the data
        //        var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

        //        // Create a list to store the extracted data
        //        var extractedData = new List<RoutineAtBirthVaccinesProvinceData>();

        //        // Loop through the query result and extract the relevant data
        //        foreach (var item in dashboardData)
        //        {
        //            var data = new RoutineAtBirthVaccinesProvinceData();
        //            data.ProvinceName = item.ProvinceName;
        //            data.BCG = item.BCG;
        //            data.Hepatitis_B = item.Hepatitis_B;
        //            data.PVI = item.PVI;
        //            data.PVII = item.PVII;
        //            data.PVIII = item.PVIII;
        //            data.OPVI = item.OPVI;
        //            data.OPVII = item.OPVII;
        //            data.OPVIII = item.OPVIII;
        //            data.IPVI = item.IPVI;
        //            data.IPVII = item.IPVII;
        //            data.PCVI = item.PCVI;
        //            data.PCVII = item.PCVII;
        //            data.PCVIII = item.PCVIII;
        //            data.MMRI = item.MMRI;
        //            data.MMRII = item.MMRII;

        //            // Add the extracted data to the list
        //            extractedData.Add(data);
        //        }
        //        var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public JsonResult RoutinePVVaccinesProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutinePVVaccinesProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutinePVVaccinesProvinceData();
                    data.ProvinceName = item.ProvinceName;
                    data.PVI = item.PVI;
                    data.PVII = item.PVII;
                    data.PVIII = item.PVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineOPVVaccinesProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineOPVVaccinesProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineOPVVaccinesProvinceData();
                    data.ProvinceName = item.ProvinceName;
                    data.OPVI = item.OPVI;
                    data.OPVII = item.OPVII;
                    data.OPVIII = item.OPVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineIPVVaccinesProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineIPVVaccinesProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineIPVVaccinesProvinceData();
                    data.ProvinceName = item.ProvinceName;
                    data.IPVI = item.IPVI;
                    data.IPVII = item.IPVII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutinePCVVaccinesProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutinePCVVVaccinesProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutinePCVVVaccinesProvinceData();
                    data.ProvinceName = item.ProvinceName;
                    data.PCVI = item.PCVI;
                    data.PCVII = item.PCVII;
                    data.PCVIII = item.PCVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineMMRVaccinesProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesProvinceData(region_id).OrderBy(data => data.ProvinceName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineMMRVaccinesProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineMMRVaccinesProvinceData();
                    data.ProvinceName = item.ProvinceName;
                    data.MMRI = item.MMRI;
                    data.MMRII = item.MMRII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public JsonResult RoutineAtBirthVaccinesCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesCityMunData(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineAtBirthVaccinesCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineAtBirthVaccinesCityMunData();
                    data.CityMunName = item.CityMunName;
                    data.BCG = item.BCG;
                    data.Hepatitis_B = item.Hepatitis_B;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }
                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutinePVVaccinesCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesCityMunData(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutinePVVaccinesCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutinePVVaccinesCityMunData();
                    data.CityMunName = item.CityMunName;
                    data.PVI = item.PVI;
                    data.PVII = item.PVII;
                    data.PVIII = item.PVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineOPVVaccinesCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesCityMunData(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineOPVVaccinesCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineOPVVaccinesCityMunData();
                    data.CityMunName = item.CityMunName;
                    data.OPVI = item.OPVI;
                    data.OPVII = item.OPVII;
                    data.OPVIII = item.OPVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineIPVVaccinesCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesCityMunData(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineIPVVaccinesCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineIPVVaccinesCityMunData();
                    data.CityMunName = item.CityMunName;
                    data.IPVI = item.IPVI;
                    data.IPVII = item.IPVII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutinePCVVaccinesCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesCityMunData(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutinePCVVVaccinesCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutinePCVVVaccinesCityMunData();
                    data.CityMunName = item.CityMunName;
                    data.PCVI = item.PCVI;
                    data.PCVII = item.PCVII;
                    data.PCVIII = item.PCVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineMMRVaccinesCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesCityMunData(province_id).OrderBy(data => data.CityMunName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineMMRVaccinesCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineMMRVaccinesCityMunData();
                    data.CityMunName = item.CityMunName;
                    data.MMRI = item.MMRI;
                    data.MMRII = item.MMRII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public JsonResult RoutineAtBirthVaccinesBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesBarangayData(citymun_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineAtBirthVaccinesBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineAtBirthVaccinesBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.BCG = item.BCG;
                    data.Hepatitis_B = item.Hepatitis_B;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }
                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutinePVVaccinesBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesBarangayData(citymun_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutinePVVaccinesBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutinePVVaccinesBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.PVI = item.PVI;
                    data.PVII = item.PVII;
                    data.PVIII = item.PVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineOPVVaccinesBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesBarangayData(citymun_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineOPVVaccinesBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineOPVVaccinesBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.OPVI = item.OPVI;
                    data.OPVII = item.OPVII;
                    data.OPVIII = item.OPVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineIPVVaccinesBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesBarangayData(citymun_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineIPVVaccinesBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineIPVVaccinesBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.IPVI = item.IPVI;
                    data.IPVII = item.IPVII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutinePCVVaccinesBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesBarangayData(citymun_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutinePCVVVaccinesBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutinePCVVVaccinesBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.PCVI = item.PCVI;
                    data.PCVII = item.PCVII;
                    data.PCVIII = item.PCVIII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult RoutineMMRVaccinesBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineVaccinesBarangayData(citymun_id).OrderBy(data => data.BarangayName);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineMMRVaccinesBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineMMRVaccinesBarangayData();
                    data.BarangayName = item.BarangayName;
                    data.MMRI = item.MMRI;
                    data.MMRII = item.MMRII;
                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData,/* total = totalRow */}, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public ActionResult Routine_2()
        {
            var provinces = db.Provinces.Where(a => a.RegionID == 1).OrderBy(a => a.province_name).Select(a => new { value = a.province_id, text = a.province_name }).ToList();
            ViewBag.Province = new SelectList(provinces, "value", "text");

            return View();
        }


        public JsonResult LineChartRoutineProvinceData(int region_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetLineChartVPDVaccinationProvinceData(region_id).OrderBy(data => data.VM);

                // Create a list to store the extracted data
                var extractedData = new List<LineChartVPDVaccinationProvinceData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new LineChartVPDVaccinationProvinceData();
                    data.VM = item.VM;
                    data.NoV = item.NoV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult LineChartRoutineCityMunData(int province_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetLineChartVPDVaccinationCityMunData(province_id).OrderBy(data => data.VM);

                // Create a list to store the extracted data
                var extractedData = new List<LineChartVPDVaccinationCityMunData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new LineChartVPDVaccinationCityMunData();
                    data.VM = item.VM;
                    data.NoV = item.NoV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult LineChartRoutineBarangayData(int citymun_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetLineChartVPDVaccinationBarangayData(citymun_id).OrderBy(data => data.VM);

                // Create a list to store the extracted data
                var extractedData = new List<LineChartVPDVaccinationBarangayData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new LineChartVPDVaccinationBarangayData();
                    data.VM = item.VM;
                    data.NoV = item.NoV;

                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
















        public JsonResult RoutineVaxxDashboard(int vaccine_type_id = 0)
        {
            try
            {
                // Execute the SQL query and retrieve the data
                var dashboardData = dbMRSpecialVacs.GetRoutineCount(vaccine_type_id).OrderBy(data => data.Vaccine);

                // Create a list to store the extracted data
                var extractedData = new List<RoutineVaxxDashboardData>();

                // Loop through the query result and extract the relevant data
                foreach (var item in dashboardData)
                {
                    var data = new RoutineVaxxDashboardData();
                    data.Vaccine = item.Vaccine;
                    data.AT_BIRTH = item.AT_BIRTH;
                    data.C1ST_VISIT = item.C1ST_VISIT;
                    data.C2ND_VISIT = item.C2ND_VISIT;
                    data.C3RD_VISIT = item.C3RD_VISIT;
                    data.C4TH_VISIT = item.C4TH_VISIT;
                    data.C5TH_VISIT = item.C5TH_VISIT;


                    // Add the extracted data to the list
                    extractedData.Add(data);
                }

                var jsonResult = Json(new { isSuccess = true, data = extractedData }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion ROUTINE VAX
    }
}