using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IssueTracker.Abstractions
{
    public class CultureHelper
    {
        public static string PREFFERED_CULTURE_COOKIE = "PrefferedCulture";
        public static CultureInfo DEFAULT_CULTURE = new CultureInfo("en-us", false);

        public static IReadOnlyCollection<CultureInfo> SupportedCultures = new List<CultureInfo>() {
            DEFAULT_CULTURE,
            new CultureInfo("sk-sk", false)
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

                // Try to find exact match
                var match = SupportedCultures.SingleOrDefault(c => c.Equals(requestedCulture));
                // If not found try to get language match
                match = match ?? SupportedCultures.SingleOrDefault(c => c.TwoLetterISOLanguageName == requestedCulture.TwoLetterISOLanguageName);

                if (match != null)
                {
                    return match;
                }
            }

            return DEFAULT_CULTURE;
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