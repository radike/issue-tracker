using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using IssueTracker.Data;
using IssueTracker.Data.Contracts.Repository_Interfaces;

namespace IssueTracker.Controllers
{
    [AuthorizeOrErrorPage]
    public class HomeController : Controller
    {
        private IIssueRepository _issueRepo;

        public HomeController(IIssueRepository issueRepository)
        {
            _issueRepo = issueRepository;
        }

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

        public JsonResult AutoCompleteSearch(string query)
        {
            var allIssues = _issueRepo.Fetch()
                .Where(n => n.Active)
                .GroupBy(n => n.Id)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).FirstOrDefault())
                .Include(n => n.Project)
                .OrderByDescending(x => x.Created);

            var result = allIssues.Where(x => (x.Project.Code + x.CodeNumber + ": " + x.Name).ToLower().Contains(query.ToLower())).Select(x => new { x.Id, Title = x.Project.Code + x.CodeNumber + ": " + x.Name }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}