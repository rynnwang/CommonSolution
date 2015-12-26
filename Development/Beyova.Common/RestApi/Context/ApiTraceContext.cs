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

        #region Constructors      

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceContext"/> class.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        internal ApiTraceContext(string traceId)
        {
            this.Chain = new List<ApiTraceLog>();
            this.TraceId = traceId;
        }

        #endregion

        #region Enter

        internal void Enter(MethodInfo method)
        {
            Enter(method.ToTraceLog());
        }

        internal void Enter(RuntimeContext context)
        {
            Enter(context.ToTraceLog());
        }

        private void Enter(ApiTraceLog traceLog)
        {
            if (traceLog != null)
            {
                Chain.Add(traceLog);
                currentIndex = Chain.Count - 1;
            }
        }

        #endregion

        /// <summary>
        /// Gets the parent trace log.
        /// </summary>
        /// <returns>ApiTraceLog.</returns>
        private ApiTraceLog GetParentTraceLog()
        {
            return currentIndex > 0 ? Chain[(currentIndex - 1)] : null;
        }

        /// <summary>
        /// Exits this instance.
        /// </summary>
        internal void Exit()
        {
            Chain.RemoveFrom(currentIndex);

            currentIndex--;
            if (currentIndex < 0)
            {
                throw new OperationFailureException("Exit", hintMessage: "Exited index is below 0, which indicates Enter and Exit are not mathed.");
            }
        }
    }
}
