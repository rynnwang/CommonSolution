using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.RestApi;

namespace Beyova
{
    /// <summary>
    /// Class EnvironmentEndpointCriteria.
    /// </summary>
    public class EnvironmentEndpointCriteria : EnvironmentEndpoint, ICriteria
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get; set;
        }
    }
}
