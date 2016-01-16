using System;

namespace Beyova
{
    /// <summary>
    /// Enum Gender
    /// </summary>
    [Flags]
    public enum Gender
    {
        /// <summary>
        /// Value indicating it is Undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating it is male
        /// </summary>
        Male = 1,
        /// <summary>
        /// Value indicating it is female
        /// </summary>
        Female = 2
    }
}
