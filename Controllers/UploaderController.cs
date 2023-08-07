
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

using System.IO;
using ClosedXML.Excel;
using static ClosedXML.Excel.XLPredefinedFormat;

using OfficeOpenXml;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Runtime.InteropServices.ComTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;
using WebGrease.Css.Ast;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

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

        // LETTER AND SPACES CHECKER - USED
        public bool ContainsOnlyLetters(string input)
        {
            // Use a regular expression to match only letters and spaces (alphabetic characters)
            //return Regex.IsMatch(input, "^[a-zA-Z-  ]+$");
            return Regex.IsMatch(input, @"^[a-zA-ZñÑ\- ']+$");
        }


        public bool CheckString(string inputString)
        {
            // Define the regular expression pattern
            string pattern = @"^[+]?[0-9]{10,12}$";

            // Use Regex.IsMatch() to check the inputString against the pattern
            bool isMatch = Regex.IsMatch(inputString, pattern);

            return isMatch;

        }


        // FUNCTIONING EXCEL DOWNLOAD
        [HttpGet]
        public ActionResult DownloadExcelTemplate()
        {

            // procedure to restore complete path and filename of existing excel
            TempData["excelfilename"] = "ExcelTemplateAndReferences.xlsx";


            pb.GetExcelFileName = TempData["excelfilename"] as string;
            TempData["excelfilename"] = pb.GetExcelFileName;

            pb.GetSaveTargetPath = Server.MapPath("/References/" + pb.GetExcelFileName); // complete excel path and filename

            // PROCESS ON HOW TO DELETE EXCEL ROW
            using (XLWorkbook wb = new XLWorkbook(pb.GetSaveTargetPath))
            {
                // PROCESS ON HOW TO DOWNLOAD EXCEL FILE
                using (MemoryStream ms = new MemoryStream())
                {
                    // Save the workbook to the stream
                    wb.SaveAs(ms);

                    // Return the file for download
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", pb.GetExcelFileName);
                }
            }
        }




        //async Task<ActionResult>
        public ActionResult Uploader(HttpPostedFileBase myExcelData, string selectedValue) 
        {
            // MAIN PROCESS
            int result;

            if (int.TryParse(selectedValue, out result))
            {
                if (result > 0)
                {
                    if (myExcelData != null)
                    {
                        if (myExcelData.ContentLength > 0)
                        {
                            pb.worksheet = Convert.ToInt32(selectedValue);
                            // Get Current Excel Filename
                            pb.GetExcelFileName = System.IO.Path.GetFileName(myExcelData.FileName);


                            // Get path where to save excel file to upload folder in the project
                            pb.GetSaveTargetPath = Server.MapPath("/Upload/");

                            // Generate excel new filename with datetime tag
                            pb.ExcelNewFileName = pb.datetime + pb.GetExcelFileName;
                            TempData["excelfilename"] = pb.ExcelNewFileName;

                            // complete path and new excel file
                            pb.CompleteFilePathAndFileName = pb.GetSaveTargetPath + pb.ExcelNewFileName;

                            // save excel file to upload folder in the project with new filename
                            myExcelData.SaveAs(pb.CompleteFilePathAndFileName);

                            // penetrate excel content
                            XLWorkbook xlworkbook = new XLWorkbook(pb.CompleteFilePathAndFileName);

                            // Cout Excel Sheets
                            pb.SheetCount = xlworkbook.Worksheets.Count;

                            // get total excel rows
                            pb.TotalExcelRows = xlworkbook.Worksheet(pb.worksheet).LastRowUsed().RowNumber() - 1;

                            // from VAS Excel
                            if (pb.SheetCount == 1 && result != 1)
                            {
                                return Json(new { success = false, message = "Excel Attached contains 1 sheet only so therefore, it is probably downloaded from VAS Line Website." });
                            }
                            else if (pb.SheetCount > 1 && result == 1)
                            {
                                return Json(new { success = false, message = "Excel Attached contains multiple sheets so therefore, it is probably Excel Default Template." });
                            }


                            var progress = 0;

                            // GET LIST
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

                                // MAIN TABLE
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
                                pb.secondadditionalboosterdose = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 29).GetString();
                                pb.adverseevent = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 30).GetString();
                                pb.adverseeventcondition = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 31).GetString();
                                pb.RowHash = xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 32).GetString();

                                // Unique Person ID
                                if (pb.uniquepersonid == "")
                                {
                                    pb.UniquePersonIdErrorMessage = "Unique Person ID must not empty,";
                                }

                                // First Name
                                if (pb.firstname != "")
                                {
                                    pb.FirstNameChecker = ContainsOnlyLetters(pb.firstname);

                                    if (pb.FirstNameChecker == false)
                                    {
                                        pb.FirstNameErrorMessage = "First Name is invalid,";
                                    }
                                }
                                else
                                {
                                    pb.FirstNameErrorMessage = "First Name must not empty,";
                                }

                                // Middle Name
                                if (pb.middlename != "")
                                {
                                    pb.MiddleNameChecker = ContainsOnlyLetters(pb.middlename);

                                    if (pb.MiddleNameChecker == false)
                                    {
                                        pb.MiddleNameErrorMessage = "Middle Name is invalid,";
                                    }

                                    //pb.MiddleNameErrorMessage = "* Middle Name must not empty!";
                                }

                                // Last Name
                                if (pb.lastname != "")
                                {
                                    pb.LastNameChecker = ContainsOnlyLetters(pb.lastname);

                                    if (pb.LastNameChecker == false)
                                    {
                                        pb.LastNameErrorMessage = "Last Name is invalid,";
                                    }
                                }
                                else
                                {
                                    pb.LastNameErrorMessage = "Last Name must not empty,";
                                }

                                // Contact Number
                                if (pb.contactnumber != "")
                                {
                                    pb.ContactNumberChecker = CheckString(pb.contactnumber);

                                    if (pb.ContactNumberChecker == false)
                                    {
                                        pb.ContactNumberErrorMessage = "Contact Number is invalid!";
                                    }
                                }
                                else
                                {
                                    pb.ContactNumberErrorMessage = "Contact number must not empty,";
                                }


                                // Suffix
                                if (pb.suffix != "")
                                {
                                    pb.SuffixChecker = ContainsOnlyLetters(pb.suffix);

                                    if (pb.SuffixChecker == false)
                                    {
                                        pb.SuffixErrorMessage = "Suffix is invalid,";
                                    }
                                }


                                // Gender
                                // CHECK GENDER
                                if (pb.gender.ToLower().StartsWith("m"))
                                {
                                    //pb.isMale = true;
                                    pb.gender = "m";
                                    pb.GenderCheck = true;
                                }
                                else if (pb.gender.ToLower().StartsWith("f"))
                                {
                                    //pb.isMale = false;
                                    pb.gender = "f";
                                    pb.GenderCheck = true;
                                }
                                else
                                {
                                    pb.GenderErrorMessage = "Sex is either Incorrect or empty,";
                                    pb.GenderCheck = false;
                                }


                                // PWD
                                // CHECK PWD
                                // No or N || yes or Y
                                if (pb.pwd.ToLower().StartsWith("y"))
                                {
                                    pb.isPWD = true;
                                    pb.PWDCheck = true;
                                }
                                else if (pb.pwd.ToLower().StartsWith("n"))
                                {
                                    pb.isPWD = false;
                                    pb.PWDCheck = true;
                                }
                                else
                                {
                                    pb.PWDErrorMessage = "PWD is either incorrect or empty,";
                                    pb.PWDCheck = false;
                                }

                                // Ethnic Group
                                // Get Ethnic Group ID
                                var GetEthnicGroupId = db.EthnicGroups.FirstOrDefault(s => s.IndigenousMember.ToLower() == pb.ethnicgroup.ToLower());

                                //Category
                                // Get Priority Group ID
                                if (pb.category.ToLower() == "a3")
                                {
                                    pb.category = "a3 - immunocompetent";
                                }
                                var GetPriorityGroupId = db.PriorityGroups.FirstOrDefault(s => s.Category.ToLower() == pb.category.ToLower());
                                if (GetPriorityGroupId == null)
                                {
                                    pb.PriorityGroupErrorMessage = "Cannot find Priority Group or Category reference in the database,";
                                }
                                else
                                {
                                    if (GetPriorityGroupId.Category.ToLower().StartsWith("ropp") || pb.category.ToLower().StartsWith("pedia"))
                                    {
                                        pb.GuardianIsRequired = true;
                                    }
                                    else
                                    {
                                        pb.GuardianIsRequired = false;
                                    }
                                }


                                // Region
                                if (pb.region == "")
                                {
                                    pb.RegionErrorMessage = "Region must not empty,";
                                }


                                // Province
                                // Get Province ID
                                var GetProvinceId = db.Provinces.FirstOrDefault(s => s.province_code_excel.ToLower() == pb.province.ToLower());
                                if (GetProvinceId == null)
                                {
                                    pb.ProvinceErrorMessage = "Cannot find province reference in the database,";
                                }


                                // Get City Municipality ID
                                var GetCityMunicipalityId = db.CityMunicipalities.FirstOrDefault(s => s.CityMunicipalityCodeExcel.ToLower() == pb.citymunicipality.ToLower());
                                if (GetCityMunicipalityId == null)
                                {
                                    pb.CityErrorMessage = "Cannot find City reference in the database,";
                                }


                                // Get Barangay ID
                                if (pb.barangay != "")
                                {
                                    var GetBarangayId = db.Barangays.FirstOrDefault(s => s.barangay_name.ToLower() == pb.barangay.ToLower());

                                    if (GetBarangayId != null)
                                    {
                                        pb.BarangayId = GetBarangayId.barangay_id;
                                    }
                                }

                                // Vaccine Manufacturer
                                // Get Vaccine Manufacturer ID
                                var GetVaccineManufacturerId = db.Vaccines.FirstOrDefault(s => s.VaccineBrand.ToLower() == pb.vaccinemanufacturername.ToLower() || s.VaccineManufacturer.ToLower() == pb.vaccinemanufacturername.ToLower());
                                if (GetVaccineManufacturerId == null)
                                {
                                    pb.VaccineManufacturerErrorMessage = "Cannot find Vaccine Manufacturer reference in the database,";
                                }


                                // Batch Number
                                if (pb.batchnumber == "")
                                {
                                    pb.BatchNumberErrorMessage = "Batch Number must not empty,";
                                }


                                // Lot Number
                                if (pb.lotnumber == "")
                                {
                                    pb.LotNumberErrorMessage = "Lot Number must not empty,";
                                }


                                // Bakuna Center CBCB ID
                                if (pb.bakunacentercbcrid == "")
                                {
                                    pb.BakunaCenterCBCRIdErrorMessage = "Bakuna Center CBCR ID must not empty,";
                                }


                                // Vaccinator Name
                                if (pb.vaccinatorname == "")
                                {
                                    pb.VaccinatorNameErrorMessage = "Vaccinator Name must not empty,";
                                }


                                // Dose
                                // Get Dose ID
                                // FIRST DOSE
                                if (pb.firstdose.ToLower().StartsWith("y") && pb.seconddose.ToLower().StartsWith("n") && pb.additionalboosterdose.ToLower().StartsWith("n") && pb.secondadditionalboosterdose.ToLower().StartsWith("n"))
                                {
                                    var GetDoseId = db.Dose.FirstOrDefault(s => s.VaccineDose.ToLower() == "1st dose");
                                    pb.DoseId = GetDoseId.ID;
                                }
                                // SECOND DOSE
                                else if (pb.firstdose.ToLower().StartsWith("n") && pb.seconddose.ToLower().StartsWith("y") && pb.additionalboosterdose.ToLower().StartsWith("n") && pb.secondadditionalboosterdose.ToLower().StartsWith("n"))
                                {
                                    var GetDoseId = db.Dose.FirstOrDefault(s => s.VaccineDose.ToLower() == "2nd dose");
                                    pb.DoseId = GetDoseId.ID;
                                }
                                // ADDITIONAL BOOSTER DOSE
                                else if (pb.firstdose.ToLower().StartsWith("n") && pb.seconddose.ToLower().StartsWith("n") && pb.additionalboosterdose.ToLower().StartsWith("y") && pb.secondadditionalboosterdose.ToLower().StartsWith("n"))
                                {
                                    var GetDoseId = db.Dose.FirstOrDefault(s => s.VaccineDose.ToLower() == "additional/booster dose");
                                    pb.DoseId = GetDoseId.ID;
                                }
                                // SECOND ADDITIONAL BOOSTER DOSE
                                else if (pb.firstdose.ToLower().StartsWith("n") && pb.seconddose.ToLower().StartsWith("n") && pb.additionalboosterdose.ToLower().StartsWith("n") && pb.secondadditionalboosterdose.ToLower().StartsWith("y"))
                                {
                                    var GetDoseId = db.Dose.FirstOrDefault(s => s.VaccineDose.ToLower() == "2nd additional/booster dose");
                                    pb.DoseId = GetDoseId.ID;
                                }
                                else
                                {
                                    pb.DoseErrorMessage = "Cannot find Dose reference in the database,";
                                }


                                // adverse Event
                                // ADVERSE EVENT AND ADVERSE EVENT CONDITION
                                if (pb.adverseevent.ToLower().StartsWith("y"))
                                {
                                    if (pb.adverseeventcondition != "")
                                    {
                                        var GetAdverseID = db.Adverses.SingleOrDefault(s => s.Condition == pb.adverseeventcondition);

                                        if (GetAdverseID != null)
                                        {
                                            pb.AdverseID = GetAdverseID.ID;
                                        }
                                        pb.AdverseEventCheck = true;
                                    }
                                    else
                                    {
                                        pb.AdverseEventErrorMessage = "Make sure that the Adverse Event Condition is not empty if the Adverse Event status is Yes,";
                                        pb.AdverseEventCheck = false;
                                    }
                                }
                                else if (pb.adverseevent.ToLower().StartsWith("n"))
                                {
                                    if (pb.adverseeventcondition == "" || pb.adverseeventcondition.ToLower().StartsWith("n"))
                                    {
                                        pb.AdverseEventCheck = true;
                                    }
                                    else
                                    {
                                        pb.AdverseEventErrorMessage = "Make sure that the Adverse Event status is Yes if the Adverse Event Condition has a content,";
                                        pb.AdverseEventCheck = false;
                                    }
                                }
                                else
                                {
                                    pb.AdverseEventCheck = false;

                                }


                                //BIRTHDATE
                                if (pb.birthdate != "")
                                {
                                    // CHECK AND VALIDATE BIRTH DATE
                                    // check if value is already in date format
                                    if (System.DateTime.TryParse(pb.birthdate, out pb.parsedDate))
                                    {
                                        
                                        // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                        pb.birthdateForQry = pb.birthdate.AsDateTime();

                                        var currentDate = System.DateTime.Now;
                                        var numberOfYears = currentDate.Year - pb.birthdateForQry.Year;
                                        var minDate = currentDate.AddYears(-150);
                                        var maxDate = currentDate;

                                        if (pb.birthdateForQry > maxDate)
                                        {
                                            pb.BirthDateErrorMessage = "Oops! It looks like the selected date is in the future."; //Date cannot be in the future
                                            pb.isValidBirthDate = false;
                                        }

                                        else if (pb.birthdateForQry < minDate)
                                        {
                                            pb.BirthDateErrorMessage = "Oops! It looks like the selected date is too far."; // Date cant be more than 150 years old
                                            pb.isValidBirthDate = false;
                                        }
                                        else if (numberOfYears < 5)
                                        {
                                            pb.BirthDateErrorMessage = "Oops! It looks like age is less than 5 years old."; // Age cant be less than 5 years old
                                            pb.isValidBirthDate = false;
                                        }
                                        else
                                        {
                                            pb.isValidBirthDate = true;
                                        }

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

                                        // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                        pb.birthdateForQry = pb.birthdate.AsDateTime();

                                        var currentDate = System.DateTime.Now;
                                        var numberOfYears = currentDate.Year - pb.birthdateForQry.Year;
                                        var minDate = currentDate.AddYears(-150);
                                        var maxDate = currentDate;

                                        if (pb.birthdateForQry > maxDate)
                                        {
                                            pb.BirthDateErrorMessage = "Oops! It looks like the selected date is in the future."; //Date cannot be in the future
                                            pb.isValidBirthDate = false;
                                        }

                                        else if (pb.birthdateForQry < minDate)
                                        {
                                            pb.BirthDateErrorMessage = "Oops! It looks like the selected date is too far."; // Date cant be more than 150 years old
                                            pb.isValidBirthDate = false;
                                        }
                                        else if (numberOfYears < 5)
                                        {
                                            pb.BirthDateErrorMessage = "Oops! It looks like age is less than 5 years old."; // Age cant be less than 5 years old
                                            pb.isValidBirthDate = false;
                                        }
                                        else
                                        {
                                            pb.isValidBirthDate = true;
                                        }
                                    }
                                    else
                                    {
                                        pb.isValidBirthDate = false;
                                        pb.BirthDateErrorMessage = "Birth Date format is not valid,";
                                    }
                                }
                                else
                                {
                                    pb.isValidBirthDate = false;
                                    pb.BirthDateErrorMessage = "Birth Date must not empty,";
                                }



                                //VACCINATION DATE
                                if (pb.vaccinationdate != "")
                                {
                                    // check if value is already in date format
                                    if (System.DateTime.TryParse(pb.vaccinationdate, out pb.parsedDate))
                                    {
                                        pb.isValidVaccinationDate = true;

                                        // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                        pb.vaccinationdateForQry = pb.vaccinationdate.AsDateTime();
                                    }
                                    // check if value is in double format
                                    else if (double.TryParse(pb.vaccinationdate, out pb.parsedResult))
                                    {
                                        // get value as double
                                        var dataExcelValue = xlworkbook.Worksheet(pb.worksheet).Cell(pb.row, 20).GetDouble();

                                        // convert double value to date time format
                                        pb.vaccinationdate = System.DateTime.FromOADate(dataExcelValue).ToString();

                                        // validate datetime value
                                        pb.isValidVaccinationDate = System.DateTime.TryParseExact(pb.vaccinationdate, pb.dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out pb.parsedDate);

                                        // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                        pb.vaccinationdateForQry = pb.vaccinationdate.AsDateTime();
                                    }
                                    else
                                    {
                                        pb.isValidVaccinationDate = false;
                                        pb.VaccinationDateErrorMessage = "Vaccination Date format is not valid,";
                                    }
                                }
                                else
                                {
                                    pb.isValidVaccinationDate = false;
                                    pb.VaccinationDateErrorMessage = "Vaccination Date must not empty,";
                                }


                                // DEFERRAL AND REASON FOR DEFERRAL
                                if (pb.deferral != "")
                                {
                                    if (pb.deferral.ToLower().StartsWith("y"))
                                    {
                                        if (pb.reasonfordeferral != "")
                                        {
                                            var GetDeferralID = db.Deferrals.SingleOrDefault(s => s.Reason == pb.reasonfordeferral);

                                            if (GetDeferralID != null)
                                            {
                                                pb.DeferralId = GetDeferralID.Id;
                                                pb.DeferralCheck = true;
                                            }
                                            else
                                            {
                                                pb.DeferralErrorMessage = "Cannot find deferral references in the database,";
                                                pb.DeferralCheck = false;
                                            }
                                        }
                                        else
                                        {
                                            pb.DeferralErrorMessage = "Make sure that the Reason for deferral is not empty if the Deferral Status is YES,";
                                            pb.DeferralCheck = false;
                                        }
                                    }
                                    else if (pb.deferral.ToLower().StartsWith("n"))
                                    {
                                        if (pb.reasonfordeferral == "" || pb.reasonfordeferral.ToLower().StartsWith("n"))
                                        {
                                            pb.DeferralCheck = true;
                                        }
                                        else
                                        {
                                            pb.DeferralErrorMessage = "Make sure that the Deferral Status is YES if the Reason for deferral is not empty,";
                                            pb.DeferralCheck = false;
                                        }
                                    }
                                    else
                                    {
                                        pb.DeferralCheck = false;
                                        pb.DeferralErrorMessage = "Deferral status is not in valid format,";
                                    }
                                }
                                else
                                {
                                    pb.DeferralCheck = false;
                                    pb.DeferralErrorMessage = "Deferral status must not empty,";
                                }

                                // CHECK REQUIRED FIELDS IF VALID
                                if (pb.uniquepersonid != "" &&
                                    pb.firstname != "" &&
                                    pb.FirstNameChecker == true &&
                                    pb.lastname != "" &&
                                    pb.LastNameChecker == true &&
                                    pb.GenderCheck == true &&
                                    pb.PWDCheck == true &&
                                    pb.contactnumber != "" &&
                                    pb.ContactNumberChecker == true &&
                                    GetPriorityGroupId != null &&
                                    GetProvinceId != null &&
                                    GetCityMunicipalityId != null &&
                                    //pb.BarangayId > 0 &&
                                    GetVaccineManufacturerId != null &&
                                    pb.batchnumber != "" &&
                                    pb.lotnumber != "" &&
                                    pb.bakunacentercbcrid != "" &&
                                    pb.vaccinatorname != "" &&
                                    pb.DoseId > 0 &&
                                    pb.AdverseEventCheck == true &&
                                    pb.isValidBirthDate == true &&
                                    pb.isValidVaccinationDate == true &&
                                    pb.DeferralCheck == true
                                    )
                                {
                                    // CHECKING FOR DUPLICATES
                                    // PERSON
                                    pb.PersonDuplicateChecker = db.Persons.Where(d =>
                                    d.FirstName.ToLower() == pb.firstname.ToLower() &&
                                    d.MiddleName.ToLower() == pb.middlename.ToLower() &&
                                    d.LastName.ToLower() == pb.lastname.ToLower() &&
                                    d.Gender == pb.gender &&
                                    d.BirthDate == pb.birthdateForQry).Any(); // && d.isPWD == pb.isPWD


                                    // DUPLICATE CONDITION
                                    if (pb.PersonDuplicateChecker == true)
                                    {
                                        pb.PersonDuplicateErrorMessage = "Person information is already exist,";
                                    }
                                    else if (pb.GuardianIsRequired == true && pb.guardianname == "")
                                    {
                                        pb.GuardianNameErrorMessage = "Category/ Priotity Group is in PEDIA/ ROPP, therefore the Guardian Name must not empty,";
                                    }
                                    else
                                    {
                                        Person person = new Person();
                                        person.UniquePersonID = pb.uniquepersonid;
                                        person.FirstName = pb.firstname;

                                        if (pb.MiddleNameChecker == true)
                                        {
                                            person.MiddleName = pb.middlename;
                                        }

                                        person.LastName = pb.lastname;

                                        if (pb.SuffixChecker == true)
                                        {
                                            person.Suffix = pb.suffix;
                                        }

                                        person.ContactNumber = pb.contactnumber;

                                        person.ProvinceID = GetProvinceId.province_id;
                                        person.CityMunicipalityID = GetCityMunicipalityId.city_municipality_id;

                                        if (pb.BarangayId > 0)
                                        {
                                            person.BarangayID = pb.BarangayId;
                                        }


                                        person.CityMunicipalityID = GetCityMunicipalityId.city_municipality_id;
                                        person.ProvinceID = GetProvinceId.province_id;

                                        if (pb.GuardianIsRequired == true)
                                        {
                                            person.GuardianName = pb.guardianname;
                                        }

                                        person.Gender = pb.gender.ToLower();

                                        person.isPWD = pb.isPWD;

                                        if (GetEthnicGroupId != null)
                                        {
                                            person.EthnicGroupID = GetEthnicGroupId.Id;
                                        }

                                        person.BirthDate = pb.birthdateForQry;

                                        db.Persons.Add(person);
                                        db.SaveChanges();
                                    }

                                    // GET PERSON ID AFTER CHECKING
                                    var GetPersonID = db.Persons.FirstOrDefault(s =>
                                    s.FirstName.ToLower() == pb.firstname.ToLower() &&
                                    s.LastName.ToLower() == pb.lastname.ToLower() &&
                                    s.MiddleName.ToLower() == pb.middlename.ToLower() &&
                                    s.Gender == pb.gender &&
                                    s.isPWD == pb.isPWD &&
                                    s.BirthDate == pb.birthdateForQry);

                                    // VACCINATION
                                    // check references if not empty
                                    if (GetPersonID == null)
                                    {
                                        pb.PersonErrorMessage = "Cannot find Person reference in the database,";
                                    }
                                    else
                                    {
                                        // CHECK FOR DUPLICATE VACCINATION RECORD
                                        var VaccinationDuplicateScanner = db.Vaccinations.FirstOrDefault(z => z.PersonID == GetPersonID.ID && z.DoseID == pb.DoseId);

                                        if (VaccinationDuplicateScanner == null)
                                        {
                                            Vaccination vaccination = new Vaccination();
                                            vaccination.PriorityGroupID = GetPriorityGroupId.ID;
                                            vaccination.PersonID = GetPersonID.ID;

                                            if (pb.DeferralId > 0)

                                            {
                                                vaccination.DeferralID = pb.DeferralId;
                                            }

                                            vaccination.VaccinationDate = pb.vaccinationdateForQry.Date;
                                            vaccination.VaccineID = GetVaccineManufacturerId.ID;
                                            vaccination.BatchNumber = pb.batchnumber;
                                            vaccination.LotNumber = pb.lotnumber;
                                            vaccination.BakunaCenterCBCRID = pb.bakunacentercbcrid;
                                            vaccination.VaccinatorName = pb.vaccinatorname;
                                            vaccination.DoseID = pb.DoseId;

                                            if (pb.AdverseID > 0)
                                            {
                                                vaccination.AdverseID = pb.AdverseID;
                                            }
                                            vaccination.Comorbidity = pb.comorbidity;
                                            //vaccination.DateCreate = System.DateTime.Now.Date;
                                            db.Vaccinations.Add(vaccination);
                                            db.SaveChanges();

                                            pb.VaccinationUploadedCounter++;

                                            // Get List of Uploaded 
                                            pb.RowUploadedorDeuplicateFirstNameList.Add(pb.firstname);
                                            pb.RowUploadedorDuplicateMiddleNameList.Add(pb.middlename);
                                            pb.RowUploadedorDuplicateLastNameList.Add(pb.lastname);
                                            pb.RowUploadedorDuplicateBirthdayList.Add(pb.birthdate);
                                            pb.RowUploadedorDuplicateGenderList.Add(pb.gender);
                                            pb.RowUploadedorDuplicatePWDList.Add(pb.pwd);
                                            pb.RowUploadedorDuplicateFirstDoseList.Add(pb.firstdose);
                                            pb.RowUploadedorDuplicateSecondDoseList.Add(pb.seconddose);
                                            pb.RowUploadedorDuplicateAdditionalBoosterDoseList.Add(pb.additionalboosterdose);
                                            pb.RowUploadedorDuplicateSecondAdditionalBoosterDoseList.Add(pb.secondadditionalboosterdose);
                                        }
                                        else
                                        {
                                            pb.VaccinationDuplicateCounter++;

                                            // Get List of Duplicate 
                                            pb.RowUploadedorDeuplicateFirstNameList.Add(pb.firstname);
                                            pb.RowUploadedorDuplicateMiddleNameList.Add(pb.middlename);
                                            pb.RowUploadedorDuplicateLastNameList.Add(pb.lastname);
                                            pb.RowUploadedorDuplicateBirthdayList.Add(pb.birthdate);
                                            pb.RowUploadedorDuplicateGenderList.Add(pb.gender);
                                            pb.RowUploadedorDuplicatePWDList.Add(pb.pwd);
                                            pb.RowUploadedorDuplicateFirstDoseList.Add(pb.firstdose);
                                            pb.RowUploadedorDuplicateSecondDoseList.Add(pb.seconddose);
                                            pb.RowUploadedorDuplicateAdditionalBoosterDoseList.Add(pb.additionalboosterdose);
                                            pb.RowUploadedorDuplicateSecondAdditionalBoosterDoseList.Add(pb.secondadditionalboosterdose);
                                        }
                                    }
                                }
                                else
                                {
                                    pb.VaccinationErrorCounter++;

                                    // GET ERROR LIST
                                    // Get Error Message
                                    pb.ErrorMessagesList.Add(
                                            pb.UniquePersonIdErrorMessage +
                                            pb.FirstNameErrorMessage +
                                            pb.LastNameErrorMessage +
                                            pb.MiddleNameErrorMessage +
                                            pb.GenderErrorMessage +
                                            pb.PWDErrorMessage +
                                            pb.EthnicGroupErrorMessage +
                                            pb.ContactNumberErrorMessage +
                                            pb.PriorityGroupErrorMessage +
                                            pb.GuardianNameErrorMessage +
                                            pb.RegionErrorMessage +
                                            pb.ProvinceErrorMessage +
                                            pb.CityErrorMessage +
                                            pb.VaccineManufacturerErrorMessage +
                                            pb.BatchNumberErrorMessage +
                                            pb.LotNumberErrorMessage +
                                            pb.BakunaCenterCBCRIdErrorMessage +
                                            pb.VaccinatorNameErrorMessage +
                                            pb.DoseErrorMessage +
                                            pb.AdverseEventErrorMessage +
                                            pb.BirthDateErrorMessage +
                                            pb.VaccinationDateErrorMessage +
                                            pb.DeferralErrorMessage //+
                                                                    //pb.BarangayErrorMessage

                                            );

                                    // Row Details
                                    pb.RowErrorFirstNameList.Add(pb.firstname);
                                    pb.RowErrorMiddleNameList.Add(pb.middlename);
                                    pb.RowErrorLastNameList.Add(pb.lastname);

                                    // Saving remarks to excel row
                                    xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 34).Value =
                                        pb.UniquePersonIdErrorMessage +
                                        pb.FirstNameErrorMessage +
                                        pb.LastNameErrorMessage +
                                        pb.MiddleNameErrorMessage +
                                        pb.GenderErrorMessage +
                                        pb.PWDErrorMessage +
                                        pb.EthnicGroupErrorMessage +
                                        pb.ContactNumberErrorMessage +
                                        pb.PriorityGroupErrorMessage +
                                        pb.GuardianNameErrorMessage +
                                        pb.RegionErrorMessage +
                                        pb.ProvinceErrorMessage +
                                        pb.CityErrorMessage +
                                        pb.VaccineManufacturerErrorMessage +
                                        pb.BatchNumberErrorMessage +
                                        pb.LotNumberErrorMessage +
                                        pb.BakunaCenterCBCRIdErrorMessage +
                                        pb.VaccinatorNameErrorMessage +
                                        pb.DoseErrorMessage +
                                        pb.AdverseEventErrorMessage +
                                        pb.BirthDateErrorMessage +
                                        pb.VaccinationDateErrorMessage +
                                        pb.DeferralErrorMessage +
                                        pb.BarangayErrorMessage;


                                    pb.filteredData.Add(xlworkbook.Worksheets.Worksheet(pb.worksheet).Cell(pb.row, 34).Value.ToString());

                                    pb.ErrorRowList.Add(pb.row);

                                }

                                ClearFields();

                                pb.row++;

                                // get total processed rows
                                pb.TotalProcessedExcelRows = pb.row - 2;

                                progress = (pb.TotalProcessedExcelRows * 100) / pb.TotalExcelRows; // (int)((float)pb.TotalProcessedExcelRows / (float)pb.TotalExcelRows * 100.0);


                            }

                            // DELETE UPLOADED OR DUPLICATE ROWS IN EXCEL
                            int lastRow = xlworkbook.Worksheet(pb.worksheet).LastRowUsed().RowNumber(); // worksheet.LastRowUsed().RowNumber();
                            var rowsToDelete = new List<int>();

                            // Iterate over the rows to find the ones with an empty cell in the specified column
                            for (int row = 2; row <= lastRow; row++)
                            {

                                if (xlworkbook.Worksheet(pb.worksheet).Cell(row, 34).IsEmpty())
                                {
                                    rowsToDelete.Add(row);
                                }
                            }

                            // Delete the rows after the iteration is complete
                            foreach (var rowNumber in rowsToDelete.OrderByDescending(r => r))
                            {
                                xlworkbook.Worksheet(pb.worksheet).Row(rowNumber).Delete();
                            }


                            xlworkbook.Save();
                            pb.success = true;

                            var data = new
                            {
                                success = pb.success,
                                vaccineduplicate = pb.VaccinationDuplicateCounter,
                                vaccineerror = pb.VaccinationErrorCounter,
                                vaccineuploaded = pb.VaccinationUploadedCounter,

                                // LIST
                                ERRORDETAILS = pb.ErrorMessagesList,
                                ERRORFIRSTNAMELIST = pb.RowErrorFirstNameList,
                                ERRORLASTNAMELIST = pb.RowErrorLastNameList
                            };

                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = pb.success, message = "Please select an excel file first." });
                    }
                }
                else
                {
                    return Json(new { success = pb.success, message = "Please select worksheet number." });
                }
            }
            else
            {
                return Json(new { success = pb.success, message = "Please select worksheet format." });
            }

            return Json(new { success = false, message = "Excel File encoundered an error during uploading. Either excel rows is more than 10k or incorrect foramt. Please double check the Excel File or contact your administrator." });
        }



        private int CalculateProgress()
        {
            // Calculate the progress percentage here based on your task
            // For demonstration purposes, let's just simulate progress
            Random random = new Random();
            return random.Next(0, 101);
        }




        // FUNCTIONING EXCEL DOWNLOAD
        [HttpGet]
        public ActionResult DownloadExistingExcel(string selectedValue)
        {

            // procedure to restore complete path and filename of existing excel
            pb.GetExcelFileName = TempData["excelfilename"] as string;
            TempData["excelfilename"] = pb.GetExcelFileName;

            pb.GetSaveTargetPath = Server.MapPath("/Upload/" + pb.GetExcelFileName); // complete excel path and filename

            // PROCESS ON HOW TO DELETE EXCEL ROW
            using (XLWorkbook wb = new XLWorkbook(pb.GetSaveTargetPath))
            {
                // PROCESS ON HOW TO DOWNLOAD EXCEL FILE
                using (MemoryStream ms = new MemoryStream())
                {
                    // Save the workbook to the stream
                    wb.SaveAs(ms);

                    // Return the file for download
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", pb.GetExcelFileName);
                }
            }
        }


        [HttpPost]
        public ActionResult ClearFields()
        {
            // Clear Error Messages
            pb.UniquePersonIdErrorMessage = "";
            pb.FirstNameErrorMessage = "";
            pb.LastNameErrorMessage = "";
            pb.MiddleNameErrorMessage = "";
            pb.GenderErrorMessage = "";
            pb.PWDErrorMessage = "";
            pb.EthnicGroupErrorMessage = "";
            pb.ContactNumberErrorMessage = "";
            pb.PriorityGroupErrorMessage = "";
            pb.GuardianNameErrorMessage = "";
            pb.RegionErrorMessage = "";
            pb.ProvinceErrorMessage = "";
            pb.CityErrorMessage = "";
            pb.VaccineManufacturerErrorMessage = "";
            pb.BatchNumberErrorMessage = "";
            pb.LotNumberErrorMessage = "";
            pb.BakunaCenterCBCRIdErrorMessage = "";
            pb.VaccinatorNameErrorMessage = "";
            pb.DoseErrorMessage = "";
            pb.AdverseEventErrorMessage = "";
            pb.BirthDateErrorMessage = "";
            pb.VaccinationDateErrorMessage = "";
            pb.DeferralErrorMessage = "";
            pb.BarangayErrorMessage = "";

            pb.GuardianIsRequired = false;

            return Json(new {success = true});
        }



        public ActionResult Instructions()
        {
            return View();
        }
    }
}