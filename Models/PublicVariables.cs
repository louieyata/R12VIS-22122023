using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;



namespace R12VIS.Models
{
    public class PublicVariables
    {
        // Excel Uploader Path
        //public string filePath = "D:\\FPRIII\\SYSTEMS\\VS2022\\WEB\\MVC\\R12VIS\\Upload\\";
        public string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        public string GetExcelFileName;
        public string ExcelNewFileName;
        public string CompleteFilePathAndFileName;
        public string GetSaveTargetPath;

        // Excel Parameters
        public int row = 2;
        public int worksheet = 0;

        public List<int> ErrorRowList = new List<int>();
        //public int ExcelErrorRowNumber = 1;

        // GET LIST OF UPLOADED ROWS FOR DELETION IN EXCEL FILE
        public List<string> RowUploadedorDeuplicateFirstNameList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateMiddleNameList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateLastNameList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateBirthdayList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateGenderList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicatePWDList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateFirstDoseList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateSecondDoseList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateAdditionalBoosterDoseList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateSecondAdditionalBoosterDoseList = new List<string>(); // to delete
        public List<string> RowUploadedorDuplicateThirdAdditionalBoosterDoseList = new List<string>(); // to delete

        // GET LIST OF ERROR ROWS AND ADD REMARKS 
        // LIST OF ERROR MESSAGES
        public List<string> ErrorMessagesList = new List<string>();
        public List<string> filteredData = new List<string>();

        // LIST OF ROW DETAILS
        public List<string> RowErrorFirstNameList = new List<string>();
        public List<string> RowErrorMiddleNameList = new List<string>();
        public List<string> RowErrorLastNameList = new List<string>();


        // CHERCKER
        public bool GenderCheck = false;
        public bool PWDCheck = false;
        public bool AdverseEventCheck = false;
        public bool DeferralCheck = false;
        public bool GuardianIsRequired = false;

        public bool FirstNameChecker = false;
        public bool LastNameChecker = false;
        public bool MiddleNameChecker = false;
        public bool SuffixChecker = false;
        public bool ContactNumberChecker = false;


        public bool PersonCheck = false;
        public string PersonErrorMessage;

        // DUPLICATES
        public bool PersonDuplicateChecker = false;
        public string PersonDuplicateErrorMessage;


        // ERROR MESSAGES
        //public bool RequiredFieldsChecker = false;
        //public string RequiredFieldsCheckerErrorMessage;

        public string UniquePersonIdErrorMessage;
        public string FirstNameErrorMessage;
        public string LastNameErrorMessage;
        public string MiddleNameErrorMessage;
        public string GenderErrorMessage;
        public string PWDErrorMessage;
        public string EthnicGroupErrorMessage;
        public string ContactNumberErrorMessage;
        public string PriorityGroupErrorMessage;
        public string GuardianNameErrorMessage;
        public string RegionErrorMessage;
        public string ProvinceErrorMessage;
        public string CityErrorMessage;
        public string VaccineManufacturerErrorMessage;
        public string BatchNumberErrorMessage;
        public string LotNumberErrorMessage;
        public string BakunaCenterCBCRIdErrorMessage;
        public string VaccinatorNameErrorMessage;
        public string DoseErrorMessage;
        public string AdverseEventErrorMessage;
        public string BirthDateErrorMessage;
        public string VaccinationDateErrorMessage;
        public string DeferralErrorMessage;
        public string RowHash;
        public int SheetCount;
        public int TotalExcelRows = 0;
        public int TotalProcessedExcelRows = 0;
        public string BarangayErrorMessage;
        public string SuffixErrorMessage;


        public bool success = false;


        // VACCINATION
        public int VaccinationUploadedCounter = 0;

        public bool VaccinationDuplicatesChecker = false;
        public int VaccinationDuplicateCounter = 0;

        public bool VaccinationErrorChecker = false;
        public int VaccinationErrorCounter = 0;



        //public bool isMale = false;
        public bool isPWD = false;
        //public string Gender { get; set; }

        // for date validator
        public bool isValidBirthDate = false;
        public bool isValidVaccinationDate = false;
        public double parsedResult;
        public DateTime parsedDate;

        // date Validator
        // Add more formats as needed
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

        // Excel Fields
        public string category;
        public string comorbidity;
        public string uniquepersonid;
        public string pwd;
        public string ethnicgroup; // Indigenous Member
        public string lastname;
        public string firstname;
        public string middlename;
        public string suffix;
        public string contactnumber;
        public string guardianname;
        public string region;

        public string province;

        public string citymunicipality;

        public string barangay;
        public int BarangayId;

        public string gender;

        public string birthdate;
        public DateTime birthdateForQry;

        public string deferral;
        public string reasonfordeferral;
        public int DeferralId;

        public string vaccinationdate;
        public DateTime vaccinationdateForQry;

        public string vaccinemanufacturername;
        public string batchnumber;
        public string lotnumber;
        public string bakunacentercbcrid;
        public string vaccinatorname;

        public string firstdose;
        public string seconddose;
        public string additionalboosterdose;
        public string secondadditionalboosterdose;
        public int DoseId;

        public string adverseevent;
        public string adverseeventcondition;
        public int AdverseID;
    }
}