using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using R12VIS.Models;

namespace R12VIS.Controllers
{
    public class CityMunicipalitiesController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: CityMunicipalities
        public ActionResult Index()
        {
            var cityMunicipalities = db.CityMunicipalities.Include(c => c.Province);
            return View(cityMunicipalities.ToList());
        }

        // GET: CityMunicipalities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityMunicipality cityMunicipality = db.CityMunicipalities.Find(id);
            if (cityMunicipality == null)
            {
                return HttpNotFound();
            }
            return View(cityMunicipality);
        }

        // GET: CityMunicipalities/Create
        public ActionResult Create()
        {
            ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name");
            return View();
        }

        // POST: CityMunicipalities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "city_municipality_id,province_id,CityMunicipalityName,ProvinceCode,CityMunicipalityCode,ZipCode,CityMunicipalityCodeExcel")] CityMunicipality cityMunicipality)
        {
            if (ModelState.IsValid)
            {
                db.CityMunicipalities.Add(cityMunicipality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name", cityMunicipality.province_id);
            return View(cityMunicipality);
        }

        // GET: CityMunicipalities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityMunicipality cityMunicipality = db.CityMunicipalities.Find(id);
            if (cityMunicipality == null)
            {
                return HttpNotFound();
            }
            ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name", cityMunicipality.province_id);
            return View(cityMunicipality);
        }

        // POST: CityMunicipalities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "city_municipality_id,province_id,CityMunicipalityName,ProvinceCode,CityMunicipalityCode,ZipCode,CityMunicipalityCodeExcel")] CityMunicipality cityMunicipality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cityMunicipality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.province_id = new SelectList(db.Provinces, "province_id", "province_name", cityMunicipality.province_id);
            return View(cityMunicipality);
        }

        // GET: CityMunicipalities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityMunicipality cityMunicipality = db.CityMunicipalities.Find(id);
            if (cityMunicipality == null)
            {
                return HttpNotFound();
            }
            return View(cityMunicipality);
        }

        // POST: CityMunicipalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CityMunicipality cityMunicipality = db.CityMunicipalities.Find(id);
            db.CityMunicipalities.Remove(cityMunicipality);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ImportMuniCityCodes()
        {
            //CityMunicipality cityMunicipality = db.CityMunicipalities.Find(id);
            //db.CityMunicipalities.Remove(cityMunicipality);
            //db.SaveChanges();
            return View();
        }

        [HttpPost, ActionName("ImportMuniCityCodes")]
        public ActionResult ImportMuniCityCodes(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    file.SaveAs(filePath);

                    // Read the CSV file
                    DataTable dataTable = ReadCSVFile(filePath);

                    // Process the data and store it in the database or any other storage mechanism
                    StoreData(dataTable);

                    ViewBag.Message = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error occurred while uploading the file: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Error = "Please select a file.";
            }

            return View();
        }
        private DataTable ReadCSVFile(string filePath)
        {
            // Read the CSV file into a DataTable
            DataTable dataTable = new DataTable();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }

                while (!reader.EndOfStream)
                {
                    string[] rows = reader.ReadLine().Split(',');

                    if (rows.Length == dataTable.Columns.Count)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow.ItemArray = rows;
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            return dataTable;
        }

        private void StoreData(DataTable dataTable)
        {
            // Here, you can perform your logic to store the data in the database or any other storage mechanism
            // Iterate through the rows of the DataTable and extract the CODE, DESCRIPTION, and MUNICITY values
            // Store the extracted data in your desired storage mechanism (e.g., database table)

            foreach (DataRow row in dataTable.Rows)
            {
                string code = row["CODE"].ToString();
                string description = row["DESCRIPTION"].ToString();
                string municity = row["MUNICITY"].ToString();

                CityMunicipality city = db.CityMunicipalities.Where(x=>x.CityMunicipalityName == description).FirstOrDefault();
                if (city !=null) {
                    city.CityMunicipalityCodeExcel = municity;
                    db.Entry(city).State = EntityState.Modified;
                    db.SaveChanges();
                }


                // Store the data in your desired storage mechanism (e.g., database table)
                // Example:
                // InsertDataIntoDatabase(code, description, municity);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
