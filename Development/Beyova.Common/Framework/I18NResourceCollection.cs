using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Beyova
{
    /// <summary>
    /// Class I18NResourceCollection.
    /// </summary>
    public class I18NResourceCollection : II18NResourceCollection
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
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns>System.String.</returns>
        public string GetResourceString(string resourceKey, CultureInfo cultureInfo = null, bool languageCompatibility = true)
        {
            return (string)GetResourceObject(resourceKey, cultureInfo, languageCompatibility);
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns>System.Object.</returns>
        public object GetResourceObject(string resourceKey, CultureInfo cultureInfo = null, bool languageCompatibility = true)
        {
            if (!string.IsNullOrWhiteSpace(resourceKey))
            {
                if (cultureInfo != null)
                {
                    var resourceSet = this.ResourceManager.GetResourceSet(cultureInfo, false, languageCompatibility);
                    if (resourceSet != null)
                    {
                        return resourceSet.GetString(resourceKey);
                    }
                }

                return this.ResourceManager.GetString(resourceKey);
            }

            return string.Empty;
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
        public Dictionary<CultureInfo, Dictionary<string, object>> GetResourceSet()
        {
            return this.ResourceManager.ToDictionary(false);
        }

        /// <summary>
        /// Gets the resource sets.
        /// </summary>
        /// <returns>Dictionary&lt;CultureInfo, ResourceSet&gt;.</returns>
        public Dictionary<CultureInfo, ResourceSet> GetResourceSets()
        {
            var result = new Dictionary<CultureInfo, ResourceSet>();

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                var resourceSet = this.ResourceManager.GetResourceSet(cultureInfo, false, true);
                result.AddIfNotNull(cultureInfo, resourceSet);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [has culture resource] [the specified culture information].
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns><c>true</c> if [has culture resource] [the specified culture information]; otherwise, <c>false</c>.</returns>
        public bool HasCultureResource(CultureInfo cultureInfo)
        {
            return cultureInfo != null && this.ResourceManager.GetResourceSet(cultureInfo, false, true) != null;
        }

        #endregion
    }
}
