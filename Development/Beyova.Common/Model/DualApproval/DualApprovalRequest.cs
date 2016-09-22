using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class DualApprovalRequest.
    /// </summary>
    public class DualApprovalRequest : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the rule key.
        /// </summary>
        /// <value>The rule key.</value>
        public Guid? RuleKey { get; set; }

        /// <summary>
        /// Gets or sets the request identifier. For client to retrieve and attach biz id.
        /// </summary>
        /// <value>The request identifier.</value>
        public string RequestIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the requested by.
        /// </summary>
        /// <value>The requested by.</value>
        public Guid? RequestedBy { get; set; }
    }
}
