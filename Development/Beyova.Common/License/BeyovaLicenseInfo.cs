using System;
using System.Collections.Generic;
using System.Diagnostics;
using Beyova.Gravity;
using Newtonsoft.Json.Linq;

namespace Beyova.License
{
    /// <summary>
    /// Class BeyovaLicenseInfo.
    /// </summary>
    public class BeyovaLicenseInfo : GravityBaseObject
    {
        /// <summary>
        /// Gets or sets the license path.
        /// </summary>
        /// <value>The license path.</value>
        public string LicensePath { get; set; }

        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; set; }

        /// <summary>
        /// Gets the current. This method would find assembly by stack trace. So use it directly where you need.
        /// </summary>
        /// <value>The current.</value>
        public static BeyovaLicenseInfo Current
        {
            get
            {
                var assemblyName = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;
                return BeyovaLicenseContainer.GetLicense(assemblyName);
            }
        }
    }
}
