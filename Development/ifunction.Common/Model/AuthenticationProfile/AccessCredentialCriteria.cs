using System;

namespace ifunction.Model
{
    /// <summary>
    /// Class AccessCredentialCriteria.
    /// </summary>
    public class AccessCredentialCriteria : AccessCredential
    {
        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }
    }
}
