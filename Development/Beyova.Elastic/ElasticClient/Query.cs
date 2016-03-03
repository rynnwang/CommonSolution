using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticClient.
    /// </summary>
    partial class ElasticClient
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T GetById<T>(string indexName, string type, string id)
        {
            try
            {
                id.CheckEmptyString("id");

                var httpRequest = (GetHttpRequestUri(indexName, type) + id).CreateHttpWebRequest(HttpConstants.HttpMethod.Get);
                var resultRaw = JsonConvert.DeserializeObject<RawDataItem<T>>(httpRequest.ReadResponseAsText(Encoding.UTF8));
                resultRaw.CheckNullObject("resultRaw");

                return resultRaw.Source;
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetById", new { indexName, type, id });
            }
        }

        /// <summary>
        /// Gets the by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <param name="wildCardSuffix">The wild card suffix.</param>
        /// <returns>T.</returns>
        public T GetByKey<T>(string indexName, string type, Guid? key, string wildCardSuffix = null)
        {
            try
            {
                key.CheckNullObject("key");

                var searchResult = Search<T>(indexName, type, new SearchCriteria
                {
                    QueryCriteria = new QueryCriteria
                    {
                        Terms = new
                        {
                            Key = key.ToString()
                        }.AsList()
                    }
                }, wildCardSuffix);

                if (searchResult != null)
                {
                    var hit = searchResult.Hits.SafeFirstOrDefault();
                    if (hit != null)
                    {
                        return hit.Source;
                    }
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetByKey", new
                {
                    indexName,
                    type,
                    key
                });
            }
        }

        /// <summary>
        /// Searches the specified index name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>QueryResult&lt;T&gt;.</returns>
        public QueryResult<T> Search<T>(string indexName, string type, QueryString queryString)
        {
            try
            {
                queryString.CheckNullObject("queryString");

                var httpRequest = (GetHttpRequestUri(indexName, type) + "_search?" + queryString.ToString()).CreateHttpWebRequest(HttpConstants.HttpMethod.Get);

                var responseText = httpRequest.ReadResponseAsText(Encoding.UTF8);

                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    return ConvertSearchResultJsonToQueryResult<T>(responseText);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("Search", new { indexName, type, queryString });
            }

            return null;
        }

        /// <summary>
        /// Searches the specified index name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="wildCardSuffix">The wild card suffix.</param>
        /// <returns>QueryResult&lt;T&gt;.</returns>
        public QueryResult<T> Search<T>(string indexName, string type, SearchCriteria criteria, string wildCardSuffix = null)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                return InternalSearch<T>(GetFullIndexName(indexName, wildCardSuffix), type, criteria);
            }
            catch (Exception ex)
            {
                throw ex.Handle("Search", new { indexName, type, criteria, wildCardSuffix });
            }
        }

        /// <summary>
        /// Searches the specified index name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteriaJson">The criteria json.</param>
        /// <returns>QueryResult&lt;T&gt;.</returns>
        protected QueryResult<T> InternalSearch<T>(string indexName, string type, SearchCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                var httpRequest = (GetHttpRequestUri(indexName, type) + "_search").CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                httpRequest.FillData(HttpConstants.HttpMethod.Post, criteria.ToJson(), Encoding.UTF8);

                var responseText = httpRequest.ReadResponseAsText(Encoding.UTF8);

                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    return ConvertSearchResultJsonToQueryResult<T>(responseText);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("InternalSearch", new { indexName, type, criteria });
            }

            return null;
        }

        /// <summary>
        /// Converts the search result json to query result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns>List&lt;T&gt;.</returns>
        protected QueryResult<T> ConvertSearchResultJsonToQueryResult<T>(string json)
        {
            //Reference:https://www.elastic.co/guide/en/elasticsearch/reference/current/search-request-body.html

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    var jObject = JObject.Parse(json);

                    var shards = jObject.GetProperty("_shards")?.ToObject<QueryResultShard>();
                    var hitsNode = jObject.GetProperty("hits") as JObject;

                    var hits = hitsNode?.GetProperty("hits").ToObject<List<RawDataItem<T>>>();

                    if (shards != null && hits != null)
                    {
                        var result = new QueryResult<T>
                        {
                            Total = hitsNode.Value<int>("total"),
                            Shards = shards
                        };

                        result.Hits = hits?.ToList();

                        return result;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle("ConvertSearchResultJsonToQueryResult", new { type = typeof(T).FullName, json });
                }
            }

            return null;
        }
    }
}
