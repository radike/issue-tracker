using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IssueTracker.Abstractions
{
    public class Helper
    {
        /// <summary>
        /// Checks whether the string has regex format ^[A-Z]+$ of project code
        /// (one or more uppercase characters).
        /// E.g.: CODE-19
        /// </summary>
        /// <param name="s">The string to be checked</param>
        /// <returns>True if the pattern fits, false otherwise.</returns>
        public static bool CheckProjectCodePattern(string s)
        {
            var rgx = new Regex(@"^[a-zA-Z]+$");
            return rgx.IsMatch(s);
        }
    }
}