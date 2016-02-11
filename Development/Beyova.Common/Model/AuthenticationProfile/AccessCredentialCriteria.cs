using System;

namespace Beyova
{
    /// <summary>
    /// Class AccessCredentialCriteria.
    /// </summary>
    public class AccessCredentialCriteria : AccessCredential, ICriteria
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }
    }
}
