using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.WebExtension.HttpLongPolling
{
    /// <summary>
    /// Enum PollingAction
    /// </summary>
    public enum PollingAction
    {
        /// <summary>
        /// Value indicating that action is none
        /// </summary>
        None = 0,
        /// <summary>
        /// Value indicating that action is fetch
        /// </summary>
        Fetch,
        /// <summary>
        /// Value indicating that action is pull
        /// </summary>
        Pull,
        /// <summary>
        /// Value indicating that action is push
        /// </summary>
        Push
    }
}
