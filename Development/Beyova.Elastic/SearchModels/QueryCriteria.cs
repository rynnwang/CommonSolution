﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class SearchCriteria.
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
        public object Terms { get; set; }

        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        /// <value>The match.</value>
        [JsonProperty(PropertyName = "match", NullValueHandling = NullValueHandling.Ignore)]
        public object Matches { get; set; }

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
        public object Range { get; set; }
    }
}
