using System;

namespace Beyova
{
    /// <summary>
    /// Class AccessCredentialInfo.
    /// </summary>
    public class AccessCredentialInfo : AccessCredential, ISimpleBaseObject
    {
        #region ISimpleBaseObject

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
        public DateTime? CreatedStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>
        /// The last updated stamp.
        /// </value>
        public DateTime? LastUpdatedStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public ObjectState State { get; set; }

        #endregion ISimpleBaseObject

        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the token expired stamp.
        /// </summary>
        /// <value>The token expired stamp.</value>
        public DateTime? TokenExpiredStamp { get; set; }
    }
}