﻿using IssueTracker.Abstractions;
using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;

namespace IssueTracker.App_Start
{
    public static class CultureConfig
    {
        public static void Application_AcquireRequestState(object sender, EventArgs args)
        {
            var app = (HttpApplication) sender;
            var requestContext = app.Context.Request.RequestContext;
            var cultureCookie = requestContext.HttpContext.Request.Cookies[CultureHelper.PREFFERED_CULTURE_COOKIE];
            var cultureCode = requestContext.RouteData.Values["culture"] as string;

            CultureInfo culture = null;

            if (cultureCode != null)
            {
                CheckLangToken(cultureCode, cultureCookie, app);
                culture = CultureHelper.GetSupportedCulture(cultureCode);
                AppendLocaleCookie(cultureCookie, culture, requestContext);
            }
            else
            {
                var langs = CultureHelper.GetCultureForCookie(cultureCookie) ?? app.Context.Request.UserLanguages;
                culture = CultureHelper.GetSupportedCulture(langs);
                AppendLocaleCookie(cultureCookie, culture, requestContext);
            }

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private static void CheckLangToken(string cultureCode, HttpCookie cultureCookie, HttpApplication app)
        {
            if (CultureHelper.IsSupportedCulture(cultureCode)) return;

            var routeData = app.Context.Request.RequestContext.RouteData.Values;
            var langs = CultureHelper.GetCultureForCookie(cultureCookie) ?? app.Context.Request.UserLanguages;
            routeData["culture"] = CultureHelper.GetSupportedCulture(langs);
            app.Response.RedirectToRoute(routeData);
        }

        private static void AppendLocaleCookie(HttpCookie cookie, CultureInfo culture, System.Web.Routing.RequestContext requestContext)
        {
            if (cookie == null)
            {
                cookie = new HttpCookie(CultureHelper.PREFFERED_CULTURE_COOKIE, culture.Name)
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