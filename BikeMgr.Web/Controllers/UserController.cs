using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;

namespace BikeMgrWeb.Controllers
{
    public class UserController : Controller
    {
        [ResourceAuthorize("Read", "Generic")]
        public ActionResult Index()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        [Authorize]
        public ActionResult Login()
        {
            return this.Redirect("/");
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}