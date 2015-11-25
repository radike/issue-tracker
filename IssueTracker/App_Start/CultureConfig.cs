using IssueTracker.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace IssueTracker.App_Start
{
    public static class CultureConfig
    {
        public static void Application_AcquireRequestState(object sender, EventArgs args)
        {
            var app = (HttpApplication)sender;

            CultureInfo culture = null;

            var cultureCode = app.Context.Request.RequestContext.RouteData.Values["culture"] as string;
            if (cultureCode != null)
            {
                culture = CultureHelper.GetSupportedCulture(cultureCode);
            }
            else
            {
                culture = CultureHelper.GetSupportedCulture(app.Context.Request.UserLanguages);
            }

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}