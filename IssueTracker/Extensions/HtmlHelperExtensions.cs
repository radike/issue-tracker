using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace IssueTracker.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string ActivePage(this HtmlHelper helper, string controller, string activeClassName = "active")
        {
            return helper.ActivePage(controller, null, activeClassName);
        }

        public static string ActivePage(this HtmlHelper helper, string controller, string action, string activeClassName = "active")
        {
            if (activeClassName == null) throw new ArgumentNullException(nameof(activeClassName));
            return helper.ActivePage(controller, action, null, activeClassName);
        }

        public static string ActivePage(this HtmlHelper helper, string controller, string action, string id, string activeClassName = "active")
        {
            if (activeClassName == null) throw new ArgumentNullException(nameof(activeClassName));
            string classValue = null;

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();
            ValueProviderResult currentId = helper.ViewContext.Controller.ValueProvider.GetValue("id");
            string currentIdValue = currentId?.RawValue.ToString();

            if (currentController == controller && (string.IsNullOrEmpty(action) || currentAction == action) && (string.IsNullOrEmpty(id) || currentIdValue == id))
            {
                classValue = activeClassName;
            }

            return classValue;
        }

        public static MvcHtmlString DisplayEnum(this HtmlHelper helper, Enum e)
        {
            string result = string.Empty;

            var display = e.GetType()
                       .GetMember(e.ToString()).First()
                       .GetCustomAttributes(false)
                       .OfType<DisplayAttribute>()
                       .LastOrDefault();

            if (display != null)
            {
                result = display.GetName();
            }

            return helper.DisplayName(result);
        }

    }
}