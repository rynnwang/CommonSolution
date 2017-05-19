using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityEntryObject.
    /// </summary>
    public class GravityEntryObject
    {
        /// <summary>
        /// Gets or sets the member identifiable key. (Token)
        /// </summary>
        /// <value>The member identifiable key.</value>
        public string MemberIdentifiableKey { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the gravity service URI. E.g.: https://localhost/gravity/
        /// </summary>
        /// <value>The gravity service URI.</value>
        public Uri GravityServiceUri { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigurationName { get; set; }
    }
}
