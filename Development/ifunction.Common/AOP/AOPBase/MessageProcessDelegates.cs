using System;
using System.Runtime.Remoting.Messaging;

namespace ifunction.AOP
{
    #region Delegate

    /// <summary>
    /// Delegate HandleReturnedMessage
    /// </summary>
    /// <param name="returnMessage">The return message.</param>
    public delegate void ProcessReturnedMessageDelegate(IMethodReturnMessage returnMessage);

    /// <summary>
    /// Delegate HandleMessageCallDelegate
    /// </summary>
    /// <param name="callMessage">The call message.</param>
    public delegate void ProcessMessageCallDelegate(IMethodCallMessage callMessage);

    /// <summary>
    /// Delegate HandleExceptionDelegate
    /// </summary>
    /// <param name="returnMessage">The return message.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="data">The data.</param>
    /// <param name="removeException">if set to <c>true</c> [remove exception].</param>
    /// <returns>Exception.</returns>
    public delegate Exception ProcessExceptionDelegate(IMethodReturnMessage returnMessage, Exception exception, object data, out bool removeException);

    /// <summary>
    /// Delegate RetrieveMethodArgumentDelegate
    /// </summary>
    /// <param name="callMessage">The call message.</param>
    /// <returns>System.Object.</returns>
    public delegate object RetrieveMethodArgumentDelegate(IMethodCallMessage callMessage);

    /// <summary>
    /// Delegate OverrideMethodReturnMessageDelegate
    /// </summary>
    /// <param name="callMessage">The call message.</param>
    /// <param name="returnMessage">The return message.</param>
    /// <param name="exception">The exception. This is the exception which from <see cref="ProcessExceptionDelegate"/> delegate.</param>
    /// <returns>IMethodReturnMessage.</returns>
    public delegate IMethodReturnMessage OverrideMethodReturnMessageDelegate(IMethodCallMessage callMessage, IMethodReturnMessage returnMessage, Exception exception);

    #endregion

    /// <summary>
    /// Class MessageProcessDelegates.
    /// </summary>
    public class MessageProcessDelegates
    {
        /// <summary>
        /// Gets or sets the return message delegate.
        /// This delegate would be invoked after real method is invoked.
        /// </summary>
        /// <value>The return message delegate.</value>
        public ProcessReturnedMessageDelegate ReturnMessageDelegate { get; set; }

        /// <summary>
        /// Gets or sets the message call delegate.
        /// This delegate would be invoked before real method is invoked.
        /// </summary>
        /// <value>The message call delegate.</value>
        public ProcessMessageCallDelegate MessageCallDelegate { get; set; }

        /// <summary>
        /// Gets or sets the exception delegate.
        /// This delegate would be invoked only when real method threw exception.
        /// It would be invoked after <c>ReturnMessageDelegate</c> is done.
        /// </summary>
        /// <value>The exception delegate.</value>
        public ProcessExceptionDelegate ExceptionDelegate { get; set; }

        /// <summary>
        /// Gets or sets the method argument delegate.
        /// This delegate would be invoked for generating exception data.
        /// By default, it would use default method for generating key-value based argument dictionary.
        /// </summary>
        /// <value>The method argument delegate.</value>
        public RetrieveMethodArgumentDelegate MethodArgumentDelegate { get; set; }

        /// <summary>
        /// Gets or sets the method return message delegate.
        /// This delegate would be invoked after <c>ReturnMessageDelegate</c> and <c>ExceptionDelegate</c>.
        /// This is to override <see cref="ReturnMessage"/> (<see cref="IMethodReturnMessage"/>).
        /// </summary>
        /// <value>The method return message delegate.</value>
        public OverrideMethodReturnMessageDelegate ReturnMessageOverrideDelegate { get; set; }
    }
}
