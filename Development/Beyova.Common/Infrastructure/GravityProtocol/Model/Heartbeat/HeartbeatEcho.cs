using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class HeartbeatEcho.
    /// </summary>
    public class HeartbeatEcho
    {
        /// <summary>
        /// Gets or sets the command requests.
        /// </summary>
        /// <value>The command requests.</value>
        public List<GravityCommandRequest> CommandRequests { get; set; }

        /// <summary>
        /// Gets or sets the client key.
        /// </summary>
        /// <value>The client key.</value>
        public Guid? ClientKey { get; set; }
    }
}
