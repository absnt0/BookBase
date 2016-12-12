using System.Web.Mvc;

namespace BookBase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
                return PartialView("_HomeIndexPartial");
            else
                return View();
        }
    }
}