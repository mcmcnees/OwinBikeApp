using System.Web.Mvc;

namespace BikeMgrWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //return View((User as ClaimsPrincipal).Claims);
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Cookies()
        {
            return View();
        }
    }
}