using IssueTracker.Abstractions;
using System.Web.Mvc;

namespace IssueTracker
{
    public class CultureFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var nativeName = CultureHelper.CurrentCulture.NativeName;
            var nativeNameSplit = nativeName.Split('(');
            filterContext.Controller.ViewBag.LanguageNativeName = nativeNameSplit[0];
        }
    }
}