using System;

namespace Beyova
{
    /// <summary>
    /// Class EnterpriseCustomerContract.
    /// </summary>
    public class EnterpriseCustomerContract : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the contract key.
        /// </summary>
        /// <value>
        /// The contract key.
        /// </value>
        public Guid? ContractKey { get; set; }

        /// <summary>
        /// Gets or sets the last signature stamp.
        /// </summary>
        /// <value>
        /// The last signature stamp.
        /// </value>
        public DateTime? LastSignatureStamp { get; set; }
    }
}