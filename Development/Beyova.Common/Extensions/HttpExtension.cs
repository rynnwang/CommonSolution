using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Beyova.ExceptionSystem;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Beyova;
using Beyova.RestApi;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Beyova
{
    /// <summary>
    /// Extension class for http operations.
    /// </summary>
    public static class HttpExtension
    {
        /// <summary>
        /// Initializes static members of the <see cref="HttpExtension"/> class.
        /// </summary>
        static HttpExtension()
        {
            // ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        #region Uri and Credential

        /// <summary>
        /// The domain credential regex
        /// </summary>
        static Regex domainCredentialRegex = new Regex(@"^((?<Domain>([0-9a-zA-Z_-]+))\\)?(?<AccessIdentifier>([0-9a-zA-Z\._-]+))(:(?<Token>(.+)))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The URI credential regex
        /// </summary>
        static Regex uriCredentialRegex = new Regex(@"^((?<AccessIdentifier>([0-9a-zA-Z\._-]+))(:(?<Token>(.+)))?@(?<Domain>([0-9a-zA-Z_\.-]+)))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the pure URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="userInfo">The user information.</param>
        /// <returns>System.String.</returns>
        public static string GetPureUri(this Uri uri, out string userInfo)
        {
            userInfo = uri.UserInfo;
            return string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, uri.PathAndQuery);
        }

        /// <summary>
        /// To the full raw URL.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.String.</returns>
        public static string ToFullRawUrl(this HttpRequest httpRequest)
        {
            return httpRequest == null ? string.Empty : string.Format("{0}: {1}", httpRequest.HttpMethod, httpRequest.RawUrl);
        }

        /// <summary>
        /// To the full raw URL.
        /// </summary>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <returns>System.String.</returns>
        public static string ToFullRawUrl(this RuntimeContext runtimeContext)
        {
            return runtimeContext == null ? string.Empty : (string.Format("/{0}/{1}/{2}/{3}/", runtimeContext.ApiMethod, runtimeContext.Version, runtimeContext.ResourceName, runtimeContext.ActionName).TrimEnd('/') + "/");
        }

        /// <summary>
        /// Parses to access credential. Acceptable string samples: cnsh\rynn.wang:12345, cnsh\rynn.wang, rynn.wang:12345, rynn.wang, rynn.wang@cnsh, rynn.wang:12345@cnsh
        /// </summary>
        /// <param name="accountString">The account string.</param>
        /// <returns>AccessCredential.</returns>
        public static AccessCredential ParseToAccessCredential(this string accountString)
        {
            if (!string.IsNullOrWhiteSpace(accountString))
            {
                var match = domainCredentialRegex.Match(accountString);

                if (match.Success)
                {
                    return new AccessCredential
                    {
                        Token = match.Result("${Token}"),
                        AccessIdentifier = match.Result("${AccessIdentifier}"),
                        Domain = match.Result("${Domain}")
                    };
                }

                match = uriCredentialRegex.Match(accountString);
                if (match.Success)
                {
                    return new AccessCredential
                    {
                        Token = match.Result("${Token}"),
                        AccessIdentifier = match.Result("${AccessIdentifier}"),
                        Domain = match.Result("${Domain}")
                    };
                }
            }

            return null;
        }

        #endregion

        #region Read response

        /// <summary>
        /// Reads the response as t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="responseDelegate">The response delegate.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>T.</returns>
        private static T ReadResponseAsT<T>(this HttpWebRequest httpWebRequest, Func<WebResponse, T> responseDelegate, out HttpStatusCode statusCode, out WebHeaderCollection headers, out CookieCollection cookieCollection)
        {
            statusCode = default(HttpStatusCode);
            headers = null;
            T result = default(T);
            string destinationMachine = null;
            cookieCollection = null;

            if (httpWebRequest != null)
            {
                WebResponse response = null;
                HttpWebResponse webResponse = null;

                try
                {
                    response = httpWebRequest.GetResponse();

                    webResponse = (HttpWebResponse)response;
                    webResponse.CheckNullObject(nameof(webResponse));

                    statusCode = webResponse.StatusCode;
                    headers = webResponse.Headers;
                    destinationMachine = headers?.Get(HttpConstants.HttpHeader.SERVERNAME);
                    cookieCollection = webResponse.Cookies;

                    if (responseDelegate != null)
                    {
                        result = responseDelegate(response);
                    }
                }
                catch (WebException webEx)
                {
                    webResponse = (HttpWebResponse)webEx.Response;
                    if (webResponse != null)
                    {
                        statusCode = webResponse.StatusCode;
                        headers = webResponse.Headers;
                        destinationMachine = headers?.Get(HttpConstants.HttpHeader.SERVERNAME);
                        cookieCollection = webResponse.Cookies;

                        var responseText = webResponse.ReadAsText();

                        throw new HttpOperationException(httpWebRequest.RequestUri.ToString(),
                            httpWebRequest.Method,
                            webEx.Message,
                            httpWebRequest.ContentLength,
                            responseText,
                            webResponse.StatusCode,
                            webEx.Status, destinationMachine);
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpOperationException(httpWebRequest.RequestUri.ToString(),
                        httpWebRequest.Method,
                        httpWebRequest.ContentLength,
                        webResponse.StatusCode,
                        ex as BaseException, destinationMachine);
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Reads the response as text.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadResponseAsText</exception>
        public static string ReadResponseAsText(this HttpWebRequest httpWebRequest, Encoding encoding, out HttpStatusCode statusCode, out WebHeaderCollection headers, out CookieCollection cookieCollection)
        {
            return ReadResponseAsT<string>(httpWebRequest, (webResponse) => { return HttpExtension.ReadAsText(webResponse, encoding, false); }, out statusCode, out headers, out cookieCollection);
        }

        /// <summary>
        /// Reads the response as bytes.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ReadResponseAsBytes(this HttpWebRequest httpWebRequest, out HttpStatusCode statusCode, out WebHeaderCollection headers, out CookieCollection cookieCollection)
        {
            return ReadResponseAsT<byte[]>(httpWebRequest, (webResponse) => { return HttpExtension.InternalReadAsBytes(webResponse, false); }, out statusCode, out headers, out cookieCollection);
        }

        /// <summary>
        /// Reads the response as text.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string ReadResponseAsText(this HttpWebRequest httpWebRequest, Encoding encoding = null)
        {
            CookieCollection cookieCollection;
            WebHeaderCollection headers;
            HttpStatusCode statusCode;
            return ReadResponseAsText(httpWebRequest, encoding, out statusCode, out headers, out cookieCollection);
        }

        /// <summary>
        /// Reads the response as bytes.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ReadResponseAsBytes(this HttpWebRequest httpWebRequest)
        {
            CookieCollection cookieCollection;
            WebHeaderCollection headers;
            HttpStatusCode statusCode;
            return ReadResponseAsBytes(httpWebRequest, out statusCode, out headers, out cookieCollection);
        }

        #region As Text Async

        /// <summary>
        /// Reads the response as text asynchronous.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> ReadResponseAsTextAsync(this HttpWebRequest httpWebRequest, Encoding encoding = null, int? timeout = null)
        {
            try
            {
                httpWebRequest.CheckNullObject("httpWebRequest");

                if (timeout != null)
                {
                    httpWebRequest.Timeout = timeout.Value;
                }

                var response = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
                return response.InternalReadAsText(encoding, true);
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Reads the asynchronous response as text.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="OperationFailureException">ReadResponseAsTextAsTraditionalAsync</exception>
        public static void ReadResponseAsTextUsingTraditionalAsync(this HttpWebRequest httpWebRequest, Action<string> callback, Encoding encoding = null, int? timeout = null)
        {
            if (httpWebRequest != null)
            {
                if (timeout != null)
                {
                    httpWebRequest.Timeout = timeout.Value;
                }

                try
                {
                    httpWebRequest.BeginGetResponse(new AsyncCallback(delegate (IAsyncResult asnyc)
                    {
                        var response = (asnyc.AsyncState as HttpWebRequest).EndGetResponse(asnyc) as HttpWebResponse;

                        if (response != null)
                        {
                            try
                            {
                                if (callback != null)
                                {
                                    callback(response.ReadAsText(encoding));
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex.Handle();
                            }
                            finally
                            {
                                response.Close();
                            }
                        }
                    }), httpWebRequest);
                }
                catch (Exception ex)
                {
                    throw ex.Handle();
                }
            }
        }

        #endregion

        #region WebResponse Extension

        /// <summary>
        /// Reads as text. This method would try to detect GZip and decode it correctly.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeResponse">The close response.</param>
        /// <returns>System.String.</returns>
        public static string ReadAsText(this WebResponse webResponse, Encoding encoding = null, bool closeResponse = true)
        {
            try
            {
                return webResponse.IsGZip() ?
                    webResponse.InternalReadAsGZipText(encoding ?? Encoding.UTF8, closeResponse)
                    : webResponse.InternalReadAsText(encoding, closeResponse);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { encoding = encoding?.EncodingName, closeResponse });
            }
        }

        /// <summary>
        /// Reads as text.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadAsText</exception>
        private static string InternalReadAsText(this WebResponse webResponse, Encoding encoding, bool closeResponse)
        {
            string result = string.Empty;

            if (webResponse != null)
            {
                try
                {
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        var streamReader = new StreamReader(responseStream, (encoding ?? Encoding.UTF8), true);
                        result = streamReader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { encoding = encoding?.EncodingName, closeResponse });
                }
                finally
                {
                    if (closeResponse)
                    {
                        webResponse.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Reads as GZip text.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">ReadAsGZipText</exception>
        private static string InternalReadAsGZipText(this WebResponse webResponse, Encoding encoding, bool closeResponse)
        {
            var stringBuilder = new StringBuilder();

            if (webResponse != null)
            {
                try
                {
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        using (var gZipStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            var buffer = new byte[2048];
                            var length = gZipStream.Read(buffer, 0, 2048);
                            while (length > 0)
                            {
                                stringBuilder.Append((encoding ?? Encoding.UTF8).GetString(buffer, 0, length));
                                length = gZipStream.Read(buffer, 0, 2048);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { encoding = encoding?.EncodingName, closeResponse });
                }
                finally
                {
                    if (closeResponse)
                    {
                        webResponse.Close();
                    }
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Reads as bytes.
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <param name="closeResponse">if set to <c>true</c> [close response].</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="OperationFailureException">ReadAsBytes</exception>
        private static byte[] InternalReadAsBytes(this WebResponse webResponse, bool closeResponse)
        {
            byte[] result = null;

            if (webResponse != null)
            {
                try
                {
                    using (Stream responseStream = webResponse.GetResponseStream())
                    {
                        result = responseStream.ToBytes();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { closeResponse });
                }
                finally
                {
                    if (closeResponse)
                    {
                        webResponse.Close();
                    }
                }
            }

            return result;
        }

        #endregion

        #endregion

        #region Fill Data On HttpWebRequest

        /// <summary>
        /// Fills the file data.
        /// <remarks>
        /// Reference: http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        /// </remarks>
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileCollection">The file collection.
        /// Key: file name. e.g.: sample.txt
        /// Value: file data in byte array.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static void FillFileData(this HttpWebRequest httpWebRequest, NameValueCollection postData, Dictionary<string, byte[]> fileCollection, string paramName)
        {
            try
            {
                var boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

                httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

                using (var stream = new MemoryStream())
                {
                    var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                    var formDataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                    if (postData != null)
                    {
                        foreach (string key in postData.Keys)
                        {
                            var formItem = string.Format(formDataTemplate, key, postData[key]);
                            var formItemBytes = Encoding.UTF8.GetBytes(formItem);
                            stream.Write(formItemBytes, 0, formItemBytes.Length);
                        }
                    }

                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

                    if (fileCollection != null)
                    {
                        foreach (var key in fileCollection.Keys)
                        {
                            var header = string.Format(headerTemplate, paramName, key);
                            var headerBytes = Encoding.UTF8.GetBytes(header);
                            stream.Write(headerBytes, 0, headerBytes.Length);

                            stream.Write(fileCollection[key], 0, fileCollection[key].Length);

                            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        }
                    }

                    httpWebRequest.ContentLength = stream.Length;
                    stream.Position = 0;
                    var tempBuffer = new byte[stream.Length];
                    stream.Read(tempBuffer, 0, tempBuffer.Length);

                    using (var requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                    }

                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(fileCollection);
            }
        }

        /// <summary>
        /// Fills the file data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileFullName">Full name of the file.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="OperationFailureException">FillFileData</exception>
        public static void FillFileData(this HttpWebRequest httpWebRequest, NameValueCollection postData, string fileFullName, string paramName)
        {
            if (httpWebRequest != null && !string.IsNullOrWhiteSpace(fileFullName))
            {
                try
                {
                    var fileData = File.ReadAllBytes(fileFullName);
                    var fileName = Path.GetFileName(fileFullName);

                    var fileCollection = new Dictionary<string, byte[]> { { fileName, fileData } };

                    FillFileData(httpWebRequest, postData, fileCollection, paramName);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { fileFullName, paramName });
                }
            }
        }

        /// <summary>
        /// Fills post data on HttpWebRequest.
        /// </summary>
        /// <param name="httpWebRequest">The HttpWebRequest instance.</param>
        /// <param name="method">The method.</param>
        /// <param name="dataMappings">The data mappings.</param>
        /// <param name="encoding">The encoding.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, Dictionary<string, string> dataMappings, Encoding encoding = null)
        {
            if (httpWebRequest != null)
            {
                var stringBuilder = new StringBuilder();
                if (dataMappings != null)
                {
                    foreach (var key in dataMappings.Keys)
                    {
                        var value = dataMappings[key] ?? string.Empty;
                        stringBuilder.Append(key + "=" + value.Trim() + "&");
                    }

                }
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }

                var data = (encoding ?? Encoding.ASCII).GetBytes(stringBuilder.ToString());

                httpWebRequest.Method = method;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = data.Length;
                using (var stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        private static void InternalFillData(this HttpWebRequest httpWebRequest, string method, byte[] data, string contentType)
        {
            if (httpWebRequest != null && data != null)
            {
                if (!string.IsNullOrWhiteSpace(method))
                {
                    httpWebRequest.Method = method;
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    httpWebRequest.ContentType = contentType;
                }

                httpWebRequest.ContentLength = data.Length;
                var dataStream = httpWebRequest.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        private static void InternalFillData(this HttpWebRequest httpWebRequest, string method, Stream stream, string contentType)
        {
            if (httpWebRequest != null && stream != null)
            {
                if (!string.IsNullOrWhiteSpace(method))
                {
                    httpWebRequest.Method = method;
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    httpWebRequest.ContentType = contentType;
                }
                httpWebRequest.ContentLength = stream.Length;
                var dataStream = httpWebRequest.GetRequestStream();
                stream.CopyTo(dataStream);
                dataStream.Close();
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="encodingToByte">The encoding to byte.</param>
        /// <param name="contentType">Type of the content.</param>
        private static void InternalFillData(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = "application/json")
        {
            byte[] byteArray = null;

            // DO NOT use String.IsNullOrWhiteSpace or String.IsNullOrEmpty. string.Empty is allowed to filled here.
            if (data != null)
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            InternalFillData(httpWebRequest, method, byteArray, contentType);
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, byte[] data, string contentType = "application/json")
        {
            InternalFillData(httpWebRequest, method, data, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, byte[] data, string contentType = "application/json")
        {
            InternalFillData(httpWebRequest, null, data, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="method">The method.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, Stream stream, string contentType = "application/json", string method = null)
        {
            InternalFillData(httpWebRequest, method, stream, contentType);
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, string data, string contentType = "application/json")
        {
            InternalFillData(httpWebRequest, method, data, null, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string data, Encoding encoding = null, string contentType = null)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                InternalFillData(httpWebRequest, null, (encoding ?? Encoding.UTF8).GetBytes(data), contentType);
            }
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="encodingToByte">The encoding to byte.</param>
        /// <param name="contentType">Type of the content.</param>
        public static void FillData(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = "application/json")
        {
            byte[] byteArray = null;

            if (!string.IsNullOrWhiteSpace(data))
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            InternalFillData(httpWebRequest, method, byteArray, contentType);
        }

        #endregion

        #region Fill Data On HttpWebRequest async

        /// <summary>
        /// Fills the file data.
        /// <remarks>
        /// Reference: http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        /// </remarks>
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileCollection">The file collection.
        /// Key: file name. e.g.: sample.txt
        /// Value: file data in byte array.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public static async Task FillFileDataAsync(this HttpWebRequest httpWebRequest, NameValueCollection postData, Dictionary<string, byte[]> fileCollection, string paramName)
        {
            try
            {
                var boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

                httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

                using (var stream = new MemoryStream())
                {
                    var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                    var formDataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                    if (postData != null)
                    {
                        foreach (string key in postData.Keys)
                        {
                            var formItem = string.Format(formDataTemplate, key, postData[key]);
                            var formItemBytes = Encoding.UTF8.GetBytes(formItem);
                            stream.Write(formItemBytes, 0, formItemBytes.Length);
                        }
                    }

                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);

                    const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

                    if (fileCollection != null)
                    {
                        foreach (var key in fileCollection.Keys)
                        {
                            var header = string.Format(headerTemplate, paramName, key);
                            var headerBytes = Encoding.UTF8.GetBytes(header);
                            stream.Write(headerBytes, 0, headerBytes.Length);

                            stream.Write(fileCollection[key], 0, fileCollection[key].Length);

                            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        }
                    }

                    httpWebRequest.ContentLength = stream.Length;
                    stream.Position = 0;
                    var tempBuffer = new byte[stream.Length];
                    stream.Read(tempBuffer, 0, tempBuffer.Length);

                    using (var requestStream = await httpWebRequest.GetRequestStreamAsync())
                    {
                        var task = requestStream.WriteAsync(tempBuffer, 0, tempBuffer.Length);
                        await task;

                        if (task.Exception == null)
                        {
                            await requestStream.FlushAsync();
                        }
                    }

                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(fileCollection);
            }
        }

        /// <summary>
        /// Fills the file data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="fileFullName">Full name of the file.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="OperationFailureException">FillFileData</exception>
        public static async Task FillFileDataAsync(this HttpWebRequest httpWebRequest, NameValueCollection postData, string fileFullName, string paramName)
        {
            if (httpWebRequest != null && !string.IsNullOrWhiteSpace(fileFullName))
            {
                try
                {
                    var fileData = File.ReadAllBytes(fileFullName);
                    var fileName = Path.GetFileName(fileFullName);

                    var fileCollection = new Dictionary<string, byte[]> { { fileName, fileData } };

                    await FillFileDataAsync(httpWebRequest, postData, fileCollection, paramName);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { fileFullName, paramName });
                }
            }
        }

        /// <summary>
        /// Fills post data on HttpWebRequest.
        /// </summary>
        /// <param name="httpWebRequest">The HttpWebRequest instance.</param>
        /// <param name="method">The method.</param>
        /// <param name="dataMappings">The data mappings.</param>
        /// <param name="encoding">The encoding.</param>
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, string method, Dictionary<string, string> dataMappings, Encoding encoding = null)
        {
            if (httpWebRequest != null)
            {
                var stringBuilder = new StringBuilder();
                if (dataMappings != null)
                {
                    foreach (var key in dataMappings.Keys)
                    {
                        var value = dataMappings[key] ?? string.Empty;
                        stringBuilder.Append(key + "=" + value.Trim() + "&");
                    }

                }
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }

                var data = (encoding ?? Encoding.ASCII).GetBytes(stringBuilder.ToString());

                httpWebRequest.Method = method;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = data.Length;
                using (var stream = await httpWebRequest.GetRequestStreamAsync())
                {
                    var task = stream.WriteAsync(data, 0, data.Length);
                    await task;

                    if (task.Exception == null)
                    {
                        await stream.FlushAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        private static async Task InternalFillDataAsync(this HttpWebRequest httpWebRequest, string method, byte[] data, string contentType = "application/json")
        {
            if (httpWebRequest != null && data != null)
            {
                if (!string.IsNullOrWhiteSpace(method))
                {
                    httpWebRequest.Method = method;
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    httpWebRequest.ContentType = contentType;
                }

                httpWebRequest.ContentLength = data.Length;
                using (var dataStream = await httpWebRequest.GetRequestStreamAsync())
                {
                    var task = dataStream.WriteAsync(data, 0, data.Length);
                    await task;
                    if (task.Exception == null)
                    {
                        await dataStream.FlushAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Internals the fill data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="encodingToByte">The encoding to byte.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Task.</returns>
        private static async Task InternalFillDataAsync(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = "application/json")
        {
            byte[] byteArray = null;

            if (!string.IsNullOrWhiteSpace(data))
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            await InternalFillDataAsync(httpWebRequest, method, byteArray, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, byte[] data, string contentType = "application/json")
        {
            await InternalFillDataAsync(httpWebRequest, null, data, contentType);
        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">Type of the content.</param>
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, string data, Encoding encoding = null, string contentType = null)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                await InternalFillDataAsync(httpWebRequest, null, (encoding ?? Encoding.UTF8).GetBytes(data), contentType);
            }
        }

        /// <summary>
        /// Fills the data on HTTP web request.
        /// </summary>
        /// <param name="httpWebRequest">The HTTP web request.</param>
        /// <param name="method">The method.</param>
        /// <param name="data">The data.</param>
        /// <param name="encodingToByte">The encoding to byte.</param>
        /// <param name="contentType">Type of the content.</param>
        public static async Task FillDataAsync(this HttpWebRequest httpWebRequest, string method, string data, Encoding encodingToByte, string contentType = "application/json")
        {
            byte[] byteArray = null;

            if (!string.IsNullOrWhiteSpace(data))
            {
                byteArray = (encodingToByte ?? Encoding.UTF8).GetBytes(data);
            }

            await InternalFillDataAsync(httpWebRequest, method, byteArray, contentType);
        }

        #endregion

        #region Get Post Data/Json

        /// <summary>
        /// Gets the post data from HTTP web request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GetPostData(this HttpRequest httpRequest)
        {
            byte[] data = null;

            if (httpRequest != null)
            {
                using (var ms = new MemoryStream())
                {
                    httpRequest.InputStream.CopyTo(ms);
                    data = ms.ToArray();
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the post data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GetPostData(this HttpRequestBase httpRequest)
        {
            byte[] data = null;

            if (httpRequest != null)
            {
                using (var ms = new MemoryStream())
                {
                    httpRequest.InputStream.CopyTo(ms);
                    data = ms.ToArray();
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the post data.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GetPostData(this HttpListenerRequest httpRequest)
        {
            byte[] data = null;

            if (httpRequest != null)
            {
                using (var ms = new MemoryStream())
                {
                    httpRequest.InputStream.CopyTo(ms);
                    data = ms.ToArray();
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the post json.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">GetPostJson</exception>
        public static string GetPostJson(this HttpRequest httpRequest, Encoding encoding = null)
        {
            string result = string.Empty;

            try
            {
                var bytes = httpRequest.GetPostData();

                if (bytes != null)
                {
                    result = (encoding ?? Encoding.UTF8).GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }

            return result;
        }

        /// <summary>
        /// Gets the post json.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string GetPostJson(this HttpRequestBase httpRequest, Encoding encoding = null)
        {
            string result = string.Empty;

            try
            {
                var bytes = httpRequest.GetPostData();

                if (bytes != null)
                {
                    result = (encoding ?? Encoding.UTF8).GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }

            return result;
        }

        /// <summary>
        /// Gets the post json.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string GetPostJson(this HttpListenerRequest httpRequest, Encoding encoding = null)
        {
            string result = string.Empty;

            try
            {
                var bytes = httpRequest.GetPostData();

                if (bytes != null)
                {
                    result = (encoding ?? Encoding.UTF8).GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }

            return result;
        }

        #endregion

        #region Response Write

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <exception cref="InvalidObjectException">WriteContent</exception>
        /// <exception cref="InvalidObjectException">WriteContent</exception>
        public static void WriteAllContent(this HttpListenerResponse httpResponse, string content, Encoding encoding = null, string contentType = "text/html", bool closeStream = true)
        {
            try
            {
                httpResponse.ContentType = contentType;

                var buffer = (encoding ?? Encoding.UTF8).GetBytes(content.SafeToString());
                httpResponse.ContentLength64 = buffer.Length;
                var output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                if (closeStream)
                {
                    output.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <exception cref="InvalidObjectException">WriteContent</exception>
        public static void WriteContent(this HttpResponseBase httpResponse, string content, Encoding encoding = null, bool closeStream = true)
        {
            try
            {
                var buffer = (encoding ?? Encoding.UTF8).GetBytes(content.SafeToString());
                var output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                if (closeStream)
                {
                    output.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Writes the content.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <exception cref="InvalidObjectException">WriteContent</exception>
        public static void WriteContent(this HttpResponse httpResponse, string content, Encoding encoding = null, bool closeStream = true)
        {
            try
            {
                byte[] buffer = (encoding ?? Encoding.UTF8).GetBytes(content.SafeToString());
                Stream output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                if (closeStream)
                {
                    output.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion

        #region CreateHttpWebRequestByRaw

        /// <summary>
        /// The raw request seperator regex
        /// </summary>
        private static Regex rawRequestSeperatorRegex = new Regex(@"((\s|\t)*((\r|\n)+)(\s|\t)*)+", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        /// <summary>
        /// The header regex
        /// </summary>
        private static Regex headerRegex = new Regex(@"^(?<Key>([^\s\t\r\n]+)):(\s)*(?<Value>([^\t\r\n])+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The destination URL regex
        /// </summary>
        private static Regex destinationUrlRegex = new Regex(@"(?<HttpMethod>([^\s\t\r\n]+))(\s)+(?<Url>([^\s\t\r\n]+))(\s)+(?<Protocal>(([^\s\t\r\n])+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Creates the HTTP web request by raw. <c>Raw</c> can be from Fiddler.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>HttpWebRequest.</returns>
        /// <exception cref="InvalidObjectException">
        /// raw
        /// or
        /// raw.destination
        /// </exception>
        public static HttpWebRequest CreateHttpWebRequestByRaw(this string raw, Encoding encoding = null)
        {
            //SAMPLE:
            //GET https://www.telerik.com/UpdateCheck.aspx?isBeta=False HTTP/1.1
            //User-Agent: Fiddler/4.6.0.2 (.NET 4.0.30319.42000; WinNT 10.0.10240.0; zh-CN; 4xAMD64)
            //Pragma: no-cache
            //Host: www.telerik.com
            //Accept-Language: zh-CN
            //Referer: http://fiddler2.com/client/4.6.0.2
            //Accept-Encoding: gzip, deflate
            //Connection: close

            try
            {
                raw.CheckEmptyString("raw");

                string destinationString, httpMethod, destinationUrl = null;
                StringBuilder bodyBuilder = new StringBuilder();
                var headerDictionary = new Dictionary<string, string>();

                string[] rawPieces = rawRequestSeperatorRegex.Split(raw.Trim());

                if (raw.Length < 1)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("raw");
                }

                destinationString = rawPieces[0];

                // Process destination string.
                var destimatimMatch = destinationUrlRegex.Match(destinationString);
                if (destimatimMatch != null && destimatimMatch.Success)
                {
                    httpMethod = destimatimMatch.Result("${HttpMethod}");
                    destinationUrl = destimatimMatch.Result("${Url}");
                }
                else
                {
                    throw ExceptionFactory.CreateInvalidObjectException("raw.destination");
                }

                // Process headers and body
                for (var i = 1; i < rawPieces.Length; i++)
                {
                    var match = headerRegex.Match(rawPieces[i]);

                    if (match.Success)
                    {
                        headerDictionary.Merge(match.Result("${Key}"), match.Result("${Value}"));
                    }
                    else
                    {
                        bodyBuilder.AppendLine(rawPieces[i].Trim());
                    }
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(destinationUrl);

                foreach (var one in headerDictionary)
                {
                    httpWebRequest.SafeSetHttpHeader(one.Key, one.Value);
                }

                httpWebRequest.Method = httpMethod;

                // Process Body
                if (bodyBuilder.Length > 0)
                {
                    httpWebRequest.InternalFillData(null, bodyBuilder.ToString(), encoding, null);
                }

                return httpWebRequest;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { raw });
            }
        }

        #endregion

        #region CreateHttpWebRequest

        /// <summary>
        /// To the raw.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.String.</returns>
        public static string ToRaw(this HttpWebRequest httpRequest)
        {
            StringBuilder builder = new StringBuilder();

            if (httpRequest != null)
            {
                //Write destination
                builder.Append(httpRequest.Method);
                builder.Append(" ");
                builder.Append(httpRequest.RequestUri.PathAndQuery);
                builder.Append(" ");
                builder.Append(httpRequest.ProtocolVersion);
                builder.AppendLine();
                builder.AppendLine();

                //Write headers
                foreach (string key in httpRequest.Headers.Keys)
                {
                    builder.AppendLineWithFormat("{0}: {1}", key, httpRequest.Headers.Get(key));
                }

                builder.AppendLine();

                if (httpRequest.Method.IsInString(HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put))
                {
                    var bytes = httpRequest.GetRequestStream().ReadStreamToBytes();
                    builder.AppendLine(Encoding.UTF8.GetString(bytes));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Omits the remote certificate validation callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="sslPolicyErrors">The SSL policy errors.</param>
        /// <returns>System.Boolean.</returns>
        private static bool OmitRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Creates the HTTP web request.
        /// </summary>
        /// <param name="destinationUrl">The destination URL.</param>
        /// <param name="httpMethod">Type of the method.</param>
        /// <param name="referrer">The referrer.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="cookieString">The cookie string.</param>
        /// <param name="accept">The accept.</param>
        /// <param name="acceptGZip">The accept g zip.</param>
        /// <param name="omitServerCertificateValidation">The omit server certificate validation.</param>
        /// <returns>HttpWebRequest.</returns>
        public static HttpWebRequest CreateHttpWebRequest(this string destinationUrl, string httpMethod = HttpConstants.HttpMethod.Get, string referrer = null, string userAgent = null, CookieContainer cookieContainer = null, string cookieString = null, string accept = null, bool acceptGZip = true, bool omitServerCertificateValidation = false)
        {
            try
            {
                destinationUrl.CheckEmptyString("destinationUrl");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(destinationUrl);
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Accept = accept.SafeToString("*/*");

                if (omitServerCertificateValidation)
                {
                    httpWebRequest.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(OmitRemoteCertificateValidationCallback);
                }

                if (!string.IsNullOrWhiteSpace(referrer))
                {
                    httpWebRequest.Referer = referrer;
                }

                if (acceptGZip)
                {
                    httpWebRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.AcceptEncoding, "gzip, deflate");
                }

                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    httpWebRequest.UserAgent = userAgent;
                }

                httpWebRequest.CookieContainer = cookieContainer ?? new CookieContainer();
                if (!string.IsNullOrWhiteSpace(cookieString))
                {
                    var collection = new CookieCollection();
                    collection.SetCookieByString(cookieString, httpWebRequest.RequestUri.Host);
                    httpWebRequest.CookieContainer.Add(httpWebRequest.RequestUri, collection);
                }

                httpWebRequest.Method = httpMethod.SafeToString(HttpConstants.HttpMethod.Get);

                return httpWebRequest;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { destinationUrl, httpMethod, referrer, userAgent });
            }
        }

        /// <summary>
        /// Creates the HTTP web request.
        /// </summary>
        /// <param name="uriObject">The URI object.</param>
        /// <param name="method">The method.</param>
        /// <returns>HttpWebRequest.</returns>
        public static HttpWebRequest CreateHttpWebRequest(this Uri uriObject, string method = HttpConstants.HttpMethod.Get)
        {
            if (uriObject != null)
            {
                return CreateHttpWebRequest(uriObject.ToString(), method);
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Sets the basic authentication.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public static void SetBasicAuthentication(this HttpWebRequest request, string userName, string password)
        {
            if (request != null)
            {
                request.Headers[HttpRequestHeader.Authorization] = "Basic " + string.Format("{0}:{1}", userName, password).ToBase64();
            }
        }

        /// <summary>
        /// Sets the cookie by string.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <param name="cookieString">The cookie string.</param>
        /// <param name="hostDomain">The host domain.</param>
        public static void SetCookieByString(this CookieCollection cookieCollection, string cookieString, string hostDomain)
        {
            if (cookieCollection != null && !string.IsNullOrWhiteSpace(cookieString))
            {
                string[] cookieProperties = cookieString.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in cookieProperties)
                {
                    string[] keyValue = property.Split(new char[] { '=' }, 2);
                    if (keyValue.Length >= 2)
                    {
                        cookieCollection.Add(new Cookie(keyValue[0], keyValue[1], null, hostDomain));
                    }
                }
            }
        }

        /// <summary>
        /// Sets the cookie by string.
        /// </summary>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="cookieString">The cookie string.</param>
        /// <param name="hostDomain">The host domain.</param>
        public static void SetCookieByString(this CookieContainer cookieContainer, string cookieString, Uri hostDomain)
        {
            if (hostDomain != null && cookieContainer != null && !string.IsNullOrWhiteSpace(cookieString))
            {
                var cookieCollection = cookieContainer.GetCookies(hostDomain);
                cookieCollection.SetCookieByString(cookieString, hostDomain.Host);
            }
        }

        /// <summary>
        /// Expireses all.
        /// </summary>
        /// <param name="cookies">The cookies.</param>
        public static void ExpiresAll(this CookieCollection cookies)
        {
            if (cookies != null)
            {
                foreach (Cookie one in cookies)
                {
                    one.Expired = true;
                    one.Expires = DateTime.UtcNow.AddDays(-1);
                }
            }
        }

        /// <summary>
        /// Automatics the cookie raw string.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.String.</returns>
        public static string ToCookieRawString(this CookieCollection cookieCollection)
        {
            var builder = new StringBuilder();

            if (cookieCollection != null)
            {
                foreach (Cookie cookie in cookieCollection)
                {
                    builder.AppendFormat("{0}={1}; ", cookie.Name, cookie.Value);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// To the key value string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="needUrlEncode">if set to <c>true</c> [need URL encode].</param>
        /// <returns>System.String.</returns>
        public static string ToKeyValueStringWithUrlEncode(this IDictionary<string, string> parameters, bool needUrlEncode = true)
        {
            var builder = new StringBuilder();

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var one in parameters)
                {
                    builder.AppendFormat("{0}={1}&", one.Key, needUrlEncode ? one.Value.ToUrlEncodedText() : one.Value);
                }
            }

            return builder.ToString().TrimEnd('&');
        }

        /// <summary>
        /// Supply binary download via HttpResponse
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="physicalPath">The physical path.</param>
        /// <param name="originalFullFileName">Name of the original full file.</param>
        /// <exception cref="OperationFailureException">SupplyBinaryDownload</exception>
        public static void SupplyBinaryDownload(this HttpResponse response, string physicalPath, string originalFullFileName)
        {
            if (response != null && !string.IsNullOrWhiteSpace(physicalPath) && File.Exists(physicalPath))
            {
                var fs = new FileStream(physicalPath, FileMode.Open);

                try
                {
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(originalFullFileName.SafeToString("download"), Encoding.UTF8));
                    response.BinaryWrite(bytes);
                    response.Flush();
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { physicalPath, originalFullFileName });
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();
                    response.End();
                }
            }
        }

        /// <summary>
        /// Supplies the binary download.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="physicalPath">The physical path.</param>
        /// <param name="originalFullFileName">Name of the original full file.</param>
        public static void SupplyBinaryDownload(this HttpContext context, string physicalPath, string originalFullFileName)
        {
            if (context != null)
            {
                SupplyBinaryDownload(context.Response, physicalPath, originalFullFileName);
            }
        }

        /// <summary>
        /// Gets the posted file.
        /// </summary>
        /// <param name="httpFileBase">The HTTP file base.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GetPostedFile(this HttpPostedFileBase httpFileBase)
        {
            if (httpFileBase != null && httpFileBase.ContentLength > 0)
            {
                return httpFileBase.InputStream.ToBytes();
            }

            return null;
        }

        /// <summary>
        /// Parses to key value pair collection.
        /// <remarks>
        /// Define separator = '&amp;',
        /// Parse string like a=1&amp;b=2&amp;c=3 into name value collection.
        /// Define separator = ';',
        /// Parse string like a=1;b=2;c=3 into name value collection.
        /// </remarks></summary>
        /// <param name="keyValuePairString">The key value pair string.</param>
        /// <param name="separator">The separator. Default is '&amp;'.</param>
        /// <returns>System.Collections.Specialized.NameValueCollection.</returns>
        public static NameValueCollection ParseToKeyValuePairCollection(this string keyValuePairString, char separator = '&')
        {
            var result = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(keyValuePairString))
            {
                try
                {
                    var pairs = keyValuePairString.Split(separator);
                    foreach (var one in pairs)
                    {
                        if (!string.IsNullOrWhiteSpace(one))
                        {
                            var keyValuePair = one.Split(new char[] { '=' }, 2);

                            if (keyValuePair.Length == 2)
                            {
                                var key = keyValuePair[0];
                                var value = keyValuePair[1];

                                if (!string.IsNullOrWhiteSpace(key))
                                {
                                    result.Set(key, value.SafeToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { keyValuePairString, separator = separator.ToString() });
                }
            }

            return result;
        }

        #region Http Async

        /// <summary>
        /// Class AsyncHttpState.
        /// </summary>
        private class AsyncHttpState
        {
            /// <summary>
            /// All done
            /// </summary>
            protected ManualResetEvent allDone = new ManualResetEvent(false);

            /// <summary>
            /// The response callback
            /// </summary>
            protected Action<WebResponse> responseCallback = null;

            /// <summary>
            /// Gets or sets the timeout.
            /// </summary>
            /// <value>The timeout.</value>
            public int Timeout
            {
                get;
                protected set;
            }

            /// <summary>
            /// Gets the request URI.
            /// </summary>
            /// <value>The request URI.</value>
            public Uri RequestUri
            {
                get
                {
                    return this.Request != null ? this.Request.RequestUri : null;
                }
            }

            /// <summary>
            /// Gets the request URL.
            /// </summary>
            /// <value>The request URL.</value>
            public string RequestUrl
            {
                get
                {
                    return this.RequestUri != null ? this.RequestUri.ToString() : string.Empty;
                }
            }

            /// <summary>
            /// Gets or sets the request.
            /// </summary>
            /// <value>The request.</value>
            public HttpWebRequest Request
            {
                get;
                protected set;
            }

            /// <summary>
            /// Gets or sets the response.
            /// </summary>
            /// <value>The response.</value>
            public WebResponse Response
            {
                get;
                protected set;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AsyncHttpState" /> class.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="timeout">The timeout.</param>
            public AsyncHttpState(HttpWebRequest request, int timeout = 120000)
            {
                this.Request = request;

                this.Timeout = timeout > 0 ? timeout : 120000;
            }

            /// <summary>
            /// Gets the response asynchronous.
            /// </summary>
            /// <param name="callback">The callback.</param>
            /// <exception cref="Beyova.ExceptionSystem.DataConflictException">callback;AsyncHttpState;null</exception>
            public void GetResponseAsync(Action<WebResponse> callback)
            {
                try
                {
                    if (responseCallback != null)
                    {
                        throw new DataConflictException("callback", "AsyncHttpState", null, this.RequestUrl);
                    }

                    this.responseCallback = callback;

                    IAsyncResult result = (IAsyncResult)this.Request.BeginGetResponse(new AsyncCallback(ResponseCallback), this);

                    ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), this.Request, this.Timeout, true);

                    // The response came in the allowed time. The work processing will happen in the 
                    // callback function.
                    allDone.WaitOne();

                    // Release the HttpWebResponse resource.
                    this.Response.Close();
                }
                catch (Exception ex)
                {
                    throw ex.Handle(this.RequestUrl);
                }
            }

            /// <summary>
            /// Timeouts the callback.
            /// </summary>
            /// <param name="state">The state.</param>
            /// <param name="timedOut">if set to <c>true</c> [timed out].</param>
            private static void TimeoutCallback(object state, bool timedOut)
            {
                if (timedOut)
                {
                    HttpWebRequest request = state as HttpWebRequest;
                    if (request != null)
                    {
                        request.Abort();
                    }
                }
            }

            /// <summary>
            /// Responses the callback.
            /// </summary>
            /// <param name="asynchronousResult">The asynchronous result.</param>
            /// <exception cref="OperationFailureException">ResponseCallback</exception>
            private static void ResponseCallback(IAsyncResult asynchronousResult)
            {
                AsyncHttpState asyncHttpState = (AsyncHttpState)asynchronousResult.AsyncState;

                try
                {
                    var myHttpWebRequest = asyncHttpState.Request;

                    asyncHttpState.Response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                    if (asyncHttpState.responseCallback != null)
                    {
                        asyncHttpState.responseCallback(asyncHttpState.Response);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(asyncHttpState.RequestUrl);
                }
                finally
                {
                    asyncHttpState.allDone.Set();
                }
            }
        }

        /// <summary>
        /// Proceeds the response asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="callback">The callback.</param>
        public static void ProceedResponseAsync(this HttpWebRequest request, Action<WebResponse> callback = null)
        {
            if (request != null)
            {
                AsyncHttpState state = new AsyncHttpState(request);
                state.GetResponseAsync(callback);
            }
        }


        #endregion

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetHeader(this HttpRequest httpRequest, string headerKey)
        {
            return httpRequest?.Headers?.Get(headerKey).SafeToString();
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetHeader(this HttpRequestBase httpRequest, string headerKey)
        {
            return httpRequest?.Headers?.Get(headerKey).SafeToString();
        }

        /// <summary>
        /// Tries the get header.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetHeader(this HttpWebRequest httpRequest, string headerKey)
        {
            return httpRequest?.Headers?.Get(headerKey).SafeToString();
        }

        #region Http proxy

        /// <summary>
        /// Copies the HTTP request to HTTP web request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="destinationHost">The destination host.</param>
        /// <param name="rewriteDelegate">The rewrite delegate.</param>
        /// <returns>
        /// HttpWebRequest.
        /// </returns>
        public static HttpWebRequest CopyHttpRequestToHttpWebRequest(this HttpRequest httpRequest, string destinationHost, Func<NameValueCollection, NameValueCollection, Exception> rewriteDelegate = null)
        {
            if (httpRequest != null && !string.IsNullOrWhiteSpace(destinationHost))
            {
                var newUrl = string.Format("{0}{1}", destinationHost.TrimEnd('/'), httpRequest.Url.PathAndQuery);

                var destinationRequest = newUrl.CreateHttpWebRequest(httpRequest.HttpMethod);
                destinationRequest.Headers.Set(HttpConstants.HttpHeader.ORIGINAL, httpRequest.UserHostAddress);

                FillHttpRequestToHttpWebRequest(httpRequest, destinationRequest, rewriteDelegate);

                return destinationRequest;
            }

            return null;
        }

        /// <summary>
        /// The ignored headers
        /// </summary>
        public readonly static string[] ignoredHeaders = new string[] { HttpConstants.HttpHeader.TransferEncoding, HttpConstants.HttpHeader.AccessControlAllowHeaders, HttpConstants.HttpHeader.AccessControlAllowMethods, HttpConstants.HttpHeader.AccessControlAllowOrigin };

        /// <summary>
        /// Transports the HTTP response.
        /// </summary>
        /// <param name="sourceResponse">The source response.</param>
        /// <param name="destinationResponse">The destination response.</param>
        public static void TransportHttpResponse(this HttpWebResponse sourceResponse, HttpResponse destinationResponse)
        {
            if (sourceResponse != null && destinationResponse != null)
            {
                foreach (var key in sourceResponse.Headers.AllKeys)
                {
                    if (!key.IsInString(ignoredHeaders))
                    {
                        destinationResponse.SafeSetHttpHeader(key, sourceResponse.Headers.Get(key));
                    }
                }
                destinationResponse.StatusCode = (int)(sourceResponse.StatusCode);
                destinationResponse.StatusDescription = sourceResponse.StatusDescription;

                using (var sourceStream = sourceResponse.GetResponseStream())
                {
                    sourceStream.CopyTo(destinationResponse.OutputStream);
                    destinationResponse.Flush();
                }
            }
        }

        /// <summary>
        /// Transports the HTTP response.
        /// </summary>
        /// <param name="sourceResponse">The source response.</param>
        /// <param name="destinationResponse">The destination response.</param>
        public static void TransportHttpResponse(this WebResponse sourceResponse, HttpResponse destinationResponse)
        {
            if (sourceResponse != null && destinationResponse != null)
            {
                TransportHttpResponse((HttpWebResponse)sourceResponse, destinationResponse);
            }
        }

        /// <summary>
        /// Fills the HTTP request to HTTP web request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="request">The request.</param>
        /// <param name="rewriteDelegate">The rewrite delegate.</param>
        public static void FillHttpRequestToHttpWebRequest(this HttpRequest httpRequest, HttpWebRequest request, Func<NameValueCollection, NameValueCollection, Exception> rewriteDelegate = null)
        {
            if (httpRequest != null && request != null)
            {
                //Copy header
                request.Headers.Clear();
                foreach (var key in httpRequest.Headers.AllKeys)
                {
                    request.SafeSetHttpHeader(key, httpRequest.Headers.Get(key));
                }

                if (rewriteDelegate != null)
                {
                    var exception = rewriteDelegate(request.Headers, httpRequest.Headers);
                    if (exception != null)
                    {
                        throw exception.Handle();
                    }
                }

                //Copy body, for PUT and POST only.
                if (httpRequest.HttpMethod == HttpConstants.HttpMethod.Put ||
                    httpRequest.HttpMethod == HttpConstants.HttpMethod.Post)
                {
                    var bytes = httpRequest.GetPostData();
                    request.FillData(httpRequest.HttpMethod, bytes, httpRequest.ContentType);
                }
            }
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// <remarks>This method would help you to set values for header, especially for those need to be set by property. But following items would be ignored.
        /// <list type="bullet"><item>host</item><item>connection</item><item>close</item><item>content-length</item><item>proxy-connection</item><item>range</item></list></remarks>
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreIfNullOrEmpty">if set to <c>true</c> [ignore if null or empty].</param>
        public static void SafeSetHttpHeader(this HttpWebRequest httpRequest, string headerKey, object value, bool ignoreIfNullOrEmpty = false)
        {
            if (httpRequest != null && !string.IsNullOrWhiteSpace(headerKey))
            {
                if (ignoreIfNullOrEmpty && (value == null || string.IsNullOrWhiteSpace(value.SafeToString())))
                {
                    return;
                }

                switch (headerKey.ToLowerInvariant())
                {
                    case "accept":
                        httpRequest.Accept = value.SafeToString();
                        break;
                    case "content-type":
                        httpRequest.ContentType = value.SafeToString();
                        break;
                    case "date":
                        httpRequest.Date = (DateTime)value;
                        break;
                    case "expect":
                        httpRequest.Expect = value.SafeToString();
                        break;
                    case "if-modified-since":
                        httpRequest.IfModifiedSince = (DateTime)value;
                        break;
                    case "referer":
                        httpRequest.Referer = value.SafeToString();
                        break;
                    case "transfer-encoding":
                        httpRequest.TransferEncoding = value.SafeToString();
                        break;
                    case "user-agent":
                        httpRequest.UserAgent = value.SafeToString();
                        break;
                    case "host":
                    case "connection":
                    case "close":
                    case "content-length":
                    case "proxy-connection":
                    case "range":
                        //do nothing
                        break;
                    default:
                        httpRequest.Headers[headerKey] = value.SafeToString();
                        break;
                }
            }
        }

        /// <summary>
        /// Safes the set HTTP header.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="headerKey">The header key.</param>
        /// <param name="value">The value.</param>
        public static void SafeSetHttpHeader(this HttpResponse httpResponse, string headerKey, object value)
        {
            if (httpResponse != null && !string.IsNullOrWhiteSpace(headerKey))
            {
                switch (headerKey.ToLowerInvariant())
                {
                    case "content-type":
                        httpResponse.ContentType = value.SafeToString();
                        break;
                    case "host":
                    //case "connection":
                    case "close":
                    case "content-length":
                    case "proxy-connection":
                    case "range":
                        //do nothing
                        break;
                    default:
                        httpResponse.Headers[headerKey] = value.SafeToString();
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// Converts the HTTP status code to exception code.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="webExceptionStatus">The web exception status.</param>
        /// <returns>ExceptionCode.</returns>
        public static ExceptionCode ConvertHttpStatusCodeToExceptionCode(this HttpStatusCode httpStatusCode, WebExceptionStatus webExceptionStatus)
        {
            ExceptionCode result = new ExceptionCode { Minor = webExceptionStatus == WebExceptionStatus.Success ? string.Empty : webExceptionStatus.ToString() };

            var statudCodeString = ((int)httpStatusCode).ToString();
            if (!(statudCodeString.StartsWith("4") || statudCodeString.StartsWith("5")))
            {
                return null;
            }

            switch (httpStatusCode)
            {
                case HttpStatusCode.BadRequest://400
                    result.Major = ExceptionCode.MajorCode.NullOrInvalidValue;
                    break;
                case HttpStatusCode.Unauthorized://401
                    result.Major = ExceptionCode.MajorCode.UnauthorizedOperation;
                    break;
                case HttpStatusCode.PaymentRequired://402
                    result.Major = ExceptionCode.MajorCode.CreditNotAfford;
                    break;
                case HttpStatusCode.Forbidden://403
                    result.Major = ExceptionCode.MajorCode.OperationForbidden;
                    break;
                case HttpStatusCode.NotFound: //404
                    result.Major = ExceptionCode.MajorCode.ResourceNotFound;
                    break;
                case HttpStatusCode.Conflict: //409
                    result.Major = ExceptionCode.MajorCode.DataConflict;
                    break;
                case HttpStatusCode.InternalServerError: //500
                    result.Major = ExceptionCode.MajorCode.OperationFailure;
                    break;
                case HttpStatusCode.NotImplemented: //501
                    result.Major = ExceptionCode.MajorCode.NotImplemented;
                    break;
                case HttpStatusCode.ServiceUnavailable: //503
                    result.Major = ExceptionCode.MajorCode.ServiceUnavailable;
                    break;
                case HttpStatusCode.HttpVersionNotSupported: //505
                    result.Major = ExceptionCode.MajorCode.Unsupported;
                    break;
                default:
                    result.Major = ExceptionCode.MajorCode.HttpBlockError;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is g zip] [the specified web response].
        /// </summary>
        /// <param name="webResponse">The web response.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsGZip(this WebResponse webResponse)
        {
            var contentEncoding = webResponse?.Headers?.Get(HttpConstants.HttpHeader.ContentEncoding);
            return contentEncoding != null && contentEncoding.Equals(HttpConstants.HttpValues.GZip, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether [is mobile user agent].
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        /// <returns>
        ///   <c>true</c> if [is mobile user agent] [the specified user agent]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMobileUserAgent(this string userAgent)
        {
            return !string.IsNullOrWhiteSpace(userAgent) && (
                  userAgent.IndexOf("pad", StringComparison.InvariantCultureIgnoreCase) > -1
                      || userAgent.IndexOf("android", StringComparison.InvariantCultureIgnoreCase) > -1
                      || userAgent.IndexOf("phone", StringComparison.InvariantCultureIgnoreCase) > -1
                  );
        }
    }
}
