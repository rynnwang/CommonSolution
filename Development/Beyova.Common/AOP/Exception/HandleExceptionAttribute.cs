using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Beyova.ExceptionSystem;

namespace Beyova.AOP
{
    /// <summary>
    /// Class HandleExceptionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HandleExceptionAttribute : BaseAOPAttribute
    {
        /// <summary>
        /// The throw exception
        /// </summary>
        public bool ThrowException { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public HandleExceptionAttribute(bool throwException = true)
            : base("HandleException", new MessageProcessDelegates
            {
                MethodArgumentDelegate = GetMethodArguments
            })
        {
            this.messageDelegates.ExceptionDelegate = HandleException;
            this.ThrowException = throwException;
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="returnedMessage">The returned message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="removeException">if set to <c>true</c> [remove exception].</param>
        /// <returns>Exception.</returns>
        protected Exception HandleException(IMethodReturnMessage returnedMessage, Exception exception, object data, out bool removeException)
        {
            removeException = false;

            if (returnedMessage == null || exception == null)
            {
                return null;
            }

            var operationName = returnedMessage.MethodName;

            var newException = exception.Handle(data, operationName: operationName);

            if (!ThrowException)
            {
                removeException = true;
                Framework.ApiTracking?.LogException(newException.ToExceptionInfo());
            }

            return newException;
        }

        #region Handle exception

        /// <summary>
        /// Gets the method arguments.
        /// </summary>
        /// <param name="callMessage">The call message.</param>
        /// <returns>System.Object.</returns>
        protected static object GetMethodArguments(IMethodCallMessage callMessage)
        {
            if (callMessage == null || callMessage.Args == null) return null;

            var result = new List<object>();
            var parameters = callMessage.MethodBase.GetParameters();

            for (var i = 0; i < callMessage.Args.Length; i++)
            {
                result.Add(new
                {
                    Name = parameters[i].Name,
                    Type = callMessage.Args[i].GetType().FullName,
                    Value = callMessage.Args[i]
                });
            }

            return result;
        }

        #endregion
    }
}
