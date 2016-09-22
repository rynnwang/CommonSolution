using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticCriteriaObject.
    /// </summary>
    [JsonConverter(typeof(ElasticCriteriaObjectSerializer))]
    public class ElasticCriteriaObject
    {
        /// <summary>
        /// Gets or sets the type of the criteria.
        /// </summary>
        /// <value>The type of the criteria.</value>
        public string CriteriaType { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the criteria value.
        /// </summary>
        /// <value>The criteria value.</value>
        public object CriteriaValue { get; set; }
    }
}
