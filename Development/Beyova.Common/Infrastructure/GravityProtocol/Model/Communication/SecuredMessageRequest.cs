using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class SecuredMessageRequest.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SecuredMessageRequest<T>
    {
        /// <summary>
        /// Gets or sets the encryption key.
        /// </summary>
        /// <value>The encryption key.</value>
        public byte[] EncryptionKey { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public SecuredMessageObject<T> Message { get; set; }
    }
}
