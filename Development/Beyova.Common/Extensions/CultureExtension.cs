using System.Globalization;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static class CultureExtension
    {
        /// <summary>
        /// The culture code regex
        /// </summary>
        private static Regex cultureCodeRegex = new Regex(@"^[a-zA-Z]{2}(\-[a-zA-Z]{2})?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// To the culture info.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultCultureInfo">The default culture information.</param>
        /// <returns>CultureInfo.</returns>
        public static CultureInfo ToCultureInfo(this string stringObject, CultureInfo defaultCultureInfo = null)
        {
            try
            {
                return new CultureInfo(stringObject);
            }
            catch
            {
                return defaultCultureInfo;
            }
        }

        /// <summary>
        /// Ensures the culture code.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="defaultCultureCode">The default culture code.</param>
        /// <returns></returns>
        public static string EnsureCultureCode(this string cultureCode, string defaultCultureCode = null)
        {
            return (!string.IsNullOrWhiteSpace(cultureCode) && cultureCodeRegex.IsMatch(cultureCode)) ? cultureCode : defaultCultureCode;
        }
    }
}