using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class DualApprovalRule.
    /// </summary>
    public class DualApprovalRule : BaseObject
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission required.
        /// </summary>
        /// <value>The permission required.</value>
        public string PermissionRequired { get; set; }

        /// <summary>
        /// Gets or sets the approve amount required.
        /// </summary>
        /// <value>The approve amount required.</value>
        public int ApproveAmountRequired { get; set; }

        /// <summary>
        /// Gets or sets the callback URI.
        /// </summary>
        /// <value>The callback URI.</value>
        public string CallbackUri { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}
