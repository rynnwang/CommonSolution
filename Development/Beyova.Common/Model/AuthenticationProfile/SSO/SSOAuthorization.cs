using System;

namespace Beyova
{
    /// <summary>
    /// Class SSOAuthorization.
    /// </summary>
    public class SSOAuthorization : SSOAuthorizationBase, ISimpleBaseObject, IExpirable
    {
        /// <summary>
        /// Gets or sets the partner key.
        /// </summary>
        /// <value>The partner key.</value>
        public Guid? PartnerKey { get; set; }

        /// <summary>
        /// Gets or sets the callback URL.
        /// </summary>
        /// <value>The callback URL.</value>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the used stamp.
        /// </summary>
        /// <value>The used stamp.</value>
        public DateTime? UsedStamp { get; set; }

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>The last updated stamp.</value>
        public DateTime? LastUpdatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public ObjectState State { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorization" /> class.
        /// </summary>
        public SSOAuthorization() : base()
        {
        }
    }
}