using System;

namespace Beyova.AOP
{
    #region Delegate

    /// <summary>
    /// Delegate AfterMethodInvokeDelegate
    /// </summary>
    /// <param name="methodCallInfo">The method call information.</param>
    public delegate void AfterMethodInvokeDelegate(MethodCallInfo methodCallInfo);

    /// <summary>
    /// Delegate BeforeMethodInvokeDelegate
    /// </summary>
    /// <param name="callMessage">The call message.</param>
    public delegate void BeforeMethodInvokeDelegate(MethodCallInfo callMessage);

    /// <summary>
    /// Delegate HandleExceptionDelegate. If return not null, that exception would be throw out.
    /// </summary>
    /// <param name="methodCallInfo">The method call information.</param>
    /// <returns>
    /// Exception.
    /// </returns>
    public delegate Exception ProcessExceptionDelegate(MethodCallInfo methodCallInfo);

    #endregion Delegate

    /// <summary>
    /// Class MethodInjectionDelegates.
    /// </summary>
    public class MethodInjectionDelegates
    {
        /// <summary>
        /// Gets or sets the method invoked event.
        /// This delegate would be invoked after real method is invoked.
        /// </summary>
        /// <value>The method invoked event.</value>
        public AfterMethodInvokeDelegate MethodInvokedEvent { get; set; }

        /// <summary>
        /// Gets or sets the method invoking event.
        /// This delegate would be invoked before real method is invoked.
        /// </summary>
        /// <value>The method invoking event.</value>
        public BeforeMethodInvokeDelegate MethodInvokingEvent { get; set; }

        /// <summary>
        /// Gets or sets the exception delegate.
        /// This delegate would be invoked only when real method threw exception.
        /// It would be invoked after <c>ReturnMessageDelegate</c> is done.
        /// </summary>
        /// <value>The exception delegate.</value>
        public ProcessExceptionDelegate ExceptionDelegate { get; set; }
    }
}