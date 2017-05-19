using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticClient.
    /// </summary>
    partial class ElasticClient
    {
        /// <summary>
        /// Gets the indices status.
        /// </summary>
        /// <returns></returns>
        public List<ElasticIndicesStatus> GetIndicesStatus()
        {
            try
            {
                var httpRequest = GetUnIndexedHttpRequestUri("_stats").CreateHttpWebRequest(HttpConstants.HttpMethod.Get);
                var responseJson = JToken.Parse(httpRequest.ReadResponseAsText());

                //ElasticStatus<ElasticIndicesStatus> result = new ElasticStatus<ElasticIndicesStatus>();
                //FillShardInfo(result, responseJson);

                return new List<ElasticIndicesStatus>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the un indexed HTTP request URI.
        /// </summary>
        /// <param name="pathAndQuery">The path and query.</param>
        /// <returns></returns>
        protected string GetUnIndexedHttpRequestUri(string pathAndQuery)
        {
            const string fullUrl = "{0}/{1}";

            try
            {
                return string.Format(fullUrl, BaseUrl, pathAndQuery);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { pathAndQuery });
            }
        }

        /// <summary>
        /// Fills the shard information.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusObject">The status object.</param>
        /// <param name="json">The json.</param>
        protected void FillShardInfo<T>(ElasticStatus<T> statusObject, JToken json)
        {
            if (json != null && statusObject != null)
            {
                var shards = json.SelectToken("_shards");
                if (shards != null)
                {
                    statusObject.TotalShards = shards.Value<long>("total");
                    statusObject.SuccessfulShards = shards.Value<long>("successful");
                    statusObject.FailedShards = shards.Value<long>("failed");
                }
            }
        }

        /// <summary>
        /// Reads the elastic indices status.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        protected List<ElasticIndicesStatus> ReadElasticIndicesStatusAsList(JToken json)
        {
            return new List<ElasticIndicesStatus>();
        }

        /// <summary>
        /// Reads the elastic indices status.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>List&lt;ElasticIndicesStatus&gt;.</returns>
        protected List<ElasticIndicesStatus> ReadElasticIndicesStatus(JToken json)
        {
            return new List<ElasticIndicesStatus>();
        }
    }
}
