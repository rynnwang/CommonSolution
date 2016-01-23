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
        /// <param name="indexName">Name of the index.</param>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> IndexAsync(string indexName, string type, object data)
        {
            try
            {
                data.CheckNullObject("data");

                var httpRequest = GetHttpRequestUri(indexName, type).CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                await httpRequest.FillDataAsync(data.ToJson(false), Encoding.UTF8, "application/json");

                return await httpRequest.ReadResponseAsTextAsync(Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw ex.Handle("IndexAsync", new { indexName, type, data });
            }
        }
    }
}
