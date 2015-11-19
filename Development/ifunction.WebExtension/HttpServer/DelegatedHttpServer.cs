using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ifunction.WebExtension
{
    /// <summary>
    /// Class DelegatedHttpServer. This class cannot be inherited.
    /// </summary>
    sealed class DelegatedHttpServer : HttpServerBase, IDisposable
    {
        /// <summary>
        /// Delegate ProcessHttpRequestDelegate
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        internal delegate void ProcessHttpRequestDelegate(HttpListenerContext httpContext);

        /// <summary>
        /// Delegate ProcessExceptionDelegate
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        internal delegate void ProcessExceptionDelegate(HttpListenerContext httpContext, Exception exception);

        /// <summary>
        /// The process HTTP request delegate
        /// </summary>
        private ProcessHttpRequestDelegate processHttpRequestDelegate;

        /// <summary>
        /// The process exception delegate
        /// </summary>
        private ProcessExceptionDelegate processExceptionDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerBase" /> class.
        /// </summary>
        /// <param name="urlPrefixes">The URL prefixes.</param>
        /// <param name="processHttpRequestDelegate">The process HTTP request delegate.</param>
        /// <param name="processExceptionDelegate">The process exception delegate.</param>
        /// <param name="authenticationSchema">The authentication schema.</param>
        public DelegatedHttpServer(string[] urlPrefixes, ProcessHttpRequestDelegate processHttpRequestDelegate, ProcessExceptionDelegate processExceptionDelegate, AuthenticationSchemes authenticationSchema = AuthenticationSchemes.Anonymous)
            : base(urlPrefixes, authenticationSchema)
        {
            this.processExceptionDelegate = processExceptionDelegate;
            this.processHttpRequestDelegate = processHttpRequestDelegate;
        }

        /// <summary>
        /// Processes the HTTP request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public override void ProcessHttpRequest(HttpListenerContext httpContext)
        {
            if (this.processHttpRequestDelegate != null)
            {
                processHttpRequestDelegate(httpContext);
            }
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        public override void ProcessException(HttpListenerContext httpContext, Exception exception)
        {
            if (this.processExceptionDelegate != null)
            {
                processExceptionDelegate(httpContext, exception);
            }
            else
            {
                base.ProcessException(httpContext, exception);
            }
        }
    }
}
