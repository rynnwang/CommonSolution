using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ifunction
{
    /// <summary>
    /// Class I18NResourceCollection.
    /// </summary>
    public class I18NResourceCollection
    {
        /// <summary>
        /// The resource manager
        /// </summary>
        protected ResourceManager resourceManager;

        /// <summary>
        /// Gets or sets the resource manager.
        /// </summary>
        /// <value>The resource manager.</value>
        public ResourceManager ResourceManager
        {
            get
            {
                return resourceManager;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="I18NResourceCollection" /> class.
        /// </summary>
        /// <param name="resourceBaseName">Name of the resource.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        public I18NResourceCollection(string resourceBaseName, Assembly resourceAssembly)
        {
            this.resourceManager = new ResourceManager(resourceBaseName, resourceAssembly);
        }

        #region Public methods

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>System.String.</returns>
        public string this[string resourceKey]
        {
            get
            {
                return GetResourceString(resourceKey);
            }
        }

        #region GetResourceString

        /// <summary>
        /// Gets the resource string.
        /// If cultureCode is specified, then use that.
        /// Otherwise, use Thread UI culture (the default behavior by .NET) instead.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns>System.String.</returns>
        public string GetResourceString(string resourceKey, bool ignoreCase, string cultureCode = null, bool languageCompatibility = true)
        {
            if (!string.IsNullOrWhiteSpace(resourceKey))
            {
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    var resourceSet = this.ResourceManager.GetResourceSet(new CultureInfo(cultureCode), false, languageCompatibility);
                    if (resourceSet != null)
                    {
                        return resourceSet.GetString(resourceKey, ignoreCase);
                    }
                }

                return this.ResourceManager.GetString(resourceKey);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the resource string.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns>System.String.</returns>
        public string GetResourceString(string resourceKey, string cultureCode = null, bool languageCompatibility = true)
        {
            return GetResourceString(resourceKey, false, cultureCode, languageCompatibility);
        }

        #endregion

        /// <summary>
        /// Gets the resource strings by culture code.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> GetResourceStringsByCultureCode(string cultureCode)
        {
            if (!string.IsNullOrWhiteSpace(cultureCode))
            {
                var resourceSet = this.ResourceManager.GetResourceSet(new CultureInfo(cultureCode), false, true);

                if (resourceSet != null)
                {
                    var enumerator = resourceSet.GetEnumerator();
                    Dictionary<string, string> result = new Dictionary<string, string>();

                    while (enumerator.MoveNext())
                    {
                        var key = (string)enumerator.Key;
                        result.Add(key, resourceSet.GetString(key));
                    }

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the resource string set.
        /// </summary>
        /// <returns>Dictionary&lt;CultureInfo, Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        public Dictionary<CultureInfo, Dictionary<string, string>> GetResourceStringSet()
        {
            Dictionary<CultureInfo, Dictionary<string, string>> result = new Dictionary<CultureInfo, Dictionary<string, string>>();

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                var resourceSet = this.ResourceManager.GetResourceSet(cultureInfo, false, false);

                if (resourceSet != null)
                {
                    var enumerator = resourceSet.GetEnumerator();
                    var set = new Dictionary<string, string>();

                    while (enumerator.MoveNext())
                    {
                        var key = (string)enumerator.Key;
                        set.Add(key, resourceSet.GetString(key));
                    }

                    result.Add(cultureInfo, set);
                }
            }

            return result;
        }

        #endregion
    }
}
