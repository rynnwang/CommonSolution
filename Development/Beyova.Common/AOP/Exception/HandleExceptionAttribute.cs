using System;

namespace Beyova.AOP
{
    /// <summary>
    /// Class HandleExceptionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [Obsolete("MarshalObject Based AOP is retired. Proxy based AOP can customize injection implementation when it is wrapped.")]
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
            : base("HandleException", new MethodMessageInjectionDelegates
            {
            })
        {
            this.MethodMessageInjectionDelegates.ExceptionDelegate = HandleException;
            this.ThrowException = throwException;
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="methodCallInfo">The method call information.</param>
        /// <returns>
        /// Exception.
        /// </returns>
        protected Exception HandleException(MethodCallInfo methodCallInfo)
        {
            if (methodCallInfo?.Exception == null)
            {
                return null;
            }

            var newException = methodCallInfo.Exception.Handle(methodCallInfo.GetExceptionReferenceObject(), operationName: methodCallInfo.MethodFullName);

            if (!this.ThrowException)
            {
                Framework.ApiTracking?.LogException(newException.ToExceptionInfo());
                return null;
            }

            return newException;
        }

        #region Handle exception

        /// <summary>
        /// Gets the method arguments.
        /// </summary>
        /// <param name="methodCallInfo">The method call information.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        protected static object GetMethodArguments(MethodCallInfo methodCallInfo)
        {
            return methodCallInfo?.InArgs;
        }

        #endregion Handle exception
    }
}