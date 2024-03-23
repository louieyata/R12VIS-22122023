using R12VIS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    public class AdversesController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: Adverses
        public ActionResult Index()
        {
            return View(db.Adverses.ToList());
        }

        // GET: Adverses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adverse adverse = db.Adverses.Find(id);
            if (adverse == null)
            {
                return HttpNotFound();
            }
            return View(adverse);
        }

        // GET: Adverses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adverses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Event,Condition")] Adverse adverse)
        {
            if (ModelState.IsValid)
            {
                db.Adverses.Add(adverse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adverse);
        }

        // GET: Adverses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adverse adverse = db.Adverses.Find(id);
            if (adverse == null)
            {
                return HttpNotFound();
            }
            return View(adverse);
        }

        // POST: Adverses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Event,Condition")] Adverse adverse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adverse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adverse);
        }

        // GET: Adverses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adverse adverse = db.Adverses.Find(id);
            if (adverse == null)
            {
                return HttpNotFound();
            }
            return View(adverse);
        }

        // POST: Adverses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adverse adverse = db.Adverses.Find(id);
            db.Adverses.Remove(adverse);
            db.SaveChanges();
            return RedirectToAction("Index");
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
