using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Newtonsoft.Json;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class BeyovaCultureResourceAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class BeyovaCultureResourceAttribute : Attribute
    {
        /// <summary>
        /// The file extension
        /// </summary>
        public const string FileExtension = ".i18n";

        /// <summary>
        /// Gets or sets the name of the resource base.
        /// </summary>
        /// <value>The name of the resource base.</value>
        public string ResourceBaseName { get; protected set; }

        /// <summary>
        /// Gets or sets the default culture code.
        /// </summary>
        /// <value>The default culture code.</value>
        public string DefaultCultureCode { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [try language compatibility].
        /// </summary>
        /// <value><c>true</c> if [try language compatibility]; otherwise, <c>false</c>.</value>
        public bool TryLanguageCompatibility { get; protected set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public string Directory { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// Parameter <c>resourceBaseName</c> should contains namespace as full name.
        /// </summary>
        /// <param name="cultureResourceDirectory">The culture resource directory.</param>
        /// <param name="resourceBaseName">Name of the resource base.</param>
        /// <param name="defaultCultureCode">The default culture code.</param>
        /// <param name="tryLanguageCompatibility">if set to <c>true</c> [try language compatibility].</param>
        public BeyovaCultureResourceAttribute(string cultureResourceDirectory, string resourceBaseName, string defaultCultureCode, bool tryLanguageCompatibility = true)
        {
            this.ResourceBaseName = resourceBaseName;
            this.DefaultCultureCode = defaultCultureCode;
            this.TryLanguageCompatibility = tryLanguageCompatibility;
            this.Directory = cultureResourceDirectory;
        }

        /// <summary>
        /// Fills the resources.
        /// </summary>
        /// <param name="container">The container.</param>
        internal void FillResources(Dictionary<CultureInfo, Dictionary<string, GlobalCultureResource>> container)
        {
            if (container == null)
            {
                return;
            }

            try
            {
                DirectoryInfo directory = EnvironmentCore.GetDirectory(this.Directory);

                if (directory.Exists)
                {
                    foreach (var file in directory.GetFiles("*" + FileExtension))
                    {
                        string cultureCode = null;

                        if (string.IsNullOrWhiteSpace(ResourceBaseName))
                        {
                            cultureCode = file.Name.Substring(0, file.Name.Length - FileExtension.Length);
                        }
                        else if (file.Name.StartsWith(ResourceBaseName, StringComparison.OrdinalIgnoreCase))
                        {
                            cultureCode = file.Name.Substring(ResourceBaseName.Length, file.Name.Length - ResourceBaseName.Length - FileExtension.Length);
                        }

                        var cultureInfo = (cultureCode?.Trim('.')).AsCultureInfo();
                        if (cultureInfo != null)
                        {
                            var resources = TryReadResourceFile(file.FullName);
                            if (resources != null)
                            {
                                var destinationContainer = container.GetOrCreate(cultureInfo, new Dictionary<string, GlobalCultureResource>(StringComparer.OrdinalIgnoreCase));
                                foreach (var item in resources)
                                {
                                    foreach (var one in item.Value)
                                    {
                                        destinationContainer.Merge(one.Key, new GlobalCultureResource { Resource = one.Value, Source = file.Name }, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { ResourceBaseName, DefaultCultureCode, TryLanguageCompatibility, Directory });
            }
        }

        /// <summary>
        /// Tries the read resource file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.Collections.Generic.Dictionary&lt;System.String, System.Collections.Generic.Dictionary&lt;System.String, System.String&gt;&gt;.</returns>
        internal static Dictionary<string, Dictionary<string, string>> TryReadResourceFile(string filePath)
        {
            string fileContent = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    fileContent = File.ReadAllText(filePath, Encoding.UTF8);
                    return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(fileContent);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { filePath, fileContent });
            }

            return null;
        }
    }
}
