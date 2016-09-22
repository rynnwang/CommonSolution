using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticIndexClient.
    /// </summary>
    public class ElasticIndexClient
    {
        /// <summary>
        /// The wild card suffix
        /// </summary>
        protected const string wildCardSuffix = "-*";

        /// <summary>
        /// Gets or sets the base client.
        /// </summary>
        /// <value>The base client.</value>
        public ElasticClient BaseClient { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the index.
        /// </summary>
        /// <value>The name of the index.</value>
        public string IndexName { get; protected set; }

        /// <summary>
        /// Gets the index suffix.
        /// </summary>
        /// <value>The index suffix.</value>
        public string IndexSuffix { get { return DateTime.UtcNow.ToString("-yyyy-MM-dd"); } }

        #region Cache

        /// <summary>
        /// The cached index full name
        /// </summary>
        protected string cachedIndexFullName;

        /// <summary>
        /// The cached date
        /// </summary>
        protected DateTime? cachedDate;

        /// <summary>
        /// The cached date locker
        /// </summary>
        protected object cachedDateLocker = new object();

        /// <summary>
        /// Gets the full name of the index.
        /// </summary>
        /// <value>The full name of the index.</value>
        public string IndexFullName
        {
            get
            {
                var nowDate = DateTime.UtcNow;
                if (cachedDate == null || cachedDate.Value.DayOfYear != nowDate.DayOfYear)
                {
                    lock (cachedDateLocker)
                    {
                        if (cachedDate == null || cachedDate.Value.DayOfYear != nowDate.DayOfYear)
                        {
                            cachedDate = nowDate;
                            cachedIndexFullName = IndexName + IndexSuffix;
                            return cachedIndexFullName;
                        }
                    }
                }

                return cachedIndexFullName;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticIndexClient" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="indexName">Name of the index.</param>
        public ElasticIndexClient(string baseUrl, string indexName)
        {
            this.BaseClient = new ElasticClient(baseUrl);

            //Elasticsearch index only allow lower cases.
            this.IndexName = indexName.SafeToLower();
        }

        #region Index

        /// <summary>
        /// Indexes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="workObject">The work object.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> IndexAsync<T, F>(IElasticWorkObject<T, F> workObject, int? timeout = null)
        {
            if (workObject != null && !string.IsNullOrWhiteSpace(workObject.Type))
            {
                workObject.IndexName = IndexFullName;
                return await BaseClient.IndexAsync(workObject, timeout);
            }

            return null;
        }

        /// <summary>
        /// Indexes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="workObject">The work object.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>System.String.</returns>
        public string Index<T, F>(IElasticWorkObject<T, F> workObject, int? timeout = null)
        {
            if (workObject != null && !string.IsNullOrWhiteSpace(workObject.Type))
            {
                workObject.IndexName = IndexFullName;
                return BaseClient.Index(workObject, timeout);
            }

            return null;
        }

        /// <summary>
        /// Deletes the index.
        /// </summary>
        public void DeleteIndex()
        {
            try
            {
                BaseClient.InternalDeleteIndex(IndexName + wildCardSuffix);
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }



        #endregion

        #region Get

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T GetById<T>(string type, string id)
        {
            try
            {
                type.CheckEmptyString("type");
                id.CheckEmptyString("id");

                return BaseClient.GetById<T>(IndexName, type, id);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type, id });
            }
        }

        /// <summary>
        /// Gets the by elastic identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>RawDataItem&lt;T&gt;.</returns>
        public RawDataItem<T> GetByElasticId<T>(string type, string id)
        {
            try
            {
                type.CheckEmptyString("type");
                id.CheckEmptyString("id");

                return BaseClient.GetByElasticId<T>(IndexName, type, id, wildCardSuffix);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type, id });
            }
        }

        /// <summary>
        /// Gets the by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public T GetByKey<T>(string type, Guid? key)
        {
            try
            {
                type.CheckEmptyString("type");
                key.CheckNullObject("key");

                return BaseClient.GetByKey<T>(IndexName, type, key, wildCardSuffix);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type, key });
            }
        }

        #endregion

        #region Query

        /// <summary>
        /// Queries the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>QueryResult&lt;T&gt;.</returns>
        public QueryResult<T> Query<T>(string type, SearchCriteria criteria)
        {
            try
            {
                type.CheckEmptyString("type");
                criteria.CheckNullObject("criteria");

                return BaseClient.Search<T>(IndexName, type, criteria, wildCardSuffix);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type, criteria });
            }
        }

        /// <summary>
        /// Gets the query HTTP request raw.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public string GetQueryHttpRequestRaw(string type, SearchCriteria criteria)
        {
            try
            {
                type.CheckEmptyString("type");
                criteria.CheckNullObject("criteria");

                return BaseClient.GetSearchHttpRequestRaw(IndexName, type, criteria, wildCardSuffix);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type, criteria });
            }
        }

        /// <summary>
        /// Aggregations the query.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>AggregationsQueryResult.</returns>
        public AggregationsQueryResult<T> AggregationQuery<T>(string type, SearchCriteria criteria)
        {
            try
            {
                type.CheckEmptyString("type");
                criteria.CheckNullObject("criteria");

                return BaseClient.AggregationSearch<T>(IndexName + wildCardSuffix, type, criteria);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { type, criteria });
            }
        }

        #endregion

        #region Status

        /// <summary>
        /// Gets the indices status.
        /// </summary>
        /// <returns></returns>
        public List<ElasticIndicesStatus> GetIndicesStatus()
        {
            try
            {
                return BaseClient.GetIndicesStatus();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion
    }
}
