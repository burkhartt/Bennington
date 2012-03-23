using System.Web.Mvc;

namespace Bennington.Cms.PrincipalProvider.Controllers
{
    public class UnauthorizedController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }        
    }
}
