using System;

namespace Beyova
{
    /// <summary>
    /// Class ThirdPartyAccountBindRequest.
    /// </summary>
    /// <typeparam name="TFunctionalRole">The type of the functional role.</typeparam>
    public class ThirdPartyAccountBindRequest<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the source access credential.
        /// </summary>
        /// <value>The source access credential.</value>
        public AccessCredential SourceAccessCredential { get; set; }

        /// <summary>
        /// Gets or sets the third party access credential.
        /// </summary>
        /// <value>The third party access credential.</value>
        public AccessCredential ThirdPartyAccessCredential { get; set; }

        /// <summary>
        /// Gets or sets the third party token expired stamp.
        /// </summary>
        /// <value>The third party token expired stamp.</value>
        public DateTime? ThirdPartyTokenExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the functional role.
        /// </summary>
        /// <value>The functional role.</value>
        public TFunctionalRole? FunctionalRole { set; get; }
    }
}