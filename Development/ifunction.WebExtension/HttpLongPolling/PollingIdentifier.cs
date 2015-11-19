using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ifunction.ExceptionSystem;

namespace ifunction.WebExtension.HttpLongPolling
{
    /// <summary>
    /// Class PollingIdentifier.
    /// </summary>
    public class PollingIdentifier
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Identifier;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().ToLowerInvariant().GetHashCode();
        }

        /// <summary>
        /// Creates the HTTP request.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="action">The action.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>HttpWebRequest.</returns>
        public HttpWebRequest CreateHttpRequest(string uri, string from, string to, PollingAction action, string jsonBody = null, int? batchSize = null)
        {
            HttpWebRequest request = uri.CreateHttpWebRequest("GET");

            request.Headers.Set(HeaderKeys.headerKey_From, from);
            request.Headers.Set(HeaderKeys.headerKey_To, to);
            request.Headers.Set(HeaderKeys.headerKey_Action, action.ToString());
            request.Headers.Set(HeaderKeys.headerKey_BatchSize, batchSize.ToString());
            request.Timeout = Timeout.Infinite;

            if (!string.IsNullOrWhiteSpace(jsonBody))
            {
                request.FillData("POST", jsonBody.SafeToString());
            }

            return request;
        }

        /// <summary>
        /// Pushes the message
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="to">To.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <returns>System.String.</returns>
        public string Push(string uri, string to, string jsonBody)
        {
            try
            {
                var request = CreateHttpRequest(uri, this.Identifier, to, PollingAction.Push, jsonBody);
                return request.ReadResponseAsText();
            }
            catch (Exception ex)
            {
                throw ex.Handle("Push", new { Uri = uri, To = to, JsonBody = jsonBody, From = this.Identifier });
            }
        }

        /// <summary>
        /// Fetches the message amount for pull.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="from">From.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">Fetch</exception>
        public string Fetch(string uri, string from)
        {
            try
            {
                var request = CreateHttpRequest(uri, from, this.Identifier, PollingAction.Fetch);
                return request.ReadResponseAsText();
            }
            catch (Exception ex)
            {
                throw ex.Handle("Fetch", new { Uri = uri, To = this.Identifier, From = from });
            }
        }

        /// <summary>
        /// Launches the long polling.
        /// </summary>
        /// <typeparam name="TFuncResult">The type of the t function result.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="from">From.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>``0.</returns>
        /// <exception cref="OperationFailureException">LaunchLongPolling</exception>
        public TFuncResult Pull<TFuncResult>(string uri, string from, int batchSize, string jsonBody, Func<string, WebHeaderCollection, CookieCollection, TFuncResult> callback)
        {
            TFuncResult result = default(TFuncResult);

            try
            {
                var request = CreateHttpRequest(uri, from, this.Identifier, PollingAction.Pull, jsonBody, batchSize);

                request.ProceedResponseAsync((response) =>
                {
                    string json = response.ReadAsText();
                    result = callback(json, response.Headers, ((HttpWebResponse)response).Cookies);
                });
            }
            catch (Exception ex)
            {
                // If the connection is closed by server side, it is not a exception.
                SocketException socketEx = ex.RootException() as SocketException;
                if (socketEx == null)
                {
                    throw ex.Handle("LaunchLongPolling", new { Uri = uri, To = this.Identifier, JsonBody = jsonBody, From = from });
                }
            }

            return result;
        }
    }
}
