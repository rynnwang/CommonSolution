using System;

namespace Beyova
{
    /// <summary>
    ///     Enum values for object status.
    /// </summary>
    [Flags]
    public enum ObjectState
    {
        /// <summary>
        ///     The value indicating that object is normal.(0x0)
        /// </summary>
        Normal = 0,
        /// <summary>
        ///     The value indicating that object is deleted logically. (0x1)
        /// </summary>
        Deleted = 0x01,
        /// <summary>
        ///     The value indicating that object is invisible. (0x2)
        /// </summary>
        Invisible = 0x02,
        /// <summary>
        ///     The value indicating that object is readonly. (0x4)
        /// </summary>
        ReadOnly = 0x04,
        /// <summary>
        ///     The value indicating that object is disabled. (0x8)
        /// </summary>
        Disabled = 0x08
    }
}