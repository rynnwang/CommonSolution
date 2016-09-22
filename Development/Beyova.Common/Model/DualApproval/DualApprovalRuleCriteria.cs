using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class DualApprovalRuleCriteria.
    /// </summary>
    public class DualApprovalRuleCriteria
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>The keyword.</value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the permission required.
        /// </summary>
        /// <value>The permission required.</value>
        public string PermissionRequired { get; set; }
    }
}
