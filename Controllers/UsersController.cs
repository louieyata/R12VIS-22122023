using R12VIS.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace R12VIS.Controllers
{
    public class UsersController : Controller
    {
        private DbContextR12 db = new DbContextR12();
        private UserDAL userDAL;

        public UsersController()
        {
            userDAL = new UserDAL();
        }

        // GET: Users
        public ActionResult Index()
        {
            List<User> users = userDAL.GetUsers();
            //var users = db.Users.Include(u => u.Role);
            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return to page not foud

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Include(x => x.Role).Where(x => x.ID == id).First();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Title");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                UserDAL userDAL = new UserDAL();
                userDAL.CreateUser(user);
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Title", user.RoleID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Title", user.RoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                userDAL.UpdateUser(user);
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Title", user.RoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] User user)
        {

            User _user = userDAL.AuthenticateUser(user.Email, user.Password);
            bool authenticated = (_user != null);
            if (authenticated)
            {
                Session["User"] = _user;
                Session["userFullName"] = $"{_user.FirstName[0]} {_user.LastName}";
                TempData["ToastMessage"] = "Welcome " + user.FirstName + " " + user.LastName;
                TempData["ToastClass"] = "toast-success"; // CSS class for success toast
            }

            return Json(new { success = authenticated, message = authenticated ? "" : "Invalid Credentials" });

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
