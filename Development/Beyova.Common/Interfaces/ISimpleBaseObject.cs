using System;

namespace Beyova
{
    /// <summary>
    /// Interface ISimpleBaseObject
    /// </summary>
    public interface ISimpleBaseObject : IIdentifier
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        DateTime? CreatedStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>The last updated stamp.</value>
        DateTime? LastUpdatedStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        ObjectState State
        {
            get;
            set;
        }

        #endregion Properties
    }
}