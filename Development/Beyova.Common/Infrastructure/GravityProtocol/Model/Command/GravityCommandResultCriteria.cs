using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandResultCriteria.
    /// </summary>
    public class GravityCommandResultCriteria : GravityCommandResultBase
    {
        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public Guid? ProductKey { get; set; }
    }
}