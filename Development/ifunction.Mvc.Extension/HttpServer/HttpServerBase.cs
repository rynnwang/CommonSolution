using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ifunction.ExceptionSystem;

namespace ifunction.Mvc.Extension
{
    /// <summary>
    /// Class HttpServerBase.
    /// </summary>
    public abstract class HttpServerBase : IDisposable
    {
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
        /// Initializes a new instance of the <see cref="HttpServerBase" /> class.
        /// </summary>
        /// <param name="urlPrefixes">The URL prefixes.</param>
        /// <param name="authenticationSchema">The authentication schema.</param>
        /// <exception cref="InvalidObjectException">urlPrefixes</exception>
        public HttpServerBase(string[] urlPrefixes, AuthenticationSchemes authenticationSchema = AuthenticationSchemes.Anonymous)
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
                        isActive = true;
                        listener.Start();
                        new Thread(new ThreadStart(delegate
                        {
                            while (isActive)
                            {
                                HttpListenerContext httpContext = listener.GetContext();

                                Thread thread = new Thread(new ParameterizedThreadStart(InvokeProcessContext));
                                thread.IsBackground = true;
                                thread.Start(httpContext);
                            }
                        })).Start();
                    }
                }
            }
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
                    }
                }
            }
        }

        /// <summary>
        /// Processes the HTTP request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public abstract void ProcessHttpRequest(HttpListenerContext httpContext);

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        public virtual void ProcessException(HttpListenerContext httpContext, Exception exception)
        {
            DefaultProcessException(httpContext, exception);
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
        /// Invokes the process context.
        /// </summary>
        /// <param name="obj">The object.</param>
        protected void InvokeProcessContext(object obj)
        {
            HttpListenerContext httpContext = obj as HttpListenerContext;

            if (httpContext != null)
            {
                try
                {
                    this.ProcessHttpRequest(httpContext);
                }
                catch (Exception ex)
                {
                    this.ProcessException(httpContext, ex);
                }
                finally
                {
                    httpContext.Response.Close();
                }
            }
        }

        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="content">The content.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="encoding">The encoding.</param>
        protected static void WriteResponse(HttpListenerResponse response, string content, string contentType = "text/html", Encoding encoding = null)
        {
            if (response != null)
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                response.ContentType = contentType;

                using (StreamWriter writer = new StreamWriter(response.OutputStream))
                {
                    writer.WriteLine(content.SafeToString());
                }
            }
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        internal static void DefaultProcessException(HttpListenerContext httpContext, Exception exception)
        {
            httpContext.Response.StatusCode = 500;

            StringBuilder stringBuilder = new StringBuilder("<!DOCTYPE html><html><head><title>Server Error</title>");

            stringBuilder.Append("</head><body>");
            stringBuilder.Append(exception == null ? string.Empty : exception.FormatToHtml());
            stringBuilder.Append("</body><html>");

            WriteResponse(httpContext.Response, stringBuilder.ToString());
        }
    }
}
