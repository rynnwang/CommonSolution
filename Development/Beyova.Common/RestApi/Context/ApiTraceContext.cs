using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiTraceContext.
    /// </summary>
    public static class ApiTraceContext
    {
        /// <summary>
        /// The _current
        /// </summary>
        [ThreadStatic]
        private static RuntimeApiTraceLog _current;

        /// <summary>
        /// The _root
        /// </summary>
        [ThreadStatic]
        private static RuntimeApiTraceLog _root;

        /// <summary>
        /// Gets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        public static string TraceId
        {
            get { return _current?.RawTraceLog?.TraceId; }
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>The root.</value>
        internal static RuntimeApiTraceLog Root { get { return _root; } }

        /// <summary>
        /// Initializes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="traceId">The trace identifier.</param>
        public static void Initialize(MethodInfo method, string traceId)
        {
            var trace = method.ToTraceLog(null);
            trace.RawTraceLog.TraceId = traceId;

            Enter(trace);
        }

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="traceId">The trace identifier.</param>
        internal static void Initialize(RuntimeContext context, string traceId, DateTime entryStamp)
        {
            var trace = context.ToTraceLog(null);
            trace.RawTraceLog.TraceId = traceId;
            trace.RawTraceLog.EntryStamp = entryStamp;

            Enter(trace);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public static void Dispose()
        {
            _current = null;
            _root = null;
        }

        public static ApiTraceLog GetCurrentTraceLog(bool dispose = false)
        {
            //Todo;
            return null;
        }

        #region Enter

        /// <summary>
        /// Enters the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        public static void Enter(MethodInfo method)
        {
            Enter(method.ToTraceLog(_current));
        }

        /// <summary>
        /// Enters the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        internal static void Enter(RuntimeContext context)
        {
            Enter(context.ToTraceLog(_current));
        }

        /// <summary>
        /// Enters the specified trace log.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        internal static void Enter(RuntimeApiTraceLog traceLog)
        {
            if (traceLog != null)
            {
                if (_root != null)
                {
                    //Todo
                }
                else
                {
                    _root = _current = traceLog;
                }
            }
        }

        #endregion

        /// <summary>
        /// Exits the specified method message.
        /// </summary>
        /// <param name="methodMessage">The method message.</param>
        internal static void Exit(IMethodReturnMessage methodMessage)
        {
            _current.CheckNullObject("_current", new { methodMessage = methodMessage?.MethodName });

            _current.RawTraceLog.ExitStamp = DateTime.UtcNow;
            _current.RawTraceLog.Exception = methodMessage.Exception.ToExceptionInfo();
        }

        /// <summary>
        /// Exits the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Exit(Exception exception = null, DateTime? exitStamp = null)
        {
            _current.CheckNullObject("_current");
            _current.RawTraceLog.Exception = exception?.ToExceptionInfo();
            _current.RawTraceLog.ExitStamp = exitStamp ?? DateTime.UtcNow;
        }
    }
}
