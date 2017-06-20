using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace Beyova
{
    /// <summary>
    /// Class GlobalizationExtension.
    /// </summary>
    public static class GlobalizationExtension
    {
        /// <summary>
        /// As the culture information.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>CultureInfo.</returns>
        public static CultureInfo AsCultureInfo(this string code)
        {
            try
            {
                return string.IsNullOrWhiteSpace(code) ? null : new CultureInfo(code);
            }
            catch { return null; }
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="tryParent">if set to <c>true</c> [try parent].</param>
        /// <returns>Dictionary&lt;CultureInfo, Dictionary&lt;System.String, System.Object&gt;&gt;.</returns>
        public static Dictionary<CultureInfo, Dictionary<string, object>> ToDictionary(this ResourceManager resourceManager, bool tryParent = false)
        {
            Dictionary<CultureInfo, Dictionary<string, object>> result = new Dictionary<CultureInfo, Dictionary<string, object>>();

            if (resourceManager != null)
            {
                foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    var dictionary = ToDictionary(resourceManager, cultureInfo, tryParent);

                    if (dictionary.HasItem())
                    {
                        result.Add(cultureInfo, dictionary);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="tryParent">if set to <c>true</c> [try parent].</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, object> ToDictionary(this ResourceManager resourceManager, CultureInfo cultureInfo, bool tryParent = false)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (resourceManager != null && cultureInfo != null)
            {
                // NOTE: here set createIfNotExists to true means, even resource is not used yet, but manager still need to load it.
                // Otherwise, the resourceSet would be null.
                var resourceSet = resourceManager.GetResourceSet(cultureInfo, true, tryParent);

                if (resourceSet != null)
                {
                    var enumerator = resourceSet.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        result.Add((string)enumerator.Key, enumerator.Value);
                    }
                }
            }

            return result;
        }
    }
}