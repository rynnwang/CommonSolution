using System;

namespace Beyova
{
    /// <summary>
    /// Class EnterpriseCustomerContractIdentity.
    /// </summary>
    public class EnterpriseCustomerContractIdentity : BusinessUnit, IExpirable
    {
        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>
        /// The expired stamp.
        /// </value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the effected contract key. Value maps to <c>Key</c> of <see cref="EnterpriseCustomerContract"/>.
        /// </summary>
        /// <value>
        /// The effected contract key.
        /// </value>
        public Guid? EffectedContractKey { get; set; }

        /// <summary>
        /// Gets or sets the enterprise customer key. Value maps to <c>Key</c> of <see cref="EnterpriseCustomerIdentity"/>.
        /// </summary>
        /// <value>
        /// The enterprise customer key.
        /// </value>
        public Guid? EnterpriseCustomerKey { get; set; }
    }
}