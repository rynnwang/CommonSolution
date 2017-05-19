using System;

namespace Beyova
{
    /// <summary>
    /// Interface IRsaKeys
    /// </summary>
    public interface IRsaKeys
    {
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>The private key.</value>
        string PrivateKey { get; set; }
    }
}
