using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using Microsoft.Ajax.Utilities;
using R12VIS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace R12VIS.Controllers
{
    public class UploaderController : Controller
    {
        private DbContextR12 db = new DbContextR12();
        PublicVariables pb = new PublicVariables();
        
        // GET: Uploader
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult PersonUploader() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Uploader(HttpPostedFileBase myExcelData) 
        {
            if (myExcelData != null)
            {
                if (myExcelData.ContentLength > 0)
                {
                    pb.CompleteFilePath = pb.filePath + pb.fileName + ".xlsx";
                    myExcelData.SaveAs(pb.CompleteFilePath);
                    XLWorkbook xlworkbook = new XLWorkbook(pb.CompleteFilePath);

                    // loop excel rows and get data on each cells
                    while (xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 1).GetString() != "")
                    {
                        // PERSON TABLE
                        pb.uniquepersonid = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 3).GetString();
                        pb.pwd = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 4).GetString();
                        pb.ethnicgroup = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 5).GetString();
                        pb.lastname = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 6).GetString();
                        pb.firstname = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 7).GetString();
                        pb.middlename = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 8).GetString();
                        pb.suffix = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 9).GetString();
                        pb.contactnumber = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 10).GetString();
                        pb.guardianname = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 11).GetString();
                        pb.gender = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 16).GetString();
                        pb.birthdate = xlworkbook.Worksheet(pb.worksheet).Cell(pb.row, 17).GetString();


                        // MAIN TABEL
                        pb.category = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 1).GetString();
                        pb.comorbidity = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 2).GetString();
                        pb.region = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 12).GetString();
                        pb.province = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 13).GetString();
                        pb.citymunicipality = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 14).GetString();
                        pb.barangay = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 15).GetString();
                        pb.deferral = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 18).GetString();
                        pb.reasonfordeferral = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 19).GetString();
                        pb.vaccinationdate = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 20).GetString();
                        pb.vaccinemanufacturername = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 21).GetString();
                        pb.batchnumber = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 22).GetString();
                        pb.lotnumber = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 23).GetString();
                        pb.bakunacentercbcrid = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 24).GetString();
                        pb.vaccinatorname = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 25).GetString();
                        pb.firstdose = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 26).GetString();
                        pb.seconddose = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 27).GetString();
                        pb.additionalboosterdose = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 28).GetString();
                        pb.adverseevent = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 29).GetString();
                        pb.adverseeventcondition = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 30).GetString();


                        // CHECK FOR EMPTY FIELDS
                        if (
                            // PERSON TABLE
                            pb.uniquepersonid != "" &&
                            pb.firstname != "" && 
                            pb.middlename != "" && 
                            pb.lastname != "" && 
                            pb.gender != "" && 
                            pb.birthdate != "" &&
                            pb.pwd != "" &&
                            pb.ethnicgroup != "" &&
                            pb.contactnumber != "" &&

                            // VACCINATION TABLE
                            pb.category != "" &&
                            pb.guardianname != "" &&
                            pb.region != "" &&
                            pb.province != "" &&
                            pb.citymunicipality != "" &&
                            pb.barangay != "" &&
                            pb.vaccinationdate != "" &&
                            pb.vaccinemanufacturername != "" &&
                            pb.batchnumber != "" &&
                            pb.lotnumber != "" &&
                            pb.bakunacentercbcrid != "" &&
                            pb.vaccinatorname != "" &&
                            pb.firstdose != "" &&
                            pb.seconddose != "" &&
                            pb.additionalboosterdose != "" &&
                            pb.adverseevent != "" &&
                            pb.adverseeventcondition != ""
                            )
                        {

                            // DATE VALIDATOR
                            //BIRTHDATE
                            // check if value is already in date format
                            if (System.DateTime.TryParse(pb.birthdate, out pb.parsedDate)) 
                            {
                                pb.isValidBirthDate = true;
                            }
                            // chaeck if value is in double format
                            else if (double.TryParse(pb.birthdate, out pb.parsedResult))
                            {
                                // get value as double
                                var dataExcelValue = xlworkbook.Worksheet(pb.worksheet).Cell(pb.row, 17).GetDouble();

                                // convert double value to date time format
                                pb.birthdate = System.DateTime.FromOADate(dataExcelValue).ToString();

                                // validate datetime value
                                pb.isValidBirthDate = System.DateTime.TryParseExact(pb.birthdate, pb.dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out pb.parsedDate);
                            }


                            //VACCINATION DATE
                            // check if value is already in date format
                            if (System.DateTime.TryParse(pb.vaccinationdate, out pb.parsedDate))
                            {
                                pb.isValidVaccinationDate = true;
                            }
                            // chaeck if value is in double format
                            else if (double.TryParse(pb.vaccinationdate, out pb.parsedResult))
                            {
                                // get value as double
                                var dataExcelValue = xlworkbook.Worksheet(pb.worksheet).Cell(pb.row, 20).GetDouble();

                                // convert double value to date time format
                                pb.vaccinationdate = System.DateTime.FromOADate(dataExcelValue).ToString();

                                // validate datetime value
                                pb.isValidVaccinationDate = System.DateTime.TryParseExact(pb.vaccinationdate, pb.dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out pb.parsedDate);
                            }

                            // IF DATE IS VALID 
                            if (pb.isValidBirthDate) 
                            {
                                if (pb.isValidVaccinationDate)
                                {
                                    // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                    // PERSON
                                    pb.birthdateForQry = pb.birthdate.AsDateTime();
                                    // VACCINATION
                                    pb.vaccinationdateForQry = pb.vaccinationdate.AsDateTime();



                                    // GET REFERENCE ID's

                                    // PERSON 
                                    // Get Ethnic Group ID
                                    var GetEthnicGroupId = db.EthnicGroups.FirstOrDefault(s => s.IndigenousMember == pb.ethnicgroup);

                                    // VACCINATION
                                    // Get Priority Group ID
                                    var GetPriorityGroupId = db.PriorityGroups.FirstOrDefault(s => s.Category == pb.category);

                                    //// Get Region ID
                                    //var GetRegionId = db.Regions.FirstOrDefault(s => s.RegionName == pb.region);

                                    // Get Province ID
                                    var GetProvinceId = db.Provinces.FirstOrDefault(s => s.province_code_excel == pb.province);

                                    // Get City Municipality ID
                                    var GetCityMunicipalityId = db.CityMunicipalities.FirstOrDefault(s => s.CityMunicipalityCodeExcel == pb.citymunicipality);

                                    // Get Barangay ID
                                    var GetBarangayId = db.Barangays.FirstOrDefault(s => 
                                    s.barangay_name == pb.barangay);

                                    if (GetBarangayId == null)
                                    {
                                        Barangay s = new Barangay();
                                        s.city_municipality_id = GetCityMunicipalityId.city_municipality_id;
                                        s.barangay_name = pb.barangay;
                                        s.province_code = GetProvinceId.province_code;
                                        s.city_municipality_code = GetCityMunicipalityId.CityMunicipalityCode;
                                        db.Barangays.Add(s);
                                        db.SaveChanges();

                                        // Get Barangay ID
                                        var GetBarangayId2 = db.Barangays.FirstOrDefault(w =>
                                        w.barangay_name == pb.barangay);

                                        pb.BarangayId = GetBarangayId2.barangay_id;
                                    }
                                    else
                                    {
                                        pb.BarangayId = GetBarangayId.barangay_id;
                                    }

                                    // Get Vaccine Manufacturer ID
                                    var GetVaccineManufacturerId = db.Vaccines.FirstOrDefault(s => 
                                    s.VaccineManufacturer == pb.vaccinemanufacturername);


                                    // Get Vaccine Manufacturer ID
                                    // FIRST DOSE
                                    if (pb.firstdose.ToLower() == "y" && pb.seconddose.ToLower() == "n" && pb.additionalboosterdose.ToLower() == "n")
                                    {
                                        var GetDoseId = db.Dose.FirstOrDefault(s => 
                                        s.VaccineDose.ToLower() == "1st dose");
                                        pb.DoseId = GetDoseId.ID;
                                    }
                                    // SECOND DOSE
                                    else if (pb.firstdose.ToLower() == "n" && pb.seconddose.ToLower() == "y" && pb.additionalboosterdose.ToLower() == "n")
                                    {
                                        var GetDoseId = db.Dose.FirstOrDefault(s => 
                                        s.VaccineDose.ToLower() == "2nd dose");
                                        pb.DoseId = GetDoseId.ID;
                                    }
                                    // ADDITIONAL BOOSTER DOSE
                                    else if (pb.firstdose.ToLower() == "n" && pb.seconddose.ToLower() == "n" && pb.additionalboosterdose.ToLower() == "y")
                                    {
                                        var GetDoseId = db.Dose.FirstOrDefault(s => 
                                        s.VaccineDose.ToLower() == "additional/booster dose");
                                        pb.DoseId = GetDoseId.ID;
                                    }


                                    // CHECK GENDER
                                    if (pb.gender.ToLower() == "m")
                                    {
                                        pb.Gender = "Male";
                                    }
                                    else
                                    {
                                        pb.Gender = "Female";
                                    }

                                    // CHECK PWD
                                    if (pb.pwd.Substring(0, 1).ToLower() == "y")
                                    {
                                        pb.isPWD = true;
                                    }
                                    else
                                    {
                                        pb.isPWD = false;
                                    }

                                    if (GetEthnicGroupId != null)
                                    {
                                        // CHECKING FOR DUPLICATES

                                        // PERSON
                                        pb.PersonDuplicatesChecker = db.Persons.Where(d =>
                                        d.FirstName.ToLower() == pb.firstname.ToLower() &&
                                        d.MiddleName.ToLower() == pb.middlename.ToLower() &&
                                        d.LastName.ToLower() == pb.lastname.ToLower() &&
                                        d.Gender == pb.Gender &&
                                        d.BirthDate == pb.birthdateForQry &&
                                        d.isPWD == pb.isPWD).Any();

                                        // NO DUPLICATES
                                        if (pb.PersonDuplicatesChecker == false)
                                        {
                                            Person y = new Person();
                                            y.UniquePersonID = pb.uniquepersonid;
                                            y.FirstName = pb.firstname;
                                            y.MiddleName = pb.middlename;
                                            y.LastName = pb.lastname;
                                            y.Suffix = pb.suffix;
                                            y.ContactNumber = pb.contactnumber;
                                            y.GuardianName = pb.guardianname;
                                            y.Gender = pb.Gender;
                                            y.isPWD = pb.isPWD;
                                            y.EthnicGroupID = GetEthnicGroupId.Id;
                                            y.BirthDate = pb.birthdateForQry;

                                            db.Persons.Add(y);
                                            db.SaveChanges();

                                            pb.PersonUploadedCounter++;
                                        }
                                        else
                                        {
                                            pb.PersonDuplicateCounter++;
                                        }


                                        // VACCINATION

                                        // CHECKER DEFERRAL AND REASON FOR DEFERRAL
                                        if (pb.deferral.ToLower() == "y" && pb.reasonfordeferral != "" ||
                                            pb.deferral.ToLower() == "n" && pb.reasonfordeferral == "" ||
                                            pb.deferral.ToLower() == "n" && pb.reasonfordeferral.ToLower() == "none"
                                            )
                                        {
                                            // GET DEFERRAL ID
                                            if (pb.deferral.ToLower() == "y" && pb.reasonfordeferral != "")
                                            {
                                                var GetDeferralID = db.Deferrals.SingleOrDefault(s =>
                                                s.Reason == pb.reasonfordeferral);

                                                if (GetDeferralID != null)
                                                {
                                                    pb.DeferralId = GetDeferralID.Id;
                                                }
                                                else
                                                {
                                                    pb.DeferralId = 0;
                                                }
                                            }

                                            if (pb.DeferralId > 0 && pb.deferral.ToLower() == "y" ||
                                                pb.DeferralId == 0 && pb.deferral.ToLower() == "n")
                                            {
                                                // GET PERSON ID
                                                var GetPersonID = db.Persons.FirstOrDefault(s =>
                                                s.FirstName.ToLower() == pb.firstname.ToLower() &&
                                                s.LastName.ToLower() == pb.lastname.ToLower() &&
                                                s.MiddleName.ToLower() == pb.middlename.ToLower() &&
                                                s.Gender == pb.Gender &&
                                                s.isPWD == pb.isPWD &&
                                                s.BirthDate == pb.birthdateForQry);

                                                // GET VACCINE ID
                                                var GetVaccineID = db.Vaccines.SingleOrDefault(s =>
                                                s.VaccineManufacturer == pb.vaccinemanufacturername);

                                                // GET ADVERSE EVENT ID
                                                if (pb.adverseevent.ToLower() == "y")
                                                {
                                                    var GetAdverseID = db.Adverses.SingleOrDefault(s =>
                                                    s.Condition == pb.adverseeventcondition);

                                                    if (GetAdverseID != null)
                                                    {
                                                        pb.AdverseID = GetAdverseID.ID;
                                                    }
                                                }



                                                // check references if not empty
                                                if (GetPersonID != null &&
                                                    GetPriorityGroupId != null &&
                                                    GetProvinceId != null &&
                                                    GetCityMunicipalityId != null &&
                                                    pb.BarangayId > 0 &&
                                                    GetVaccineManufacturerId != null &&
                                                    GetVaccineID != null &&
                                                    pb.DoseId > 0)
                                                {


                                                    // CHEACK FOR DUPLICATES
                                                    var VaccinationDuplicateScanner = db.Vaccinations.FirstOrDefault(z => 
                                                    z.PersonID == GetPersonID.ID && z.DoseID == pb.DoseId);

                                                    if (VaccinationDuplicateScanner == null)
                                                    {
                                                        Vaccination v = new Vaccination();
                                                        v.PriorityGroupID = GetPriorityGroupId.ID;
                                                        v.PersonID = GetPersonID.ID;
                                                        v.Person.ProvinceID = GetProvinceId.province_id;
                                                        v.Person.CityMunicipalityID = GetCityMunicipalityId.city_municipality_id;
                                                        v.Person.BarangayID = pb.BarangayId;

                                                        if (pb.DeferralId > 0)
                                                        {
                                                            v.DeferralID = pb.DeferralId;
                                                        }

                                                        v.VaccinationDate = pb.vaccinationdate.AsDateTime();
                                                        v.VaccineID = GetVaccineManufacturerId.ID;
                                                        v.BatchNumber = pb.batchnumber;
                                                        v.LotNumber = pb.lotnumber;
                                                        v.BakunaCenterCBCRID = pb.bakunacentercbcrid;
                                                        v.VaccinatorName = pb.vaccinatorname;
                                                        v.DoseID = pb.DoseId;

                                                        if (pb.AdverseID > 0)
                                                        {
                                                            v.AdverseID = pb.AdverseID;
                                                        }

                                                        v.Person.CityMunicipalityID = GetCityMunicipalityId.city_municipality_id;
                                                        v.Person.ProvinceID = GetProvinceId.province_id;
                                                        v.Comorbidity = pb.comorbidity;
                                                        db.Vaccinations.Add(v);
                                                        db.SaveChanges();

                                                        pb.VaccinationUploadedCounter++;
                                                    }
                                                    else
                                                    {
                                                        pb.VaccinationDuplicateCounter++;
                                                    }
                                                }
                                                else
                                                {
                                                    pb.VaccinationErrorCounter++;
                                                }
                                            }
                                            else
                                            {
                                                pb.VaccinationErrorCounter++;
                                            }
                                        }
                                        else
                                        {
                                            pb.VaccinationErrorCounter++;
                                        }
                                    }
                                    else
                                    {
                                        pb.PersonErrorCounter++;
                                        pb.VaccinationErrorCounter++;
                                    }
                                }
                                else
                                {
                                    pb.VaccinationErrorCounter++;
                                }
                            }
                            else
                            {
                                pb.PersonErrorCounter++;
                            }
                        }
                        pb.row++;
                    }

                    var data = new
                    {
                        mssg = "success" ,
                        personduplicate = pb.PersonDuplicateCounter,
                        personerror = pb.PersonErrorCounter,
                        personuploaded = pb.PersonUploadedCounter,

                        vaccineduplicate = pb.VaccinationDuplicateCounter,
                        vaccineerror = pb.VaccinationErrorCounter,
                        vaccineuploaded = pb.VaccinationUploadedCounter
                    };

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            var data2 = new
            {
                mssg = "error"
            };

            return Json(data2, JsonRequestBehavior.AllowGet);

        }
    }
}