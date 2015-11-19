using System;

namespace ifunction.Model
{
    /// <summary>
    /// Class SimpleBaseObject
    /// </summary>
    public abstract class SimpleBaseObject : ISimpleBaseObject
    {
        #region Properties

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

        #endregion
    }
}
