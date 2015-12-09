using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IssueTracker.Abstractions
{
    public class CultureHelper
    {
        public static string PREFFERED_CULTURE_COOKIE = "locale";
        public static CultureInfo DEFAULT_CULTURE = new CultureInfo("en-us", false);

        public static IReadOnlyCollection<CultureInfo> SupportedCultures = new List<CultureInfo>() {
            DEFAULT_CULTURE,
            new CultureInfo("cs-cz", false)
        };

        public static CultureInfo GetSupportedCulture(params string[] cultureCodes)
        {
            if (cultureCodes == null)
            {
                return DEFAULT_CULTURE;
            }

            foreach (var code in cultureCodes)
            {
                var requestedCulture = ParseCulture(code);

                if (requestedCulture == null)
                {
                    continue;
                }

                var match = SupportedCultures.SingleOrDefault(c => c.Equals(requestedCulture));
                match = match ?? SupportedCultures.SingleOrDefault(c => c.TwoLetterISOLanguageName == requestedCulture.TwoLetterISOLanguageName);

                if (match != null)
                {
                    return match;
                }
            }

            return DEFAULT_CULTURE;
        }

        public static CultureInfo CurrentCulture
        {
            get {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
        }

        public static bool IsSupportedCulture(string cultureCode)
        {
            return SupportedCultures.Contains(ParseCulture(cultureCode));
        }

        public static string[] GetCultureForCookie(HttpCookie cultureCookie)
        {
            return cultureCookie != null && IsSupportedCulture(cultureCookie.Value) ? new[] { cultureCookie.Value } : null;
        }

        private static CultureInfo ParseCulture(string cultureCode)
        {
            try
            {
                return new CultureInfo(cultureCode, false);
            }
            catch (Exception)
            {
                // Suppress
            }

            return null;
        }
    }
}