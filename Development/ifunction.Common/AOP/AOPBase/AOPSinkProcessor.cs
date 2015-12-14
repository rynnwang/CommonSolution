using System;
using System.Runtime.Remoting.Messaging;

namespace ifunction.AOP
{
    /// <summary>
    /// Class AOPSinkProcessor.
    /// </summary>
    public class AOPSinkProcessor : IMessageSink
    {
        /// <summary>
        /// The method injection delegate
        /// </summary>
        protected MessageProcessDelegates messageDelegates;

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public MarshalByRefObject Sender
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the next message sink in the sink chain.
        /// </summary>
        /// <value>The next sink.</value>
        /// <returns>The next message sink in the sink chain.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
        ///   </PermissionSet>
        public IMessageSink NextSink
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AOPSinkProcessor" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="nextSink">The next sink.</param>
        /// <param name="messageDelegates">The message delegates.</param>
        public AOPSinkProcessor(MarshalByRefObject sender, IMessageSink nextSink, MessageProcessDelegates messageDelegates)
        {
            this.NextSink = nextSink;
            this.Sender = sender;

            this.messageDelegates = messageDelegates;
        }

        #region IMessageSink Member

        /// <summary>
        /// Asynchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <param name="replySink">The reply sink for the reply message.</param>
        /// <returns>Returns an <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl" /> interface that provides a way to control asynchronous messages after they have been dispatched.</returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        /// <summary>
        /// Synchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A reply message in response to the request.</returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMethodCallMessage call = msg as IMethodCallMessage;

            if (this.messageDelegates != null && this.messageDelegates.MessageCallDelegate != null)
            {
                this.messageDelegates.MessageCallDelegate.Invoke(call);
            }

            var methodReturn = this.NextSink.SyncProcessMessage(call);

            IMethodReturnMessage returnMessage = methodReturn as IMethodReturnMessage;

            // To run ReturnMessageDelegate first in case any exception is thrown from ExceptionDelegate to interrupt it.
            if (this.messageDelegates.ReturnMessageDelegate != null)
            {
                this.messageDelegates.ReturnMessageDelegate.Invoke(returnMessage);
            }

            Exception exception = null;

            if (returnMessage != null && this.messageDelegates != null)
            {
                if (returnMessage.Exception != null && this.messageDelegates.ExceptionDelegate != null)
                {
                    bool removeException;
                    exception = this.messageDelegates.ExceptionDelegate(returnMessage, returnMessage.Exception, (this.messageDelegates.MethodArgumentDelegate ?? GetMethodArguments)(call), out removeException);
                    if (removeException)
                    {
                        returnMessage = new ReturnMessage(returnMessage.ReturnValue, returnMessage.OutArgs, returnMessage.OutArgCount, returnMessage.LogicalCallContext, call);
                    }
                }
            }

            if (this.messageDelegates.ReturnMessageOverrideDelegate != null)
            {
                var overridedReturnMessage = this.messageDelegates.ReturnMessageOverrideDelegate(call, returnMessage, exception);

                if (overridedReturnMessage != null)
                {
                    returnMessage = overridedReturnMessage;
                }
            }

            return returnMessage;
        }

        #endregion

        /// <summary>
        /// Methods the arguments to exception data.
        /// </summary>
        /// <param name="callMessage">The call message.</param>
        /// <returns>System.Object.</returns>
        protected object GetMethodArguments(IMethodCallMessage callMessage)
        {
            return callMessage != null ? callMessage.Args : null;
        }
    }
}
