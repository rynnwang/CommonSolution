using System;

namespace Beyova
{
    /// <summary>
    /// Enum DeviceType
    /// </summary>
    [Flags]
    public enum DeviceType
    {
        /// <summary>
        /// Value indicating it is none (0x00)
        /// </summary>
        None = 0,

        /// <summary>
        /// Value indicating it is phone
        /// </summary>
        Phone = 0x1,

        /// <summary>
        /// Value indicating it is pad
        /// </summary>
        Pad = 0x2,

        /// <summary>
        /// Value indicating it is pc (0x4)
        /// </summary>
        Pc = 0x4,

        /// <summary>
        /// Value indicating it is wearable (0x8)
        /// </summary>
        Wearable = 0x8
    }
}