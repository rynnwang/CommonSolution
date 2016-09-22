namespace Beyova.Gravity
{
    /// <summary>
    /// Class SecureMessageObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SecureMessageObject<T>
    {
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }
    }
}
