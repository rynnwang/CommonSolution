using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Beyova.License
{
    /// <summary>
    /// Class BeyovaLicenseReader.
    /// </summary>
    public class BeyovaLicenseReader : IBeyovaLicenseReader
    {
        /// <summary>
        /// Reads the specified license attribute.
        /// </summary>
        /// <param name="licenseAttribute">The license attribute.</param>
        /// <returns>BeyovaLicense.</returns>
        public BeyovaLicenseInfo Read(BeyovaLicenseAttribute licenseAttribute)
        {
            if (licenseAttribute != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(licenseAttribute.LicensePath) && File.Exists(licenseAttribute.LicensePath))
                    {
                        var json = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(licenseAttribute.LicensePath)));
                        var result = JsonConvert.DeserializeObject<BeyovaLicenseInfo>(json);
                        result.LicensePath = licenseAttribute.LicensePath;
                        result.Entry = licenseAttribute.Entry;

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking?.LogException(ex.Handle(new
                    {
                        licenseAttribute.LicensePath,
                        Uri = licenseAttribute.Entry?.GravityServiceUri,
                        licenseAttribute.IsRequired
                    }).ToExceptionInfo());
                }
            }

            return null;
        }
    }
}
