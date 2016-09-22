using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Enum ServiceRuntimeStatus
    /// </summary>
    public enum ServiceRuntimeStatus
    {
        /// <summary>
        /// Value indicating it is unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Value indicating it is initializing
        /// </summary>
        Initializing = 1,
        /// <summary>
        /// Value indicating it is running
        /// </summary>
        Running = 2,
        /// <summary>
        /// Value indicating it is isolated
        /// </summary>
        Isolated = 3,
        /// <summary>
        /// Value indicating it is updating components
        /// </summary>
        UpdatingComponents = 4,
        /// <summary>
        /// Value indicating it is maintenance
        /// </summary>
        Maintenance = 5,
        /// <summary>
        /// Value indicating it is unavailable
        /// </summary>
        Unavailable = 6
    }
}
