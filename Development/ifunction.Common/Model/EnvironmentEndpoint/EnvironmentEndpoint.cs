using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.Model;
using ifunction.RestApi;

namespace ifunction.Model
{
    /// <summary>
    /// Class EnvironmentEndpointCriteria.
    /// </summary>
    public class EnvironmentEndpointCriteria : EnvironmentEndpointBase, ICriteria
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
