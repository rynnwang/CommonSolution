using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.ProgrammingIntelligence;

namespace Beyova.License
{
    /// <summary>
    /// Interface IBeyovaLicenseReader.
    /// </summary>
    public interface IBeyovaLicenseReader
    {
        /// <summary>
        /// Reads the specified license attribute.
        /// </summary>
        /// <param name="licenseAttribute">The license attribute.</param>
        /// <returns>BeyovaLicense.</returns>
        BeyovaLicenseInfo Read(BeyovaLicenseAttribute licenseAttribute);
    }
}
