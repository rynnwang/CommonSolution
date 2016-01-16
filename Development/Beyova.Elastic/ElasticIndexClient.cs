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
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> IndexAsync(string type, object data)
        {
            try
            {
                type.CheckEmptyString("type");
                data.CheckNullObject("data");

                return BaseClient.IndexAsync(IndexName + IndexSuffix, type, data);
            }
            catch (Exception ex)
            {
                throw ex.Handle("IndexAsync", new { type, data });
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
                throw ex.Handle("GetById", new { type, id });
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
                throw ex.Handle("GetByKey", new { type, key });
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
        public QueryResult<T> Query<T>(string type, object criteria)
        {
            try
            {
                type.CheckEmptyString("type");
                criteria.CheckNullObject("criteria");

                return BaseClient.Search<T>(IndexName + wildCardSuffix, type, criteria);
            }
            catch (Exception ex)
            {
                throw ex.Handle("Query", new { type, criteria });
            }
        }

        #endregion
    }
}
