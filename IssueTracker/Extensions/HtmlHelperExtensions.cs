using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            string classValue = "";

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

            if (currentController == controller && (string.IsNullOrEmpty(action) ? true : currentAction == action))
            {
                classValue = activeClassName;
            }

            return classValue;
        }
    }
}