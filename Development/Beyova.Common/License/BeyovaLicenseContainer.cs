using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Beyova.ExceptionSystem;

namespace Beyova.License
{
    /// <summary>
    /// Class BeyovaLicenseContainer.
    /// </summary>
    internal static class BeyovaLicenseContainer
    {
        /// <summary>
        /// The licenses
        /// </summary>
        private static Dictionary<string, BeyovaLicenseInfo> licenses = new Dictionary<string, BeyovaLicenseInfo>();

        /// <summary>
        /// Gets the licenses.
        /// </summary>
        /// <value>The licenses.</value>
        internal static IEnumerable<KeyValuePair<string, BeyovaLicenseInfo>> Licenses { get { return licenses; } }

        /// <summary>
        /// Initializes static members of the <see cref="BeyovaLicenseContainer"/> class.
        /// </summary>
        static BeyovaLicenseContainer()
        {
            foreach (var one in EnvironmentCore.DescendingAssemblyDependencyChain)
            {
                var licenseAttribute = one.GetCustomAttribute<BeyovaLicenseAttribute>();
                if (licenseAttribute != null)
                {
                    AttachLicense(one, licenseAttribute);
                }
            }
        }

        /// <summary>
        /// Attaches the license.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="licenseAttribute">The license attribute.</param>
        public static void AttachLicense(Assembly assembly, BeyovaLicenseAttribute licenseAttribute)
        {
            if (assembly != null && licenseAttribute != null)
            {
                string assemblyName = assembly.GetName().Name;

                if (licenses.ContainsKey(assemblyName))
                {
                    return;
                }

                var license = licenseAttribute.ReadBeyovaLicense();

                if (license != null)
                {
                    licenses.Add(assemblyName, license);
                }
                else
                {
                    if (licenseAttribute.IsRequired)
                    {
                        throw new InvalidLicenseException(assemblyName);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the license.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>Beyova.License.BeyovaLicense.</returns>
        public static BeyovaLicenseInfo GetLicense(string assemblyName)
        {
            try
            {
                assemblyName.CheckEmptyString("assemblyName");

                return licenses.TryGetValue(assemblyName, null);
            }
            catch (Exception ex)
            {
                throw ex.Handle(assemblyName);
            }
        }
    }
}
