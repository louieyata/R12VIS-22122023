using R12VIS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    public class BarangaysController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: Barangays
        public ActionResult Index()
        {
            var barangays = db.Barangays.Include(b => b.CityMunicipality);
            return View(barangays.ToList());
        }

        // GET: Barangays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barangay barangay = db.Barangays.Find(id);
            if (barangay == null)
            {
                return HttpNotFound();
            }
            return View(barangay);
        }

        // GET: Barangays/Create
        public ActionResult Create()
        {
            ViewBag.city_municipality_id = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName");
            return View();
        }

        // POST: Barangays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "barangay_id,city_municipality_id,barangay_name,province_code,city_municipality_code,barangay_code")] Barangay barangay)
        {
            if (ModelState.IsValid)
            {
                db.Barangays.Add(barangay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.city_municipality_id = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", barangay.city_municipality_id);
            return View(barangay);
        }

        // GET: Barangays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barangay barangay = db.Barangays.Find(id);
            if (barangay == null)
            {
                return HttpNotFound();
            }
            ViewBag.city_municipality_id = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", barangay.city_municipality_id);
            return View(barangay);
        }

        // POST: Barangays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "barangay_id,city_municipality_id,barangay_name,province_code,city_municipality_code,barangay_code")] Barangay barangay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(barangay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.city_municipality_id = new SelectList(db.CityMunicipalities, "city_municipality_id", "CityMunicipalityName", barangay.city_municipality_id);
            return View(barangay);
        }

        // GET: Barangays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barangay barangay = db.Barangays.Find(id);
            if (barangay == null)
            {
                return HttpNotFound();
            }
            return View(barangay);
        }

        // POST: Barangays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Barangay barangay = db.Barangays.Find(id);
            db.Barangays.Remove(barangay);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult GetBarangays(int? cityMunicipality_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var barangays = db.Barangays.Where(x => x.city_municipality_id == cityMunicipality_id).Select(x => new { id = x.barangay_id, name = x.barangay_name }).ToList();
            return Json(barangays.OrderBy(x => x.name), JsonRequestBehavior.AllowGet);
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
