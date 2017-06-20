using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class SecuredMessagePackage.
    /// </summary>
    public class SecuredMessagePackage
    {
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public byte[] Security { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data { get; set; }

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] ToBytes()
        {
            if (this.Security != null)
            {
                var result = new byte[(this.Security?.Length ?? 0) + (this.Data?.Length ?? 0)];

                Buffer.BlockCopy(this.Security, 0, result, 0, this.Security.Length);

                if (this.Data != null)
                {
                    Buffer.BlockCopy(this.Data, 0, result, this.Security.Length, this.Data.Length);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Froms the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Beyova.Gravity.SecuredMessagePackage.</returns>
        public static SecuredMessagePackage FromBytes(byte[] bytes)
        {
            SecuredMessagePackage result = null;

            if (bytes.HasItem())
            {
                const int keyLength = 256;
                result = new SecuredMessagePackage
                {
                    Security = new byte[keyLength],
                    Data = new byte[bytes.Length - keyLength]
                };

                Buffer.BlockCopy(bytes, 0, result.Security, 0, keyLength);
                Buffer.BlockCopy(bytes, result.Security.Length, result.Data, 0, bytes.Length - keyLength);
            }

            return result;
        }
    }
}