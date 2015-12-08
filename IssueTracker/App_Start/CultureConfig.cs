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
            var app = (HttpApplication) sender;
            var routeData = app.Context.Request.RequestContext.RouteData;

            CultureInfo culture = null;

            var cultureCode = routeData.Values["culture"] as string;
            if (cultureCode != null)
            {
                if(!CultureHelper.IsSupportedCulture(cultureCode))
                {
                    routeData.Values["culture"] = CultureHelper.GetSupportedCulture(app.Context.Request.UserLanguages);
                    app.Response.RedirectToRoute(routeData.Values);
                }

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