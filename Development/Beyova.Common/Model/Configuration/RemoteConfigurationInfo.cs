﻿using System;

namespace Beyova
{
    /// <summary>
    /// Class RemoteConfigurationInfo.
    /// </summary>
    public class RemoteConfigurationInfo : RemoteConfigurationObject, IBaseObject
    {
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the last updated by.
        /// </summary>
        /// <value>The last updated by.</value>
        public string LastUpdatedBy { get; set; }

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
    }
}