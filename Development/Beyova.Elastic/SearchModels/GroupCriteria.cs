using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class GroupCriteria.
    /// </summary>
    public class GroupCriteria : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCriteria"/> class.
        /// </summary>
        public GroupCriteria() : base()
        {

        }
    }
}
