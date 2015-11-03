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

        /// <summary>
        /// Checks whether the string has regex format ^[A-Z]+[-][0-9]+$ of issue code
        /// (one or more uppercase characters, dash and one or more numbers).
        /// E.g.: CODE-19
        /// </summary>
        /// <param name="s">The string to be checked</param>
        /// <returns>True if the pattern fits, false otherwise.</returns>
        public static bool CheckIssueCodePattern(string s)
        {
            var rgx = new Regex(@"^[A-Z]+[-][0-9]+$");

            return rgx.IsMatch(s);
        }

        /// <summary>
        /// Splits issue code into project code and issue number.
        /// </summary>
        /// <param name="s">The string to be splitted</param>
        /// <returns>
        /// An array of two string, first is the code of project 
        /// and the second one is issue number
        /// </returns>
        public static string[] SplitIssueCode(string s)
        {
            return s.Split('-');
        }
    }
}