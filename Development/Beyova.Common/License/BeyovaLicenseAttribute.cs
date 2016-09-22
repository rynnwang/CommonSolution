using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.Gravity;

namespace Beyova.License
{
    /// <summary>
    /// Class BeyovaLicenseAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class BeyovaLicenseAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the license path.
        /// </summary>
        /// <value>The license path.</value>
        public string LicensePath { get; protected set; }

        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool IsRequired { get; protected set; }

        /// <summary>
        /// Gets or sets the reader.
        /// </summary>
        /// <value>The reader.</value>
        public IBeyovaLicenseReader Reader { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaLicenseAttribute" /> class.
        /// </summary>
        /// <param name="licensePath">The license path.</param>
        /// <param name="licenseServiceUri">The license service URI.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        /// <param name="readerType">(Optional) Type of the reader, which needs to implement interface <see cref="IBeyovaLicenseReader" /></param>
        public BeyovaLicenseAttribute(string licensePath, Uri licenseServiceUri, bool isRequired, Type readerType = null)
        {
            this.LicensePath = licensePath;
            this.Entry = new GravityEntryObject { GravityServiceUri = licenseServiceUri };
            this.IsRequired = isRequired;
            this.Reader = (readerType?.CreateInstance() as IBeyovaLicenseReader) ?? new BeyovaLicenseReader();
        }

        /// <summary>
        /// Reads the beyova license.
        /// </summary>
        /// <returns>Beyova.License.BeyovaLicense.</returns>
        internal BeyovaLicenseInfo ReadBeyovaLicense()
        {
            return this.Reader.Read(this);
        }
    }
}
