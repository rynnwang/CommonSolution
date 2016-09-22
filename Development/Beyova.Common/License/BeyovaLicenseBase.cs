using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.Gravity;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class BeyovaLicenseBase.
    /// </summary>
    public class BeyovaLicenseBase
    {
        /// <summary>
        /// Gets or sets the client key.
        /// </summary>
        /// <value>The client key.</value>
        public string ClientKey { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        /// <value>The name of the owner.</value>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the working status.
        /// </summary>
        /// <value>The working status.</value>
        public ComponentWorkingStatus WorkingStatus { get; set; }
    }
}
