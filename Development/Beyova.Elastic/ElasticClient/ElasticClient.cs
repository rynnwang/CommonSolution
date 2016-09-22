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
    /// Class ElasticClient.
    /// </summary>
    public partial class ElasticClient
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public ElasticClient(string baseUrl)
        {
            this.BaseUrl = baseUrl?.TrimEnd('/');
        }

        /// <summary>
        /// Gets the HTTP request URI. Uri is end with "/"
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        protected string GetHttpRequestUri(string indexName, string type)
        {
            const string fullUrl = "{0}/{1}/{2}/";

            try
            {
                indexName.CheckEmptyString("indexName");

                return string.Format(fullUrl, BaseUrl, indexName.ToUrlEncodedText(), type);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type });
            }
        }

        /// <summary>
        /// Creates the HTTP request.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpWebRequest.</returns>
        protected HttpWebRequest CreateHttpRequest(string indexName, string type, string httpMethod, string id = null)
        {
            const string fullUrl = "{0}/{1}/{2}/{3}";

            try
            {
                indexName.CheckEmptyString("indexName");
                type.CheckEmptyString("type");
                httpMethod.CheckEmptyString("httpMethod");

                return string.Format(fullUrl, BaseUrl, indexName, type, id).TrimEnd('/').CreateHttpWebRequest(httpMethod);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName, type, httpMethod });
            }
        }

        /// <summary>
        /// Gets the full name of the index.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>System.String.</returns>
        protected string GetFullIndexName(string indexName, string suffix)
        {
            return string.IsNullOrWhiteSpace(suffix) ? indexName : (indexName.SafeToString() + suffix);
        }

        /// <summary>
        /// Internals the index of the delete.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        internal void InternalDeleteIndex(string indexName)
        {
            try
            {
                indexName.CheckEmptyString("indexName");

                var httpRequest = GetHttpRequestUri(indexName, string.Empty).TrimEnd('/').CreateHttpWebRequest(HttpConstants.HttpMethod.Delete);

                var responseText = httpRequest.ReadResponseAsText(Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { indexName });
            }
        }
    }
}
