using System;

namespace ifunction.Model
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
        /// Value indicating it is tablet
        /// </summary>
        Tablet = 0x2,
        /// <summary>
        /// Value indicating it is pc (0x10)
        /// </summary>
        Pc = 0x10,
        /// <summary>
        /// Value indicating it is mobile (0x20)
        /// </summary>
        Mobile = 0x20,
        /// <summary>
        /// Value indicating it is mobile phone (0x21 = 0x20 + 0x1)
        /// </summary>
        MobilePhone = 0x21,
        /// <summary>
        /// Value indicating it is mobile pad (0x22 = 0x20 + 0x2)
        /// </summary>
        MobilePad = 0x22,
        /// <summary>
        /// Value indicating it is wearable device (0x40)
        /// </summary>
        WearableDevice = 0x40
    }
}