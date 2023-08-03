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
        public string CompleteFilePath;

        // Excel Parameters
        public int row = 2; 
        public int worksheet = 2;
        public bool isExcelError = false;

        // CHERCKER

        // PERSON
        public int PersonUploadedCounter = 0;

        public bool PersonDuplicatesChecker = false;
        public int PersonDuplicateCounter = 0;

        public bool PersonErrorChecker = false;
        public int PersonErrorCounter = 0;

        // VACCINATION
        public int VaccinationUploadedCounter = 0;

        public bool VaccinationDuplicatesChecker = false;
        public int VaccinationDuplicateCounter = 0;

        public bool VaccinationErrorChecker = false;
        public int VaccinationErrorCounter = 0;



        public bool isMale = false;
        public bool isPWD = false;
        public string Gender { get; set; }

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
        public int DoseId;

        public string adverseevent;
        public string adverseeventcondition;
        public int AdverseID;   

    }
}