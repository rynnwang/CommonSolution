namespace Beyova
{
    /// <summary>
    /// Class RsaKeys.
    /// </summary>
    public class RsaKeys : IRsaKeys
    {
        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>The private key.</value>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey { get; set; }
    }
}