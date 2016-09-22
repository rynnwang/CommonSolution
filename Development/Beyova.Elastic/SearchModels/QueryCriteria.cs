using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class QueryCriteria.
    /// </summary>
    public class QueryCriteria
    {
        /// <summary>
        /// Gets or sets from index.
        /// </summary>
        /// <value>From index.</value>
        [JsonProperty(PropertyName = "from", NullValueHandling = NullValueHandling.Ignore)]
        public int? FromIndex { get; set; }

        /// <summary>
        /// Gets or sets the terms.
        /// </summary>
        /// <value>The terms.</value>
        [JsonProperty(PropertyName = "term", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Terms { get; set; }

        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        /// <value>The match.</value>
        [JsonProperty(PropertyName = "match", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Matches { get; set; }

        /// <summary>
        /// Gets or sets the phrase matches.
        /// </summary>
        /// <value>The phrase matches.</value>
        [JsonProperty(PropertyName = "match_phrase", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> PhraseMatches { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        [JsonProperty(PropertyName = "filtered", NullValueHandling = NullValueHandling.Ignore)]
        public FilterCriteria Filters { get; set; }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
        [JsonProperty(PropertyName = "range", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, object>> Range { get; set; }

        /// <summary>
        /// Gets or sets the boolean criteria.
        /// </summary>
        /// <value>The boolean criteria.</value>
        [JsonProperty(PropertyName = "bool", NullValueHandling = NullValueHandling.Ignore)]
        public BooleanCriteria BooleanCriteria { get; set; }
    }
}
