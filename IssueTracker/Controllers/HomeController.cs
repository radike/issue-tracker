using System.Threading;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index", new { Culture = Thread.CurrentThread.CurrentUICulture.ToString() });
        }
    }
}