using System;

namespace Beyova
{
    /// <summary>
    /// Enum PlatformType
    /// </summary>
    [Flags]
    public enum PlatformType
    {
        /// <summary>
        /// Value indicating it is none (0x00)
        /// </summary>
        None = 0,
        /// <summary>
        /// Value indicating it is iOS (0x1)
        /// </summary>
        iOS = 0x1,
        /// <summary>
        /// Value indicating it is android (0x2)
        /// </summary>
        Android = 0x2,
        /// <summary>
        /// Value indicating it is windows phone (0x4)
        /// </summary>
        WindowsPhone = 0x4,
        /// <summary>
        /// Value indicating it is black berry (0x8)
        /// </summary>
        BlackBerry = 0x8,
        /// <summary>
        /// Value indicating it is web (0x10)
        /// </summary>
        Web = 0x10
    }
}