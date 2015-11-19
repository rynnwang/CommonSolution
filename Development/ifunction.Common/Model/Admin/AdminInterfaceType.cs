using System;

namespace ifunction.Model
{
    /// <summary>
    /// Class AdminInterfaceType.
    /// </summary>
    [Flags]
    public enum AdminInterfaceType
    {
        /// <summary>
        /// Value indicating it is undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating it is API
        /// </summary>
        Api = 1,
        /// <summary>
        /// Value indicating it is view
        /// </summary>
        View = 2,
        /// <summary>
        /// Value indicating it is customized
        /// </summary>
        Customized = 4
    }
}
