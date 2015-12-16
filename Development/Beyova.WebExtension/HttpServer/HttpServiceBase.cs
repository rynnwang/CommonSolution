using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Beyova.ExceptionSystem;

namespace Beyova.WebExtension
{
    /// <summary>
    /// Class HttpServerBase.
    /// </summary>
    public abstract partial class HttpServiceBase : ServiceBase, IDisposable
    {
        /// <summary>
        /// The HTTP server
        /// </summary>
        static DelegatedHttpServer httpServer;

        /// <summary>
        /// The locker
        /// </summary>
        protected static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceBase"/> class.
        /// </summary>
        /// <param name="baseAddresses">The base addresses.</param>
        public HttpServiceBase(params Uri[] baseAddresses)
            : this(AuthenticationSchemes.Anonymous, baseAddresses)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceBase"/> class.
        /// </summary>
        /// <param name="authenticationSchema">The authentication schema.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public HttpServiceBase(AuthenticationSchemes authenticationSchema, params Uri[] baseAddresses)
            : base()
        {
            if (httpServer == null)
            {
                lock (locker)
                {
                    if (httpServer == null)
                    {
                        if (baseAddresses != null && baseAddresses.Length > 0)
                        {
                            List<string> uriList = new List<string>();

                            foreach (var one in baseAddresses)
                            {
                                uriList.Add(one.ToString());
                            }

                            httpServer = new DelegatedHttpServer(uriList.ToArray(), ProcessHttpRequest, ProcessException, authenticationSchema);
                        }
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
        /// <param name="ex">The ex.</param>
        public virtual void ProcessException(HttpListenerContext httpContext, Exception ex)
        {
            HttpServerBase.DefaultProcessException(httpContext, ex);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        /// <exception cref="InitializationFailureException">HttpServiceBase</exception>
        protected override void OnStart(string[] args)
        {
            try
            {
                httpServer.Start();
            }
            catch (Exception ex)
            {
                throw new InitializationFailureException("HttpServiceBase", ex);
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        /// <exception cref="OperationFailureException">OnStop</exception>
        protected override void OnStop()
        {
            try
            {
                httpServer.Stop();
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("OnStop", ex);
            }
        }
    }
}
