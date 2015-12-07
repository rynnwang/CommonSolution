using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using ifunction;
using ifunction.ExceptionSystem;
using ifunction.RestApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class RestApiClient.
    /// </summary>
    public class RestApiClient
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; protected set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable exception restore].
        /// </summary>
        /// <value><c>true</c> if [enable exception restore]; otherwise, <c>false</c>.</value>
        public bool EnableExceptionRestore { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="version">The version.</param>
        /// <param name="isHttps">if set to <c>true</c> [is HTTPS].</param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [enable exception restore].</param>
        public RestApiClient(string host, string version, bool isHttps = false, string token = null, bool enableExceptionRestore = false)
            : this(new ApiEndpoint { Host = host, Version = version, Protocol = isHttps ? "https" : "http" }, token, enableExceptionRestore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [enable exception restore].</param>
        public RestApiClient(ApiEndpoint endpoint, string token = null, bool enableExceptionRestore = false)
            : this(endpoint.ToString(), token, enableExceptionRestore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.
        /// <example>http://xxx.xxx.com/api/</example></param>
        /// <param name="version">The version.</param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [enable exception restore].</param>
        public RestApiClient(string baseUrl, string version, string token, bool enableExceptionRestore = false)
            : this(string.Format("{0}/{1}", baseUrl.TrimEnd('/'), version), token, enableExceptionRestore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.
        /// <example>http://xxx.xxx.com/api/v1/</example></param>
        /// <param name="token">The token.</param>
        /// <param name="enableExceptionRestore">if set to <c>true</c> [include exception detail].</param>
        public RestApiClient(string baseUrl, string token, bool enableExceptionRestore = false)
        {
            this.BaseUrl = baseUrl.SafeToString().TrimEnd('/') + "/";
            this.Token = token;
        }

        /// <summary>
        /// Invokes the specified HTTP method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public object Invoke(string httpMethod, string resourceName, string resourceAction, params object[] parameters)
        {
            HttpStatusCode httpStatusCode;
            return Invoke(httpMethod, resourceName, resourceAction, out httpStatusCode, parameters);
        }

        /// <summary>
        /// Invokes the specified HTTP method.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAction">The resource action.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>System.Object.</returns>
        public object Invoke(string httpMethod, string resourceName, string resourceAction, out HttpStatusCode httpStatusCode, object parameter)
        {
            try
            {
                var url = string.Format("{0}{1}/{2}", this.BaseUrl, resourceName, resourceAction).TrimEnd('/') + "/";
                object requestBody = null;

                if (parameter != null)
                {
                    if (parameter.GetType().IsStringOrValueType())
                    {
                        url += parameter.ToString().ToUrlPathEncodedText();
                    }
                    else
                    {
                        requestBody = parameter;
                    }
                }

                var httpRequest = url.CreateHttpWebRequest(httpMethod.SafeToString("POST"));
                if (!string.IsNullOrWhiteSpace(Token))
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TOKEN, Token);
                }

                var currentApiContext = ContextHelper.ApiContext;

                var originalIpAddress = currentApiContext.IpAddress.SafeToString();
                if (!string.IsNullOrWhiteSpace(originalIpAddress))
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.ORIGINAL, originalIpAddress);
                }

                var traceId = currentApiContext.TraceId.SafeToString();
                if (!string.IsNullOrWhiteSpace(traceId))
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TRACEID, traceId);
                }

                var userAgent = currentApiContext.UserAgent.SafeToString();
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    httpRequest.UserAgent = userAgent;
                }

                if (requestBody != null)
                {
                    httpRequest.FillData(httpMethod.SafeToString("POST"), requestBody.ToJson());
                }

                var response = httpRequest.ReadResponseAsText(Encoding.UTF8, out httpStatusCode);
                return JsonConvert.DeserializeObject(response);
            }
            catch (Exception ex)
            {
                throw ex.Handle("Invoke", new { httpMethod, resourceName, resourceAction });
            }
        }

        /// <summary>
        /// Invokes as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>T.</returns>
        public T InvokeAs<T>(MethodInfo methodInfo, out HttpStatusCode httpStatusCode, params object[] parameters)
        {
            try
            {
                methodInfo.CheckNullObject("methodInfo");
                var result = Invoke(methodInfo, out httpStatusCode, parameters);
                return result == null ? default(T) : result.ToObject<T>();
            }
            catch (Exception ex)
            {
                throw ex.Handle("InvokeAs", new { Method = methodInfo?.GetFullName() });
            }
        }

        /// <summary>
        /// Invokes the with void.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="parameters">The parameters.</param>
        public void InvokeWithVoid(MethodInfo methodInfo, out HttpStatusCode httpStatusCode, params object[] parameters)
        {
            try
            {
                methodInfo.CheckNullObject("methodInfo");
                Invoke(methodInfo, out httpStatusCode, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("InvokeWithVoid", new { Method = methodInfo?.GetFullName() });
            }
        }

        /// <summary>
        /// Invokes the specified method information.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException"></exception>
        public JToken Invoke(MethodInfo methodInfo, out HttpStatusCode httpStatusCode, params object[] parameters)
        {
            try
            {
                methodInfo.CheckNullObject("methodInfo");

                var apiUrlAttribute = methodInfo.GetCustomAttribute<ApiOperationAttribute>();
                if (apiUrlAttribute == null)
                {
                    throw new ResourceNotFoundException(methodInfo.DeclaringType.FullName, methodInfo.Name);
                }

                var resourceName = apiUrlAttribute.ResourceName;
                var resourceAction = apiUrlAttribute.Action;
                var httpMethod = apiUrlAttribute.HttpMethod.SafeToString("POST");

                var url = string.Format("{0}{1}/{2}", this.BaseUrl, resourceName, resourceAction).TrimEnd('/') + "/";
                object requestBody = null;
                StringBuilder urlParameterBuilder = null;

                if (parameters != null && parameters.Length > 0)
                {
                    if (parameters.Length > 1)
                    {
                        urlParameterBuilder = new StringBuilder(url.IndexOf('?') > -1 ? "" : "?");
                    }
                    var firstParameter = parameters.First();
                    if (firstParameter.GetType().IsStringOrValueType())
                    {
                        if (urlParameterBuilder != null)
                        {
                            urlParameterBuilder.AppendFormat("{0}={1}&", methodInfo.GetParameters()[0].Name,
                                firstParameter);
                        }
                        else
                        {
                            url += firstParameter.ToString().ToUrlPathEncodedText();
                        }
                    }
                    else
                    {
                        requestBody = firstParameter;
                    }
                }

                if (urlParameterBuilder != null)
                {
                    var inputParameters = methodInfo.GetParameters();
                    var minLength = Math.Min(inputParameters.Length, parameters.Length);
                    for (var i = 1; i < minLength; i++)
                    {
                        urlParameterBuilder.AppendFormat("{0}={1}&", inputParameters[i].Name,
                          parameters[i].ToString().ToUrlPathEncodedText());
                    }
                    url += urlParameterBuilder.ToString().TrimEnd('&');
                }

                var httpRequest = url.CreateHttpWebRequest(httpMethod);

                if (!string.IsNullOrWhiteSpace(Token))
                {
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TOKEN, Token);
                }

                if (requestBody != null)
                {
                    httpRequest.FillData(httpMethod, requestBody.ToJson());
                }

                var response = httpRequest.ReadResponseAsText(Encoding.UTF8, out httpStatusCode);

                if (httpStatusCode == HttpStatusCode.NoContent)
                {
                    return null;
                }
                else if (!((int)httpStatusCode).ToString().StartsWith("2"))
                {
                    var exceptionInfo = response.TryDeserializeAsObject<ExceptionInfo>();
                    if (this.EnableExceptionRestore)
                    {
                        throw exceptionInfo.ToException().Handle("Invoke", new { method = methodInfo.SafeToString(), parameters });
                    }
                    else
                    {
                        throw new RemoteServiceOperationFailureException(string.Format("{0}{1}/{2}", BaseUrl.TrimEnd('/') + "/", resourceName, resourceAction), BaseUrl, methodInfo.GetFullName(), innerException: exceptionInfo.ToException());
                    }
                }

                return JToken.Parse(response);
            }
            catch (Exception ex)
            {
                throw ex.Handle("Invoke", new { Method = methodInfo?.GetFullName() });
            }
        }
    }
}