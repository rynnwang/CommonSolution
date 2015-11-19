using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using ifunction.ExceptionSystem;
using Newtonsoft.Json;

namespace ifunction.WebExtension.HttpLongPolling
{
    /// <summary>
    /// Class HttpPollingServerBase.
    /// </summary>
    public abstract class HttpPollingServerBase : IDisposable
    {
        /// <summary>
        /// The listener thread
        /// </summary>
        static Thread listenerThread = null;

        /// <summary>
        /// The locker
        /// </summary>
        protected object locker = new object();

        /// <summary>
        /// The listener
        /// </summary>
        protected HttpListener listener = new HttpListener();

        /// <summary>
        /// The is active
        /// </summary>
        protected bool isActive = false;

        /// <summary>
        /// The context dictionary
        /// </summary>
        protected PollingDictionary<HttpListenerContext> contextDictionary = new PollingDictionary<HttpListenerContext>();

        /// <summary>
        /// Gets the major identifiers.
        /// </summary>
        /// <value>The major identifiers.</value>
        public ICollection<string> MajorIdentifiers
        {
            get
            {
                return contextDictionary.Keys;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPollingServerBase" /> class.
        /// </summary>
        /// <param name="urlPrefixes">The URL prefixes.</param>
        /// <param name="authenticationSchema">The authentication schema.</param>
        /// <exception cref="InvalidObjectException">urlPrefixes</exception>
        public HttpPollingServerBase(string[] urlPrefixes, AuthenticationSchemes authenticationSchema = AuthenticationSchemes.Anonymous)
        {
            // URI prefixes are required,
            // for example "http://ifunction.org:8080/index/".
            if (urlPrefixes == null || urlPrefixes.Length == 0)
            {
                throw new InvalidObjectException("urlPrefixes");
            }

            // Add the prefixes.
            foreach (string s in urlPrefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.AuthenticationSchemes = authenticationSchema;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (!isActive)
            {
                lock (locker)
                {
                    if (!isActive)
                    {
                        if (listenerThread == null)
                        {
                            isActive = true;
                            listenerThread = new Thread(new ThreadStart(EstablishListening)) { IsBackground = true };
                            listenerThread.Start();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Establishes the listening.
        /// </summary>
        private void EstablishListening()
        {
            listener.Start();

            IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            result.AsyncWaitHandle.WaitOne();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (isActive)
            {
                lock (locker)
                {
                    if (isActive)
                    {
                        isActive = false;
                        listener.Stop();
                        listenerThread = null;
                    }
                }
            }
        }

        /// <summary>
        /// Listeners the callback.
        /// </summary>
        /// <param name="result">The result.</param>
        public void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            // For next coming request
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);

            HttpListenerContext context = null;

            try
            {
                context = listener.EndGetContext(result);
            }
            catch { }

            if (context != null)
            {
                try
                {
                    var httpRequest = context.Request;

                    var message = new PollingMessage
                    {
                        Data = httpRequest.GetPostJson(),
                        Sender = httpRequest.Headers.Get(HeaderKeys.headerKey_From),
                        Receiver = httpRequest.Headers.Get(HeaderKeys.headerKey_To)
                    };

                    PollingAction action = PollingAction.None;
                    Enum.TryParse<PollingAction>(httpRequest.Headers.Get(HeaderKeys.headerKey_Action), true, out action);

                    var batchSize = httpRequest.Headers.Get(HeaderKeys.headerKey_BatchSize).ToInt32();
                    var authenticationString = httpRequest.Headers.Get(HttpRequestHeader.Authorization.ToString());

                    if (message.IsValid)
                    {
                        var responseBody = ProcessHttpRequest(message, action, batchSize, authenticationString, httpRequest.Headers, httpRequest.Cookies);

                        // NOTE: Here just check whether is null, instead of string,IsNullOrEmpty().
                        // Because, when PUSH, the destination is polling, message would deliver to context directly, then return string.Empty.
                        if (responseBody != null)
                        {
                            // Response immediately.
                            context.Response.WriteAllContent(responseBody, Encoding.UTF8, "application/json", true);
                        }
                        else
                        {
                            // Hold connection for long polling.
                            // NOTE that: when the request comes, the receiver is set as operator so that it can get history record.
                            // So when no history need to processed, long polling would be established using ReceiverPollingIdentifier!
                            var operatorPollingIdentifier = message.ReceiverPollingIdentifier;
                            if (this.contextDictionary.CreateOrReplaceObject(operatorPollingIdentifier.Identifier, context))
                            {
                                RegisterPollingIdentifierEvent(operatorPollingIdentifier);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ProcessException(context, exception);
                }
            }
        }

        /// <summary>
        /// Responses the context.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="headerCollection">The header collection.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns><c>true</c> if find and response the data, <c>false</c> otherwise.</returns>
        public bool ResponseContext(PollingIdentifier identifier, string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK, WebHeaderCollection headerCollection = null, CookieCollection cookieCollection = null)
        {
            bool needToRemove = false;
            Dictionary<string, HttpListenerContext> contexts = new Dictionary<string, HttpListenerContext>();
            List<string> fullIdentifiersToBeRemoved = new List<string>();

            var resourceIdentifier = string.Empty;
            var majorIdentifier = PollingDictionary<HttpListenerContext>.SplitIdentifier(identifier.Identifier, out  resourceIdentifier);

            if (!string.IsNullOrWhiteSpace(majorIdentifier))
            {
                if (string.IsNullOrWhiteSpace(resourceIdentifier))
                {
                    var contextDictionary = this.contextDictionary.GetObjectsByMajorIdentifier(identifier.Identifier);
                    foreach (var one in contextDictionary)
                    {
                        contexts.Add(one.Key, one.Value);
                    }
                }
                else
                {
                    var context = this.contextDictionary.GetObjectByFullIdentifier(majorIdentifier, resourceIdentifier);
                    if (context != null)
                    {
                        contexts.Add(identifier.Identifier, context);
                    }
                }
            }

            if (contexts != null && contexts.Count > 0)
            {
                try
                {
                    foreach (var one in contexts)
                    {
                        var context = one.Value;

                        try
                        {
                            context.Response.StatusCode = (int)statusCode;

                            if (headerCollection != null)
                            {
                                foreach (var key in headerCollection.AllKeys)
                                {
                                    context.Response.Headers.Set(key, headerCollection[key]);
                                }
                            }

                            if (cookieCollection != null)
                            {
                                foreach (Cookie cookie in cookieCollection)
                                {
                                    context.Response.Cookies.Add(cookie);
                                }
                            }

                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseBody.SafeToString());
                            context.Response.ContentLength64 = buffer.Length;
                            System.IO.Stream output = context.Response.OutputStream;
                            output.Write(buffer, 0, buffer.Length);
                            output.Close();

                            needToRemove = true;
                        }
                        catch (Exception ex)
                        {
                            needToRemove = true;
                            ProcessException(context, ex);
                        }
                        finally
                        {
                            context.Response.Close();
                            fullIdentifiersToBeRemoved.Add(one.Key);
                        }

                        try
                        {
                            ResponseContextEvent(context);
                        }
                        catch { }
                    }

                    return true;
                }
                finally
                {
                    if (needToRemove)
                    {
                        foreach (var one in fullIdentifiersToBeRemoved)
                        {
                            this.contextDictionary.RemoveObject(one);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (isActive)
            {
                Stop();
                listener.Close();
            }
        }

        /// <summary>
        /// Responses the context event.
        /// </summary>
        /// <param name="context">The context.</param>
        protected virtual void ResponseContextEvent(HttpListenerContext context)
        {
            //To do nothing here.
        }

        /// <summary>
        /// Registers the polling identifier event.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        protected virtual void RegisterPollingIdentifierEvent(PollingIdentifier identifier)
        {
            //To do nothing here.
        }

        /// <summary>
        /// Processes the HTTP request.
        /// If returned string is not null, then it would be filled in response and return.
        /// Otherwise, the connection would be hold as long polling.
        /// Generally, only in Pull, it might happen to hold as long polling.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="action">The action.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="authenticationString">The authentication string.</param>
        /// <param name="headerCollection">The header collection.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="InvalidObjectException">action;null</exception>
        protected string ProcessHttpRequest(PollingMessage message, PollingAction action, int batchSize, string authenticationString, NameValueCollection headerCollection, CookieCollection cookieCollection)
        {
            string result = null;

            try
            {
                Authenticate(message, authenticationString);

                switch (action)
                {
                    case PollingAction.Fetch:
                        result = ProcessFetchRequest(message, headerCollection, cookieCollection);
                        break;
                    case PollingAction.Pull:
                        result = ProcessPullRequest(message, batchSize, headerCollection, cookieCollection);
                        break;
                    case PollingAction.Push:
                        if (!ResponseContext(message.ReceiverPollingIdentifier, CreateResponseString(CreatePollingMessageResult(message))))
                        {
                            result = ProcessPushRequest(message, headerCollection, cookieCollection);
                        }
                        else
                        {
                            return CreateResponseString(string.Empty);
                        }
                        break;
                    default:
                        throw new InvalidObjectException("action", null, action);
                }
            }
            catch (Exception ex)
            {
                var exceptionInfo = ex.Handle(action.ToString(), new { message, action, batchSize, authenticationString }).ToExceptionInfo();

                return JsonConvert.SerializeObject(new
                {
                    ExceptionInfo = exceptionInfo
                });
            }

            return result;
        }

        /// <summary>
        /// Authenticates the specified from.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="authenticationString">The authentication string.</param>
        protected virtual void Authenticate(PollingMessage message, string authenticationString)
        {
            //To do nothing here. Should be implemented in inherited classes.
        }

        /// <summary>
        /// Processes the pull request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="headerCollection">The header collection.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.String.</returns>
        protected abstract string ProcessPullRequest(PollingMessage message, int batchSize, NameValueCollection headerCollection, CookieCollection cookieCollection);

        /// <summary>
        /// Processes the push request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="headerCollection">The header collection.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.String.</returns>
        protected abstract string ProcessPushRequest(PollingMessage message, NameValueCollection headerCollection, CookieCollection cookieCollection);

        /// <summary>
        /// Processes the fetch request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="headerCollection">The header collection.</param>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <returns>System.String.</returns>
        protected abstract string ProcessFetchRequest(PollingMessage message, NameValueCollection headerCollection, CookieCollection cookieCollection);

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        protected virtual void ProcessException(HttpListenerContext httpContext, Exception exception)
        {
            DefaultProcessException(httpContext, exception);
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        internal static void DefaultProcessException(HttpListenerContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.WriteAllContent(exception == null ? string.Empty : exception.FormatToString());
        }


        /// <summary>
        /// Creates the response string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="exceptionInfo">The exception information.</param>
        /// <returns>System.String.</returns>
        protected string CreateResponseString<T>(T data, ExceptionInfo exceptionInfo = null)
        {
            return JsonConvert.SerializeObject(new
            {
                ExceptionInfo = exceptionInfo,
                Data = data
            });
        }

        /// <summary>
        /// Creates the polling message result.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>List&lt;PollingMessage&gt;.</returns>
        protected List<PollingMessage> CreatePollingMessageResult(PollingMessage message)
        {
            List<PollingMessage> result = new List<PollingMessage>();

            if (message != null)
            {
                result.Add(message);
            }

            return result;
        }
    }
}
