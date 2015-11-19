﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace ifunction.AOP
{
    /// <summary>
    /// Class HandleExceptionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HandleExceptionAttribute : BaseAOPAttribute
    {
        /// <summary>
        /// The server identifier
        /// </summary>
        public string ServerIdentifier { get; protected set; }

        /// <summary>
        /// The throw exception
        /// </summary>
        public bool ThrowException { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class.
        /// </summary>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public HandleExceptionAttribute(string serverIdentifier, bool throwException)
            : base("HandleException", new MessageProcessDelegates
            {
                MethodArgumentDelegate = GetMethodArguments
            })
        {
            this.messageDelegates.ExceptionDelegate = HandleException;
            this.ServerIdentifier = serverIdentifier;
            this.ThrowException = throwException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute"/> class.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public HandleExceptionAttribute(bool throwException)
            : this(string.Empty, throwException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class.
        /// </summary>
        public HandleExceptionAttribute()
            : this(string.Empty, true)
        {
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

            if (returnedMessage == null || exception == null) return null;

            var operationName = returnedMessage.MethodName;
            var operatorIdentity = Framework.OperatorInfo.ObjectToString();

            var newException = exception.Handle(operationName, operatorIdentity, data);

            if (!ThrowException)
            {
                removeException = true;

                if (Framework.ApiTracking != null)
                {
                    Framework.ApiTracking.LogExceptionAsync(newException, this.ServerIdentifier, EnvironmentCore.ServerName);
                }
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
