using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityEntryObject.
    /// </summary>
    public class GravityEntryObject
    {
        /// <summary>
        /// Gets or sets the client key.
        /// </summary>
        /// <value>The client key.</value>
        public string ClientKey { get; set; }

        /// <summary>
        /// Gets or sets the gravity service URI.
        /// </summary>
        /// <value>The gravity service URI.</value>
        public Uri GravityServiceUri { get; set; }
    }
}
