using System;
using System.Globalization;
using System.Threading;
using System.Web;
using IssueTracker.Abstractions;

namespace IssueTracker
{
    public static class CultureConfig
    {
        public static void Application_AcquireRequestState(object sender, EventArgs args)
        {
            var app = (HttpApplication) sender;
            var requestContext = app.Context.Request.RequestContext;
            var cultureCookie = requestContext.HttpContext.Request.Cookies[CultureHelper.PrefferedCultureCookie];
            var cultureCode = requestContext.RouteData.Values["culture"] as string;

            CultureInfo culture;

            if (cultureCode != null)
            {
                checkLangToken(cultureCode, cultureCookie, app);
                culture = CultureHelper.GetSupportedCulture(cultureCode);
                appendLocaleCookie(cultureCookie, culture, requestContext);
            }
            else
            {
                var langs = CultureHelper.GetCultureForCookie(cultureCookie) ?? app.Context.Request.UserLanguages;
                culture = CultureHelper.GetSupportedCulture(langs);
                appendLocaleCookie(cultureCookie, culture, requestContext);
            }

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private static void checkLangToken(string cultureCode, HttpCookie cultureCookie, HttpApplication app)
        {
            if (CultureHelper.IsSupportedCulture(cultureCode)) return;

            var routeData = app.Context.Request.RequestContext.RouteData.Values;
            var langs = CultureHelper.GetCultureForCookie(cultureCookie) ?? app.Context.Request.UserLanguages;
            routeData["culture"] = CultureHelper.GetSupportedCulture(langs);
            app.Response.RedirectToRoute(routeData);
        }

        private static void appendLocaleCookie(HttpCookie cookie, CultureInfo culture, System.Web.Routing.RequestContext requestContext)
        {
            if (cookie == null)
            {
                cookie = new HttpCookie(CultureHelper.PrefferedCultureCookie, culture.Name)
                {
                    Expires = DateTime.MaxValue
                };
                requestContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else if (cookie.Value != culture.Name)
            {
                cookie.Value = culture.Name;
                cookie.Expires = DateTime.MaxValue;
                requestContext.HttpContext.Response.Cookies.Set(cookie);
            }
        }

    }
}