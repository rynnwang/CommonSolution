using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandResultCriteria.
    /// </summary>
    public class GravityCommandResultCriteria : GravityCommandResultBase
    {
        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>The project key.</value>
        public Guid? ProjectKey { get; set; }
    }
}