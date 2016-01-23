using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Class DualToken. This class cannot be inherited.
    /// DualToken is used when a random token is required and needs to be safely stored in somewhere for later use by RSA.
    /// Common procedures:
    /// <list type="number">
    /// <item>
    /// A pair of RSA keys are generated. Member A get public key while Member B get private key.
    /// </item>
    /// <item>
    /// Member A: new DualToken(), export token for storing in DB or file using <c>GetTokenForStore</c>.
    /// </item>
    /// <item>
    /// Member A call <c>GetTokenForValidate</c> to get token for validation, and give it to Member B. 
    /// </item>
    /// <item>
    /// Member B get token, decrypt it using private key, get raw token and give it to A for validation.
    /// </item>
    /// <item>
    /// Member A call <c>Validate</c> using token from Member B to see whether it is authorized.
    /// </item>
    /// </list>
    /// </summary>
    public sealed class DualToken
    {
        /// <summary>
        /// The encoding
        /// </summary>
        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// The raw token
        /// </summary>
        private string rawToken;

        /// <summary>
        /// The encrypted token
        /// </summary>
        private string encryptedToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="DualToken"/> class.
        /// </summary>
        /// <param name="storedToken">The stored token.</param>
        public DualToken(string storedToken)
        {
            if (!string.IsNullOrWhiteSpace(storedToken))
            {
                try
                {
                    this.encryptedToken = storedToken;
                    this.rawToken = encryptedToken.R3DDecrypt3DES(encoding);
                }
                catch
                {
                    this.rawToken = string.Empty;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DualToken" /> class.
        /// </summary>
        /// <param name="length">The length.</param>
        public DualToken(int length = 6)
        {
            try
            {
                this.rawToken = this.CreateRandomString(length < 4 ? 4 : length).ToUpperInvariant();
                this.encryptedToken = rawToken.R3DEncrypt3DES(encoding);
            }
            catch { this.rawToken = string.Empty; }
        }

        /// <summary>
        /// Validates the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns><c>true</c> if it is validated, <c>false</c> otherwise.</returns>
        public bool Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            return token.Equals(rawToken, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the token for store.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetTokenForStore()
        {
            return this.encryptedToken.SafeToString();
        }

        /// <summary>
        /// Gets the token for validate.
        /// </summary>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <returns>System.String.</returns>
        public string GetTokenForValidate(string rsaPublicKey)
        {
            try
            {
                var bytes = encoding.GetBytes(this.rawToken);
                var encryptedBytes = bytes.RsaEncrypt(rsaPublicKey);
                return Convert.ToBase64String(encryptedBytes);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.encryptedToken.SafeToString();
        }

        #region Status methods

        /// <summary>
        /// Creates the RSA keys.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        public static void CreateRsaKeys(out string publicKey, out string privateKey)
        {
            EncodingOrSecurityExtension.CreateRsaKeys(null, out publicKey, out privateKey);
        }

        /// <summary>
        /// Validates the specified stored token.
        /// </summary>
        /// <param name="storedToken">The stored token.</param>
        /// <param name="validateToken">The validate token.</param>
        /// <returns><c>true</c> if it is validated, <c>false</c> otherwise.</returns>
        public static bool Validate(string storedToken, string validateToken)
        {
            DualToken token = new DualToken(storedToken);
            return token.Validate(validateToken);
        }

        #endregion
    }
}
