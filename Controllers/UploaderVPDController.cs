using ClosedXML.Excel;
using R12VIS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace R12VIS.Controllers
{
    //[UserAuthenticationFilter]

    public class UploaderVPDController : Controller
    {
        DbMRSpecialVacsEntities vpddb = new DbMRSpecialVacsEntities();
        DbContextR12 db = new DbContextR12();
        PublicVariablesVPD pb = new PublicVariablesVPD();

        public string[] dateFormats = {
                            "yyyy-MM-dd",

                            "M/dd/y",
                            "M/dd/yy",
                            "M/dd/yyy",
                            "M/dd/yyyy",

                            "MM/dd/y",
                            "MM/dd/yy",
                            "MM/dd/yyy",
                            "MM/dd/yyyy",

                            "MMM/dd/y",
                            "MMM/dd/yy",
                            "MMM/dd/yyy",
                            "MMM/dd/yyyy",

                            "MMMM/dd/y",
                            "MMMM/dd/yy",
                            "MMMM/dd/yyy",
                            "MMMM/dd/yyyy",

                            "dd/MM/yyyy",
                            "dddd, MMMM dd, yyyy",
                            "MMMM dd, yyyy",
                            "dddd, dd MMMM yyyy",
                            "dd MMMM yyyy",
                            "dddd, dd MMMM yyyy HH:mm:ss",
                            "MMMM dd, yyyy HH:mm:ss",
                            "MM/dd/yyyy h:mm:ss tt"
                        };


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
            return Regex.IsMatch(input, @"^[a-zA-ZñÑ\- .']+$");
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
            TempData["excelfilenameVPD"] = "MRSIATemplate.xlsx";


            pb.GetExcelFileName = TempData["excelfilenameVPD"] as string;
            TempData["excelfilenameVPD"] = pb.GetExcelFileName;

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

        #region uploader new code

        [HttpPost]
        public JsonResult UploadExcel(HttpPostedFileBase file, int vaccination_type)
        {
            var result = new JsonResult();
            result.MaxJsonLength = int.MaxValue;

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    var datetotstring = DateTime.Now.ToString("MM dd yyyy");

                    // Get the file name
                    string fileName = Path.GetFileName(file.FileName);
                    string buttonText = "Okay";
                    fileName = datetotstring.Replace(" ", "") + fileName;

                    pb.GetExcelFileName = fileName;
                    TempData["excelfilenameVPD"] = fileName;

                    // Specify the path where you want to save the uploaded file
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);

                    // Save the file
                    file.SaveAs(path);

                    // Process the Excel file
                    string resultMessage = ProcessExcelFile(path, vaccination_type);
                    var icon = "success";

                    if (resultMessage != "File uploaded successfully")
                    {
                        icon = "error";
                        buttonText = "Download";
                    }

                    result.Data = new { isSuccess = true, message = resultMessage, icon = icon, buttonText = buttonText, fileName = fileName };
                }
                catch (Exception ex)
                {
                    result.Data = new { isSuccess = false, message = "Error: " + ex.Message };
                }
            }
            else
            {
                result.Data = new { isSuccess = false, message = "Please select a file" };
            }

            return result;
        }


        private string ProcessExcelFile(string filePath, int vaccination_type)
        {
            bool isValid = true;
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(3); // Assuming data is in the third sheet

                // Assuming the data structure is simple, adjust as per your Excel format
                var rows = worksheet.RowsUsed().Skip(3); // Skip the first and second rows
                //var datetotstring = DateTime.Now.ToString("MM dd yyyy"); 

                worksheet.Cell(3, 23).Value = "Error Message";

                foreach (var row in rows)
                {
                    // For checking
                    string error_message = "";

                    int person_region_address = 1;
                    int person_province_address = 0;
                    int person_city_municipality_address = 0;
                    int person_barangay_address = 0;
                    DateTime birthdateForQry = new DateTime();
                    double parsedResult;

                    int vaccination_region_address = 1;
                    int vaccination_province_address = 0;
                    int vaccination_city_municipality_address = 0;
                    int vaccination_barangay_address = 0;
                    int vaccination_vaccine_id = 0;
                    DateTime parsedDate;
                    DateTime vaccinationdateForQry = new DateTime();

                    // Access each cell in the row by index
                    var ethnicgroup = row.Cell(1).Value.ToString();
                    var lastname = row.Cell(2).Value.ToString();
                    var firstname = row.Cell(3).Value.ToString();
                    var middlename = row.Cell(4).Value.ToString();
                    var suffix = row.Cell(5).Value.ToString();
                    var birthdate = row.Cell(6).Value.ToString();
                    var sex = row.Cell(7).Value.ToString();
                    var address = row.Cell(8).Value.ToString();
                    var addressRegion = row.Cell(9).Value.ToString();
                    var addressProvince = row.Cell(10).Value.ToString();
                    var addressCityMunicipality = row.Cell(11).Value.ToString();
                    var addressBarangay = row.Cell(12).Value.ToString();

                    var vaccinationRegion = row.Cell(13).Value.ToString();
                    var vaccinationProvince = row.Cell(14).Value.ToString();
                    var vaccinationCityMunicipality = row.Cell(15).Value.ToString();
                    var vaccinationBarangay = row.Cell(16).Value.ToString();
                    var vaccinationVaccine = row.Cell(17).Value.ToString();
                    vaccinationVaccine = vaccinationVaccine.Replace("_03", "");
                    vaccinationVaccine = vaccinationVaccine.Replace("_07", "");
                    vaccinationVaccine = vaccinationVaccine.Replace("_12", "");
                    var vaccinationActionTaken = row.Cell(18).Value.ToString();
                    var vaccinationDate = row.Cell(19).Value.ToString();
                    var vaccinationVaccinatorName = row.Cell(22).Value.ToString();

                    if (vaccinationActionTaken != "")
                    {
                        if (vaccinationActionTaken.ToLower() == "vaccinate")
                        {
                            // Unique Person ID
                            //if (ethnicgroup == "")
                            //{
                            //    error_message = "Ethnic Group required,";
                            //}

                            // First Name
                            if (firstname != "")
                            {
                                //if (!ContainsOnlyLetters(firstname))
                                //{
                                //    error_message += "First Name is invalid,";
                                //    isValid = false;
                                //}
                            }
                            else
                            {
                                error_message = "First Name required,";
                                isValid = false;
                            }

                            // Middle Name
                            if (middlename != "")
                            {
                                //if (!ContainsOnlyLetters(middlename))
                                //{
                                //    error_message += "Middle Name is invalid,";
                                //    isValid = false;
                                //}
                            }

                            // Last Name
                            if (lastname != "")
                            {
                                //if (!ContainsOnlyLetters(lastname))
                                //{
                                //    error_message += "Last Name is invalid,";
                                //    isValid = false;
                                //}
                            }
                            else
                            {
                                error_message += "Last Name required,";
                                isValid = false;
                            }

                            // Suffix
                            if (suffix != "" && !suffix.Equals("N/A"))
                            {
                                if (!ContainsOnlyLetters(suffix))
                                {
                                    error_message += "Suffix is invalid,";
                                    isValid = false;
                                }
                            }

                            //BIRTHDATE
                            if (birthdate != "")
                            {
                                // CHECK AND VALIDATE BIRTH DATE
                                // check if value is already in date format
                                if (System.DateTime.TryParse(birthdate, out birthdateForQry))
                                {

                                    // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                    birthdateForQry = birthdate.AsDateTime();

                                    var currentDate = System.DateTime.Now;
                                    var numberOfYears = currentDate.Year - birthdateForQry.Year;
                                    var minDate = currentDate.AddYears(-150);
                                    var maxDate = currentDate;

                                    if (birthdateForQry > maxDate)
                                    {
                                        error_message += "Selected date invalid."; //Date cannot be in the future
                                        isValid = false;
                                    }

                                    else if (birthdateForQry < minDate)
                                    {
                                        error_message += "Selected date invalid."; // Date cant be more than 150 years old
                                        isValid = false;
                                    }
                                    else if (numberOfYears > 5)
                                    {
                                        error_message += "Age must be 5 years old below."; // Age cant be less than 5 years old
                                        isValid = false;
                                    }

                                }
                                // chaeck if value is in double format
                                else if (double.TryParse(birthdate, out parsedResult))
                                {
                                    // get value as double
                                    var dataExcelValue = ((double)row.Cell(22).Value);

                                    // convert double value to date time format
                                    birthdate = System.DateTime.FromOADate(dataExcelValue).ToString();

                                    // validate datetime value
                                    var isValidBirthDate = System.DateTime.TryParseExact(birthdate, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out pb.parsedDate);

                                    // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                    birthdateForQry = birthdate.AsDateTime();

                                    var currentDate = System.DateTime.Now;
                                    var numberOfYears = currentDate.Year - pb.birthdateForQry.Year;
                                    var minDate = currentDate.AddYears(-150);
                                    var maxDate = currentDate;

                                    if (birthdateForQry > maxDate)
                                    {
                                        error_message += "Selected date invalid."; //Date cannot be in the future
                                        isValid = false;
                                    }

                                    else if (birthdateForQry < minDate)
                                    {
                                        error_message += "Selected date invalid."; // Date cant be more than 150 years old
                                        isValid = false;
                                    }
                                    else if (numberOfYears > 5)
                                    {
                                        error_message += "Age must be 5 years old below."; // Age cant be less than 5 years old
                                        isValid = false;
                                    }
                                }
                                else
                                {
                                    error_message += "Birth Date format invalid,";
                                    isValid = false;
                                }
                            }
                            else
                            {
                                error_message += "Birth Date required,";
                                isValid = false;
                            }

                            // Gender
                            // CHECK GENDER
                            if (sex.ToLower().StartsWith("m"))
                                sex = "m";
                            else if (sex.ToLower().StartsWith("f"))
                                sex = "f";
                            else
                            {
                                error_message += "Sex is invalid,";
                                isValid = false;
                            }

                            // Province
                            var GetProvinceId = db.Provinces.FirstOrDefault(s => s.mrsia_province_code.ToLower() == addressProvince.ToLower());
                            if (GetProvinceId == null)
                            {
                                error_message += "Province invalid,";
                                isValid = false;
                            }
                            else
                                person_province_address = GetProvinceId.province_id;

                            // Get City Municipality ID
                            var GetCityMunicipalityId = db.CityMunicipalities.FirstOrDefault(s => s.mrsia_city_municipality_code.ToLower() == addressCityMunicipality.ToLower());
                            if (GetCityMunicipalityId == null)
                            {
                                error_message += "City invalid,";
                                isValid = false;
                            }
                            else
                                person_city_municipality_address = GetCityMunicipalityId.city_municipality_id;

                            // Get Barangay ID
                            var GetBarangayId = db.Barangays.FirstOrDefault(s => s.mrsia_barangay_code.ToLower() == addressBarangay.ToLower());
                            if (GetBarangayId == null)
                            {
                                error_message += "Barangay invalid,";
                                isValid = false;
                            }
                            else
                                person_barangay_address = GetBarangayId.barangay_id;

                            // Province
                            var GetVaccinationProvinceId = db.Provinces.FirstOrDefault(s => s.mrsia_province_code.ToLower() == vaccinationProvince.ToLower());
                            if (GetVaccinationProvinceId == null)
                            {
                                error_message += "Province vaccination invalid,";
                                isValid = false;
                            }
                            else
                                vaccination_province_address = GetVaccinationProvinceId.province_id;

                            // Get City Municipality ID
                            var GetVaccinationCityMunicipalityId = db.CityMunicipalities.FirstOrDefault(s => s.mrsia_city_municipality_code.ToLower() == vaccinationCityMunicipality.ToLower());
                            if (GetVaccinationCityMunicipalityId == null)
                            {
                                error_message += "City vaccination invalid,";
                                isValid = false;
                            }
                            else
                                vaccination_city_municipality_address = GetVaccinationCityMunicipalityId.city_municipality_id;

                            // Get Barangay ID
                            var GetVaccinationBarangayId = db.Barangays.FirstOrDefault(s => s.mrsia_barangay_code.ToLower() == vaccinationBarangay.ToLower());
                            if (GetVaccinationBarangayId == null)
                            {
                                error_message += "Barangay vaccination invalid,";
                                isValid = false;
                            }
                            else
                                vaccination_barangay_address = GetVaccinationBarangayId.barangay_id;

                            // Vaccine
                            var GetVaccineId = vpddb.Vaccine_VPD.Where(a => a.vaccine_description.Equals(vaccinationVaccine)).FirstOrDefault();
                            if (GetVaccineId == null)
                            {
                                error_message += "Vaccine name invalid,";
                                isValid = false;
                            }
                            else
                                vaccination_vaccine_id = GetVaccineId.vaccine_id;

                            //VACCINATION DATE
                            if (vaccinationDate != "")
                            {
                                // check if value is already in date format
                                if (System.DateTime.TryParse(vaccinationDate, out parsedDate))
                                {
                                    // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                    vaccinationdateForQry = vaccinationDate.AsDateTime();
                                }
                                // check if value is in double format
                                else if (double.TryParse(vaccinationDate, out parsedResult))
                                {
                                    var dataExcelValue = ((double)row.Cell(19).Value);

                                    // convert double value to date time format
                                    vaccinationDate = System.DateTime.FromOADate(dataExcelValue).ToString();

                                    // validate datetime value
                                    var isValidVaccinationDate = System.DateTime.TryParseExact(pb.vaccinationDate, pb.dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out pb.parsedDate);

                                    // PASS DATE TO DATETIME VARIABLE FOR QUERY
                                    vaccinationdateForQry = pb.vaccinationDate.AsDateTime();
                                }
                                else
                                {
                                    error_message += "Vaccination Date format invalid,";
                                    isValid = false;
                                }
                            }
                            else
                            {
                                error_message += "Vaccination Date required,";
                                isValid = false;
                            }

                            // Vaccinator Name
                            //if (vaccinationVaccinatorName == "")
                            //{
                            //    error_message += "Vaccinator Name invalid,";
                            //    isValid = false;
                            //}

                            if (birthdateForQry > vaccinationdateForQry)
                            {
                                error_message = "Vaccination date should not be before birthdate";
                                isValid = false;
                            }
                        }
                        else
                            isValid = false;
                    }

                    else
                    {
                        isValid = false;
                    }

                    // CHECK REQUIRED FIELDS IF VALID
                    if (isValid)
                    {
                        // CHECKING FOR DUPLICATES
                        // PERSON
                        Person_VPD person = vpddb.Person_VPD.Where(d =>
                        d.person_first_name.ToLower() == firstname.ToLower() &&
                        d.person_last_name.ToLower() == lastname.ToLower() &&
                        d.person_suffix.ToLower() == suffix.ToLower() &&
                        d.person_sex == sex &&
                        d.person_birth_date == birthdateForQry).FirstOrDefault(); // && d.isPWD == pb.isPWD

                        // DUPLICATE CONDITION
                        if (person == null)
                        {
                            person = new Person_VPD();

                            // Nullable values
                            // Not on template, only true or false
                            //person.person_ethnic_group_id = int.TryParse(data["ethnic_group_list"]?.ToString(), out ethnicGroupId) ? ethnicGroupId : (int?)null;
                            person.person_suffix = suffix;
                            person.person_middle_name = middlename;

                            // Not nullable values
                            person.person_sex = sex;
                            person.person_first_name = firstname;
                            person.person_last_name = lastname;
                            person.person_is_pwd = false; // Not on template
                            person.person_sibling_rank = 1; // Not on template

                            person.person_region_address = person_region_address; // Region XII as default
                            person.person_province_address = person_province_address;
                            person.person_city_municipality_address = person_city_municipality_address;
                            person.person_barangay_address = person_barangay_address;
                            //person.person_religion_id = data["religion_list"]; Not on template
                            //person.person_parent_educational_attainment_id = data["educational_attainment_list"]; Not on template
                            //person.person_parent_income_class_id = data["income_class_list"]; Not on template
                            //person.person_parent_occupation_id = data["mrsia_occupation_list"]; Not on template
                            person.person_birth_date = birthdateForQry;
                            vpddb.Person_VPD.Add(person);
                        }
                        // CHECK FOR DUPLICATE VACCINATION RECORD
                        var VaccinationDuplicateScanner = vpddb.Vaccination_VPD.FirstOrDefault(z => z.vaccination_person_id == person.person_id && z.vaccination_vaccine_id == vaccination_vaccine_id);

                        if (VaccinationDuplicateScanner == null)
                        {
                            Vaccination_VPD vaccination = new Vaccination_VPD();

                            vaccination.vaccination_person_id = person.person_id;
                            vaccination.vaccination_region_id = vaccination_region_address;
                            vaccination.vaccination_province_id = vaccination_province_address;
                            vaccination.vaccination_city_municipality_id = vaccination_city_municipality_address;
                            vaccination.vaccination_barangay_id = vaccination_barangay_address;
                            vaccination.vaccination_vaccinator = vaccinationVaccinatorName;
                            vaccination.vaccination_vaccine_id = vaccination_vaccine_id;
                            vaccination.vaccination_date = vaccinationdateForQry;
                            vaccination.vaccination_type_id = vaccination_type;

                            vpddb.Vaccination_VPD.Add(vaccination);
                            vpddb.SaveChanges();
                        }
                        else
                        {
                            error_message = "Person with this vaccine already exist";
                        }
                    }
                    // Saving remarks to excel row

                    row.Cell(23).Value = error_message;

                    error_message = ""; // ClearFields(); function to remove all error messages
                }
                // DELETE UPLOADED OR DUPLICATE ROWS IN EXCEL
                int lastRow = worksheet.LastRowUsed().RowNumber();
                var rowsToDelete = new List<int>();

                // Iterate over the rows to find the ones with an empty cell in the specified column
                for (int i = 4; i <= lastRow; i++)
                {
                    if (worksheet.Row(i).Cell(23).IsEmpty())
                    {
                        rowsToDelete.Add(i);
                    }
                }

                // Delete the rows after the iteration is complete
                foreach (var rowNumber in rowsToDelete.OrderByDescending(r => r))
                {
                    worksheet.Row(rowNumber).Delete();
                }
                // Get the desktop directory
                //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Specify the path where you want to save the modified workbook
                //string modifiedFilePath = Path.Combine(desktopPath, "Unsuccessful uploads.xlsx");

                // Save the modified workbook
                //workbook.SaveAs(modifiedFilePath);
                workbook.Save();

                // Save the error workbook to a memory stream (if needed)
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    workbook.SaveAs(ms);
                //    ms.ToArray();
                //}
            }
            return isValid ? "File uploaded successfully" : "There are errors upon uploading, please click download button to save this file";
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
            pb.GetExcelFileName = TempData["excelfilenameVPD"] as string;
            TempData["excelfilenameVPD"] = pb.GetExcelFileName;

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

        #endregion uploader new code

        public ActionResult Instructions()
        {
            return View();
        }
    }
}