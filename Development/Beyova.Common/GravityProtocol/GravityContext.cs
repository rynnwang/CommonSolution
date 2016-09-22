using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityClient.
    /// </summary>
    internal class GravityContext
    {
        #region Thread base

        const string threadKey = "GravityContext";

        /// <summary>
        /// Gets the current. Get or create <see cref="GravityContext"/> instance for current thread.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static GravityContext Current
        {
            get
            {
                var result = ThreadExtension.GetThreadData(threadKey) as GravityContext;

                if (result == null)
                {
                    result = new GravityContext();
                    ThreadExtension.SetThreadData(threadKey, result);
                }

                return result;
            }
        }

        /// <summary>
        /// Disposes the current.
        /// </summary>
        public static void DisposeCurrent()
        {
            ThreadExtension.SetThreadData(threadKey, null);
        }

        #endregion

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        public string PrivateKey { get; set; }
    }
}
