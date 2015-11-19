using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace ifunction.AOP
{
    /// <summary>
    /// Class BaseAOPMessageProcessor.
    /// </summary>
    public class AOPMessageProcessor : IContextProperty, IContributeObjectSink
    {
        /// <summary>
        /// The handle exception delegate
        /// </summary>
        protected MessageProcessDelegates messageDelegates;

        /// <summary>
        /// Initializes a new instance of the <see cref="AOPMessageProcessor" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="messageDelegates">The message delegates.</param>
        public AOPMessageProcessor(string name, MessageProcessDelegates messageDelegates)
        {
            this.Name = name;
            this.messageDelegates = messageDelegates;
        }

        #region IContextProperty Member

        /// <summary>
        /// Called when the context is frozen.
        /// </summary>
        /// <param name="newContext">The context to freeze.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
        ///   </PermissionSet>
        public virtual void Freeze(Context newContext)
        {

        }

        /// <summary>
        /// Returns a Boolean value indicating whether the context property is compatible with the new context.
        /// </summary>
        /// <param name="newCtx">The new context in which the <see cref="T:System.Runtime.Remoting.Contexts.ContextProperty" /> has been created.</param>
        /// <returns>true if the context property can coexist with the other context properties in the given context; otherwise, false.</returns>
        public virtual bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        /// <summary>
        /// Gets the name of the property under which it will be added to the context.
        /// </summary>
        /// <value>The name.</value>
        /// <returns>The name of the property.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
        ///   </PermissionSet>
        public string Name
        {
            get;
            protected set;
        }

        #endregion

        #region IContributeObjectSink Member

        /// <summary>
        /// Chains the message sink of the provided server object in front of the given sink chain.
        /// </summary>
        /// <param name="obj">The server object which provides the message sink that is to be chained in front of the given chain.</param>
        /// <param name="nextSink">The chain of sinks composed so far.</param>
        /// <returns>The composite sink chain.</returns>
        public IMessageSink GetObjectSink(MarshalByRefObject obj, System.Runtime.Remoting.Messaging.IMessageSink nextSink)
        {
            return new AOPSinkProcessor(obj, nextSink, this.messageDelegates);
        }

        #endregion
    }
}
