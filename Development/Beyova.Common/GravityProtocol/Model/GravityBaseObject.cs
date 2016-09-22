using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityBaseObject.
    /// </summary>
    public abstract class GravityBaseObject
    {
        /// <summary>
        /// Gets or sets the client key.
        /// </summary>
        /// <value>The client key.</value>
        public string ClientKey { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        /// <value>The name of the owner.</value>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityBaseObject"/> class.
        /// </summary>
        protected internal GravityBaseObject()
        {

        }
    }
}
