using ClosedXML.Excel;
using R12VIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    public class UploaderController : Controller
    {
        private DbContextR12 db = new DbContextR12();
        string fileName;
        int row = 2;
        int worksheet = 2;
        int Uploaded = 0;
        int Duplicate = 0;
        int Error = 0;

        bool duplicateschecker = false;
        bool errorchecker = false;

        int percentage = 0;


        // GET: Uploader
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Checker(HttpPostedFileBase myExcelData)
        {
            if (myExcelData != null)
            {
                if (myExcelData.ContentLength > 0) // check if the file uploaded
                {
                    string filePath = "D:\\FPRIII\\SYSTEMS\\VS2022\\WEB\\MVC\\DOH-ExcelUploader\\DOH-ExcelUploader\\Upload";
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    filePath = filePath + fileName + ".xlsx";
                    myExcelData.SaveAs(filePath);
                    XLWorkbook xlworkbook = new XLWorkbook(filePath);

                    // Get Category ID (Priority Group Table)

                    // 



                    // loop excel rows and get data on each cells
                    while (xlworkbook.Worksheets.Worksheet(worksheet).Cell(row, 1).GetString() != "")
                    {
                        string category = xlworkbook.Worksheets.Worksheet(worksheet).Cell(row, 1).GetString();

                        row++;
                    }



                    var data = new
                    {
                        TotalRws = row - 2
                    };

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Create an anonymous object or a custom class to hold the data values
                    var error = new
                    {
                        Value1 = "Upload Error!"
                    };

                    return Json(error, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // Create an anonymous object or a custom class to hold the data values
                var error = new
                {
                    Value1 = "Upload Error!"
                };

                return Json(error, JsonRequestBehavior.AllowGet);
            }

        }
    }
}