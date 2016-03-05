using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class FilterCriteria.
    /// </summary>
    public class FilterCriteria
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [JsonProperty(PropertyName = "filter", NullValueHandling = NullValueHandling.Ignore)]
        public object Items { get; set; }
    }
}
