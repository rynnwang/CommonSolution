using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    partial class ElasticClient
    {
        /// <summary>
        /// Indexes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="indexObject">The index object.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> IndexAsync<T, F>(IElasticWorkObject<T, F> indexObject, int? timeout = null)
        {
            try
            {
                indexObject.CheckNullObject("indexObject");

                if (!string.IsNullOrWhiteSpace(indexObject.IndexName)
                  && !string.IsNullOrWhiteSpace(indexObject.Type)
                  && indexObject.RawData != null)
                {
                    var httpRequest = GetHttpRequestUri(indexObject.IndexName, indexObject.Type).CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpRequest.Timeout = timeout ?? 10000;//10sec for timeout as default.
                    await httpRequest.FillDataAsync(indexObject.RawData.ToJson(false), Encoding.UTF8, "application/json");

                    return await httpRequest.ReadResponseAsTextAsync(Encoding.UTF8);
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Indexes the specified index name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="indexObject">The index object.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>System.String.</returns>
        public string Index<T, F>(IElasticWorkObject<T, F> indexObject, int? timeout = null)
        {
            try
            {
                indexObject.CheckNullObject("indexObject");

                if (!string.IsNullOrWhiteSpace(indexObject.IndexName)
                    && !string.IsNullOrWhiteSpace(indexObject.Type)
                    && indexObject.RawData != null)
                {
                    var httpRequest = GetHttpRequestUri(indexObject.IndexName, indexObject.Type).CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpRequest.Timeout = timeout ?? 10000;//10sec for timeout as default.
                    httpRequest.FillData(indexObject.RawData.ToJson(false), Encoding.UTF8, "application/json");

                    return httpRequest.ReadResponseAsText(Encoding.UTF8);
                }
            }
            catch { }

            return null;
        }
    }
}
