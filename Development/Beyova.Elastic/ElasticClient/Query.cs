using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ApiTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Delegate searchResultConvertDelegate
    /// </summary>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonString">The json string.</param>
    /// <returns>QueryResult&lt;T&gt;.</returns>
    public delegate TResult SearchResultConvertDelegate<TResult, T>(string jsonString);

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
                throw ex.Handle(new { indexName, type, id });
            }
        }

        /// <summary>
        /// Gets the by elastic identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="wildCardSuffix">The wild card suffix.</param>
        /// <returns>T.</returns>
        public RawDataItem<T> GetByElasticId<T>(string indexName, string type, string id, string wildCardSuffix = null)
        {
            try
            {
                id.CheckEmptyString("id");

                return (InternalSearch<T>(GetFullIndexName(indexName, wildCardSuffix), type, new SearchCriteria
                {
                    QueryCriteria = new QueryCriteria
                    {
                        PhraseMatches = new Dictionary<string, object> { { "_id", id } }
                    }
                })?.Hits?.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, id });
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
                        PhraseMatches = new Dictionary<string, object> { { "Key", key.ToString() } }
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
                throw ex.Handle(new
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
                throw ex.Handle(new { indexName, type, queryString });
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
        /// <returns>Beyova.Elastic.QueryResult&lt;T&gt;.</returns>
        public QueryResult<T> Search<T>(string indexName, string type, SearchCriteria criteria, string wildCardSuffix = null)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                return InternalSearch<T>(GetFullIndexName(indexName, wildCardSuffix), type, criteria);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria, wildCardSuffix });
            }
        }

        /// <summary>
        /// Gets the search HTTP request raw.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="wildCardSuffix">The wild card suffix.</param>
        /// <returns></returns>
        public string GetSearchHttpRequestRaw(string indexName, string type, SearchCriteria criteria, string wildCardSuffix = null)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                return InternalGetSearchHttpRequestRaw(GetFullIndexName(indexName, wildCardSuffix), type, criteria);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria, wildCardSuffix });
            }
        }

        /// <summary>
        /// Aggregations the search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="wildCardSuffix">The wild card suffix.</param>
        /// <returns>Beyova.Elastic.AggregationsQueryResult&lt;T&gt;.</returns>
        public AggregationsQueryResult<T> AggregationSearch<T>(string indexName, string type, SearchCriteria criteria, string wildCardSuffix = null)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                return InternalAggregationSearch<T>(GetFullIndexName(indexName, wildCardSuffix), type, criteria);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria, wildCardSuffix });
            }
        }

        /// <summary>
        /// Internals the search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Beyova.Elastic.QueryResult&lt;T&gt;.</returns>
        protected QueryResult<T> InternalSearch<T>(string indexName, string type, SearchCriteria criteria)
        {
            try
            {
                return InternalSearch<QueryResult<T>, T>(indexName, type, criteria, ConvertSearchResultJsonToQueryResult<T>);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria });
            }
        }

        /// <summary>
        /// Internals the aggregation search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Beyova.Elastic.AggregationsQueryResult&lt;T&gt;.</returns>
        protected AggregationsQueryResult<T> InternalAggregationSearch<T>(string indexName, string type, SearchCriteria criteria)
        {
            try
            {
                return InternalSearch<AggregationsQueryResult<T>, object>(indexName, type, criteria, ConvertSearchResultJsonToAggregationResult<T>);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria });
            }
        }

        /// <summary>
        /// Internals the search.
        /// </summary>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="convertDelegate">The convert delegate.</param>
        /// <returns>TResult.</returns>
        protected TResult InternalSearch<TResult, T>(string indexName, string type, SearchCriteria criteria, SearchResultConvertDelegate<TResult, T> convertDelegate)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var httpRequest = (GetHttpRequestUri(indexName, type) + "_search").CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                httpRequest.FillData(HttpConstants.HttpMethod.Post, criteria.ToJson(), Encoding.UTF8);

                var responseText = httpRequest.ReadResponseAsText(Encoding.UTF8);

                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    return convertDelegate(responseText);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria });
            }

            return default(TResult);
        }

        /// <summary>
        /// Internals the get search HTTP request raw.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.String.</returns>
        protected string InternalGetSearchHttpRequestRaw(string indexName, string type, SearchCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var httpRequest = (GetHttpRequestUri(indexName, type) + "_search").CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                httpRequest.FillData(HttpConstants.HttpMethod.Post, criteria.ToJson(), Encoding.UTF8);

                return httpRequest.ToRaw();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, criteria });
            }
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
                    throw ex.Handle(new { type = typeof(T).FullName, json });
                }
            }

            return null;
        }

        /// <summary>
        /// Converts the search result json to aggregation result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns>Beyova.Elastic.AggregationsQueryResult.</returns>
        protected AggregationsQueryResult<T> ConvertSearchResultJsonToAggregationResult<T>(string json)
        {
            //Reference:https://www.elastic.co/guide/en/elasticsearch/reference/current/search-request-body.html

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    var jObject = JObject.Parse(json);

                    var shards = jObject.GetProperty("_shards")?.ToObject<QueryResultShard>();
                    var hitsNode = jObject.GetProperty("hits") as JObject;
                    var aggregationNodes = jObject.GetProperty("aggregations") as JObject;

                    if (shards != null && aggregationNodes != null)
                    {
                        var bucketJson = aggregationNodes.Properties().FirstOrDefault();

                        if (bucketJson != null)
                        {
                            var result = new AggregationsQueryResult<T>
                            {
                                Total = hitsNode?.Value<int>("total") ?? 0,
                                Shards = shards,
                                Aggregations = new MatrixList<AggregationGroupObject<T>>()
                            };
                            result.Aggregations.Add(bucketJson.Name, (bucketJson.Value as JObject).ConvertToAggregationBucketObject<T>(bucketJson.Name));
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { json });
                }
            }

            return null;
        }
    }
}
