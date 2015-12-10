using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using ifunction;
using ifunction.ApiTracking.Model;
using ifunction.ExceptionSystem;

namespace Beyova.Elasticsearch.Logger
{
    /// <summary>
    /// Class ElasticsearchLogger.
    /// </summary>
    public class ElasticsearchLogger : IApiTracking
    {
        /// <summary>
        /// The index name
        /// </summary>
        static string indexName = Framework.GetConfiguration("IndexName", "DevTracking");

        /// <summary>
        /// The elasticsearch client
        /// </summary>
        static ElasticsearchClient elasticsearchClient = new ElasticsearchClient(new ConnectionConfiguration(new Uri(Framework.GetConfiguration("ElasticsearchUri"))));

        /// <summary>
        /// The instance
        /// </summary>
        static ElasticsearchLogger instance = new ElasticsearchLogger();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ElasticsearchLogger Instance { get { return instance; } }

        /// <summary>
        /// Logs the API event asynchronous.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void LogApiEventAsync(ApiEventLog eventLog)
        {
            if (elasticsearchClient != null)
            {
                try
                {
                    elasticsearchClient.IndexAsync(indexName, "ApiEvent", eventLog);
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogExceptionAsync(ex.Handle("LogApiEventAsync"));
                }
            }
        }

        /// <summary>
        /// Logs the API trace log asynchronous.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        public void LogApiTraceLogAsync(ApiTraceLog traceLog)
        {
            if (elasticsearchClient != null)
            {
                try
                {
                    elasticsearchClient.IndexAsync(indexName, "ApiTrace", traceLog);
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogExceptionAsync(ex.Handle("LogApiTraceLogAsync"));
                }
            }
        }

        /// <summary>
        /// Logs the exception asynchronous.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="serviceIdentifier">The service identifier.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        public void LogExceptionAsync(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            if (elasticsearchClient != null)
            {
                try
                {
                    elasticsearchClient.IndexAsync(indexName, "Exception", exception.ToExceptionInfo(serviceIdentifier, serverIdentifier));
                }
                catch (Exception ex)
                {
                    Framework.ApiTracking.LogExceptionAsync(ex.Handle("LogExceptionAsync"));
                }
            }
        }
    }
}
