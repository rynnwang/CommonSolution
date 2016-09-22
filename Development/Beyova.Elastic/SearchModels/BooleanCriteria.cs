using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class BooleanCriteria.
    /// </summary>
    public class BooleanCriteria
    {
        /// <summary>
        /// Gets or sets the must.
        /// </summary>
        /// <value>The must.</value>
        [JsonProperty(PropertyName = "must", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElasticCriteriaObject> Must { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        [JsonProperty(PropertyName = "filter", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElasticCriteriaObject> Filter { get; set; }

        /// <summary>
        /// Gets or sets the must not.
        /// </summary>
        /// <value>The must not.</value>
        [JsonProperty(PropertyName = "must_not", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElasticCriteriaObject> MustNot { get; set; }

        /// <summary>
        /// Gets or sets the should.
        /// </summary>
        /// <value>The should.</value>
        [JsonProperty(PropertyName = "should", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElasticCriteriaObject> Should { get; set; }

        /// <summary>
        /// Gets or sets the minimum should match.
        /// </summary>
        /// <value>The minimum should match.</value>
        [JsonProperty(PropertyName = "minimum_should_match", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinimumShouldMatch { get; set; }
    }
}
