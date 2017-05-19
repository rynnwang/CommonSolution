using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.Api
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
        private static ApiTraceLogPiece _current;

        /// <summary>
        /// The _root
        /// </summary>
        [ThreadStatic]
        private static ApiTraceLog _root;

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>The root.</value>
        internal static ApiTraceLog Root { get { return _root; } }

        /// <summary>
        /// Gets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        internal static string TraceId { get { return _root?.TraceId; } }

        /// <summary>
        /// Gets the trace sequence.
        /// </summary>
        /// <value>The trace sequence.</value>
        internal static int? TraceSequence { get { return _root?.TraceSequence; } }

        /// <summary>
        /// Initializes the specified trace identifier.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <param name="traceSequence">The trace sequence.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <param name="methodName">Name of the method.</param>
        public static void Initialize(string traceId, int? traceSequence, DateTime? entryStamp = null, [CallerMemberName] string methodName = null)
        {
            if (!string.IsNullOrWhiteSpace(traceId))
            {
                _root = new ApiTraceLog(entryStamp: entryStamp)
                {
                    TraceId = traceId,
                    TraceSequence = traceSequence.HasValue ? (traceSequence.Value + 1) : 0
                };
                var current = new ApiTraceLogPiece(_root, methodName, entryStamp);
                _root.InnerTraces.Add(current);
                _current = current;
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public static void Dispose()
        {
            _current = null;
            _root = null;
        }

        /// <summary>
        /// Gets the current trace log.
        /// </summary>
        /// <param name="dispose">if set to <c>true</c> [dispose].</param>
        /// <returns>ApiTraceLog.</returns>
        public static ApiTraceLog GetCurrentTraceLog(bool dispose = true)
        {
            try
            {
                if (_root != null)
                {
                    _root.ExitStamp = _root.InnerTraces.Last().ExitStamp;
                }
                return _root;
            }
            catch (Exception ex)
            {
                throw ex.Handle(dispose);
            }
            finally
            {
                if (dispose)
                {
                    Dispose();
                }
            }
        }

        #region Enter

        /// <summary>
        /// Enters the specified method.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="setNameAsMajor">The set name as major.</param>
        public static void Enter(string prefix = null, [CallerMemberName] string methodName = null, bool setNameAsMajor = false)
        {
            Enter(new ApiTraceLogPiece(_current, string.IsNullOrWhiteSpace(prefix) ? methodName : string.Format("{0}.{1}", prefix, methodName), DateTime.UtcNow), setNameAsMajor);
        }

        /// <summary>
        /// Enters the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <param name="setNameAsMajor">The set name as major.</param>
        internal static void Enter(RuntimeContext context, DateTime? entryStamp = null, bool setNameAsMajor = false)
        {
            Enter(context.ToTraceLog(_current, entryStamp ?? DateTime.UtcNow), setNameAsMajor);
        }

        /// <summary>
        /// Enters the specified method call message.
        /// </summary>
        /// <param name="methodCallMessage">The method call message.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <param name="setNameAsMajor">The set name as major.</param>
        internal static void Enter(IMethodCallMessage methodCallMessage, DateTime? entryStamp = null, bool setNameAsMajor = false)
        {
            Enter(methodCallMessage.ToTraceLog(_current, entryStamp ?? DateTime.UtcNow), setNameAsMajor);
        }

        /// <summary>
        /// Enters the specified method call information.
        /// </summary>
        /// <param name="methodCallInfo">The method call information.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        /// <param name="setNameAsMajor">if set to <c>true</c> [set name as major].</param>
        internal static void Enter(AOP.MethodCallInfo methodCallInfo, DateTime? entryStamp = null, bool setNameAsMajor = false)
        {
            Enter(methodCallInfo.ToTraceLog(_current, entryStamp ?? DateTime.UtcNow), setNameAsMajor);
        }

        /// <summary>
        /// Enters the specified trace log.
        /// </summary>
        /// <param name="traceLog">The trace log.</param>
        /// <param name="setNameAsMajor">The set name as major.</param>
        internal static void Enter(ApiTraceLogPiece traceLog, bool setNameAsMajor = false)
        {
            if (traceLog != null)
            {
                if (_root != null)
                {
                    traceLog.Parent = _current;
                    _current.InnerTraces.Add(traceLog);
                    _current = traceLog;

                    if (setNameAsMajor && !string.IsNullOrWhiteSpace(_current.MethodFullName))
                    {
                        _root.MethodFullName = _current.MethodFullName;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Sets the name of the major method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        internal static void SetMajorMethodName(string methodName)
        {
            if (_root != null && !string.IsNullOrWhiteSpace(methodName)) { }
            {
                _root.MethodFullName = methodName;
            }
        }

        /// <summary>
        /// Exits the specified method message.
        /// </summary>
        /// <param name="methodMessage">The method message.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        internal static void Exit(IMethodReturnMessage methodMessage, DateTime? exitStamp = null)
        {
            Exit(_current, (methodMessage.Exception as BaseException)?.Key, exitStamp);
        }

        /// <summary>
        /// Exits the specified exception.
        /// </summary>
        /// <param name="exceptionKey">The exception key.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        public static void Exit(Guid? exceptionKey, DateTime? exitStamp = null)
        {
            Exit(_current, exceptionKey, exitStamp);
        }

        /// <summary>
        /// Fills the exit information.
        /// </summary>
        /// <param name="piece">The piece.</param>
        /// <param name="exceptionKey">The exception key.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        private static void Exit(ApiTraceLogPiece piece, Guid? exceptionKey, DateTime? exitStamp = null)
        {
            if (piece != null)
            {
                piece.ExceptionKey = exceptionKey;
                piece.ExitStamp = exitStamp ?? DateTime.UtcNow;
                _current = piece.Parent;
            }

            if (_root != null && !_root.ExceptionKey.HasValue)
            {
                _root.ExceptionKey = exceptionKey;
            }
        }
    }
}
