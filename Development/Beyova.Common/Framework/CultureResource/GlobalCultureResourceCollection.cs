using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Beyova.ProgrammingIntelligence;

namespace Beyova
{
    /// <summary>
    /// Class GlobalResourceCollection. Only resources defined via <see cref="BeyovaCultureResourceAttribute"/> would be provided here.
    /// </summary>
    public sealed class GlobalCultureResourceCollection : II18NResourceCollection
    {
        #region Singleton

        /// <summary>
        /// The singleton
        /// </summary>
        private static GlobalCultureResourceCollection singleton = new GlobalCultureResourceCollection();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GlobalCultureResourceCollection Instance { get { return singleton; } }

        #endregion

        /// <summary>
        /// The culture based resources
        /// </summary>
        private Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>> cultureBasedResources;

        /// <summary>
        /// Gets or sets the default culture information.
        /// </summary>
        /// <value>The default culture information.</value>
        public CultureInfo DefaultCultureInfo { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="GlobalCultureResourceCollection"/> class from being created.
        /// </summary>
        private GlobalCultureResourceCollection()
        {
            CultureInfo defaultCultureInfo;
            cultureBasedResources = Initialize(out defaultCultureInfo);
            DefaultCultureInfo = defaultCultureInfo ?? new CultureInfo("en-US");
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Dictionary&lt;CultureInfo, Dictionary&lt;System.String, System.Object&gt;&gt;.</returns>
        private Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>> Initialize(out CultureInfo defaultCultureInfo)
        {
            Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>> result = new Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>>();

            string defaultCultureCode = null;
            foreach (var one in EnvironmentCore.DescendingAssemblyDependencyChain)
            {
                var cultureResourceAttribute = one.GetCustomAttribute<BeyovaCultureResourceAttribute>();
                if (cultureResourceAttribute != null)
                {
                    if (!string.IsNullOrWhiteSpace(cultureResourceAttribute.DefaultCultureCode))
                    {
                        defaultCultureCode = cultureResourceAttribute.DefaultCultureCode;
                    }

                    cultureResourceAttribute.FillResources(result);
                }
            }

            defaultCultureInfo = defaultCultureCode.AsCultureInfo();
            return result;
        }

        /// <summary>
        /// Determines whether [has culture resource] [the specified culture information].
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns><c>true</c> if [has culture resource] [the specified culture information]; otherwise, <c>false</c>.</returns>
        public bool HasCultureResource(CultureInfo cultureInfo)
        {
            return cultureInfo != null && cultureBasedResources.ContainsKey(cultureInfo);
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">The language compatibility.</param>
        /// <returns>System.Object.</returns>
        public string GetResourceString(string resourceKey, CultureInfo cultureInfo = null, bool languageCompatibility = true)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                return null;
            }

            cultureInfo = cultureInfo ?? ContextHelper.CurrentCultureInfo ?? DefaultCultureInfo;

            if (cultureInfo == null) return string.Empty;

            Dictionary<string, GlobalCultureResource> hitDictionary = null;
            if (!cultureBasedResources.TryGetValue(cultureInfo, out hitDictionary))
            {
                if (languageCompatibility)
                {
                    if (cultureInfo.Name.Contains("-"))
                    {
                        var parentCultureInfo = cultureInfo?.Name.SubStringBeforeFirstMatch('-').AsCultureInfo();

                        if (parentCultureInfo != null)
                        {
                            cultureBasedResources.TryGetValue(parentCultureInfo, out hitDictionary);
                        }
                    }
                }

                if (hitDictionary == null)
                {
                    cultureBasedResources.TryGetValue(DefaultCultureInfo, out hitDictionary);
                }
            }

            if (hitDictionary != null)
            {
                GlobalCultureResource result;
                return hitDictionary.TryGetValue(resourceKey, out result) ? result?.Resource : null;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the available culture information.
        /// </summary>
        /// <value>The available culture information.</value>
        public ICollection<CultureInfo> AvailableCultureInfo
        {
            get
            {
                return cultureBasedResources.Keys;
            }
        }

        /// <summary>
        /// Creates the culture resource file.
        /// </summary>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="cultureInfo">The culture information.</param>
        public static void CreateCultureResourceFile(string targetDirectory, string baseName, CultureInfo cultureInfo)
        {

        }
    }
}
