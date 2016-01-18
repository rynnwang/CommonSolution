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
        /// The trace identifier
        /// </summary>
        [ThreadStatic]
        internal static string TraceId;

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
        /// <param name="parameters">The parameters.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        public static void Initialize(MethodInfo method, string traceId, List<object> parameters, DateTime? entryStamp = null)
        {
            _root = _current = method.ToTraceLog(parameters, null, entryStamp);
            TraceId = traceId;
        }

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="traceId">The trace identifier.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        internal static void Initialize(RuntimeContext context, string traceId, DateTime? entryStamp = null)
        {
            _root = _current = context.ToTraceLog(null, entryStamp);
            TraceId = traceId;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public static void Dispose()
        {
            _current = null;
            _root = null;
            TraceId = null;
        }

        /// <summary>
        /// Gets the current trace log.
        /// </summary>
        /// <param name="dispose">if set to <c>true</c> [dispose].</param>
        /// <returns>ApiTraceLog.</returns>
        public static ApiTraceLog GetCurrentTraceLog(bool dispose = false)
        {
            try
            {
                _current.CheckNullObject("_current");

                return ConvertApiTraceLog(_current);
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetCurrentTraceLog", dispose);
            }
            finally
            {
                if (dispose)
                {
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Converts the API trace log.
        /// </summary>
        /// <param name="apiTraceLog">The API trace log.</param>
        /// <returns>ApiTraceLog.</returns>
        private static ApiTraceLog ConvertApiTraceLog(RuntimeApiTraceLog apiTraceLog)
        {
            if (apiTraceLog != null)
            {
                var result = new ApiTraceLog
                {
                    EntryStamp = apiTraceLog.EntryStamp,
                    Exception = apiTraceLog.Exception,
                    ExitStamp = apiTraceLog.ExitStamp,
                    MethodFullName = apiTraceLog.MethodFullName,
                    TraceId = TraceId,
                    MethodParameters = apiTraceLog.MethodParameters
                };

                foreach (var one in apiTraceLog.Children)
                {
                    result.InnerTraces.Add(ConvertApiTraceLog(one));
                }

                return result;
            }

            return null;
        }

        #region Enter

        /// <summary>
        /// Enters the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        public static void Enter(MethodInfo method, List<object> parameters = null, DateTime? entryStamp = null)
        {
            Enter(method.ToTraceLog(parameters, _current, entryStamp ?? DateTime.UtcNow));
        }

        /// <summary>
        /// Enters the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        internal static void Enter(RuntimeContext context, DateTime? entryStamp = null)
        {
            Enter(context.ToTraceLog(_current, entryStamp ?? DateTime.UtcNow));
        }

        /// <summary>
        /// Enters the specified method call message.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        internal static void Enter(IMethodCallMessage methodCallMessage, DateTime? entryStamp = null)
        {
            Enter(methodCallMessage.ToTraceLog(_current, entryStamp ?? DateTime.UtcNow));
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
                    traceLog.Parent = _current;
                    _current.Children.Add(traceLog);
                    _current = traceLog;
                }
            }
        }

        #endregion

        /// <summary>
        /// Exits the specified method message.
        /// </summary>
        /// <param name="methodMessage">The method message.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        internal static void Exit(IMethodReturnMessage methodMessage, DateTime? exitStamp = null)
        {
            _current.CheckNullObject("_current", new { methodMessage = methodMessage?.MethodName });
            _current.FillExitInfo(methodMessage.Exception.ToExceptionInfo(), exitStamp ?? DateTime.UtcNow);
        }

        /// <summary>
        /// Exits the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        public static void Exit(Exception exception = null, DateTime? exitStamp = null)
        {
            _current.CheckNullObject("_current");
            _current.FillExitInfo(exception?.ToExceptionInfo(), exitStamp ?? DateTime.UtcNow);
        }
    }
}
