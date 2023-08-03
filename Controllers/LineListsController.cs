using R12VIS.Models;
using R12VIS.Models.ViewModel;
using System.Web.Mvc;

namespace R12VIS.Controllers
{

    [UserAuthenticationFilter]

    public class LineListsController : Controller
    {
        private DbContextR12 db = new DbContextR12();

        // GET: LineLists
        public ActionResult Index()
        {
            return View();
        }
    }
}