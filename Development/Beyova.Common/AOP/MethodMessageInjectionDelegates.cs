using System;
using System.Runtime.Remoting.Messaging;

namespace Beyova.AOP
{
    #region Delegate

    /// <summary>
    /// Delegate OverrideMethodReturnMessageDelegate
    /// </summary>
    /// <param name="methodCallInfo">The method call information.</param>
    /// <param name="returnMessage">The return message.</param>
    /// <param name="exception">The exception. This is the exception which from <see cref="ProcessExceptionDelegate" /> delegate.</param>
    /// <returns>
    /// IMethodReturnMessage.
    /// </returns>
    public delegate IMethodReturnMessage OverrideMethodReturnMessageDelegate(IMethodCallMessage methodCallInfo, IMethodReturnMessage returnMessage, Exception exception);

    #endregion Delegate

    /// <summary>
    /// Class MethodInjectionDelegates.
    /// </summary>
    public class MethodMessageInjectionDelegates : MethodInjectionDelegates
    {
        /// <summary>
        /// Gets or sets the method return message delegate.
        /// This delegate would be invoked after <c>ReturnMessageDelegate</c> and <c>ExceptionDelegate</c>.
        /// This is to override <see cref="ReturnMessage"/> (<see cref="IMethodReturnMessage"/>).
        /// </summary>
        /// <value>The method return message delegate.</value>
        public OverrideMethodReturnMessageDelegate ReturnMessageOverrideDelegate { get; set; }
    }
}