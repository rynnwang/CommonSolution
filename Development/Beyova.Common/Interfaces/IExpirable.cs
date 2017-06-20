using System;

namespace Beyova
{
    /// <summary>
    /// Interface IExpirable
    /// </summary>
    public interface IExpirable
    {
        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>
        /// The expired stamp.
        /// </value>
        DateTime? ExpiredStamp { get; }
    }
}