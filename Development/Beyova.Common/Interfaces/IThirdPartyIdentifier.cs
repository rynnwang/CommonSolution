using System;

namespace Beyova
{
    /// <summary>
    /// Interface IThirdPartyIdentifier
    /// </summary>
    public interface IThirdPartyIdentifier
    {
        /// <summary>
        /// Gets or sets the third party identifier.
        /// </summary>
        /// <value>
        /// The third party identifier.
        /// </value>
        string ThirdPartyId { get; set; }
    }
}