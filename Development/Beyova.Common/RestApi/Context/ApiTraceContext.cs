using System.Collections.Generic;
using System.Reflection;
using Beyova.ApiTracking.Model;
using Beyova.ExceptionSystem;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiTraceContext.
    /// </summary>
    public sealed class ApiTraceContext
    {
        /// <summary>
        /// Gets the chain.
        /// </summary>
        /// <value>The chain.</value>
        public List<ApiTraceLog> Chain { get; private set; }

        /// <summary>
        /// The current index
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Gets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        public string TraceId { get; private set; }

        /// <summary>
        /// Gets the current trace.
        /// </summary>
        /// <value>The current trace.</value>
        public ApiTraceLog CurrentTrace { get { return Chain[currentIndex]; } }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>The root.</value>
        public ApiTraceLog Root { get { return Chain[0]; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="traceId">The trace identifier.</param>
        internal ApiTraceContext(RuntimeContext context, string traceId = null)
        {
            this.Chain = new List<ApiTraceLog>();
            this.Chain.AddIfNotNull(RuntimeContextToTraceLog(context));
            this.TraceId = traceId;
        }

        internal void Enter(MethodInfo method)
        {

        }

        /// <summary>
        /// Exits this instance.
        /// </summary>
        internal void Exit()
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                throw new OperationFailureException("Exit", hintMessage: "Exited index is below 0, which indicates Enter and Exit are not mathed.");
            }
        }

        /// <summary>
        /// Runtimes the context to trace log.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Beyova.ApiTracking.Model.ApiTraceLog.</returns>
        private static ApiTraceLog RuntimeContextToTraceLog(RuntimeContext context)
        {
            if (context != null)
            {
                return new ApiTraceLog
                {
                    MethodFullName = context.ApiMethod?.GetFullName(),
                    MethodParameters = new Dictionary<string, object> {
                        { "UrlParameter",context.IsActionUsed ? context.Parameter2 : context.Parameter1} }
                };
            }

            return null;
        }
    }
}
