using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading;

namespace ifunction.AOP
{
    /// <summary>
    /// Class BaseAOPAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseAOPAttribute : ContextAttribute
    {
        /// <summary>
        /// The message delegates
        /// </summary>
        protected MessageProcessDelegates messageDelegates;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAOPAttribute"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="messageDelegates">The message delegates.</param>
        public BaseAOPAttribute(string attributeName, MessageProcessDelegates messageDelegates)
            : base(attributeName)
        {
            this.messageDelegates = messageDelegates;
        }

        /// <summary>
        /// Gets the properties for new context.
        /// </summary>
        /// <param name="constructionCallMessage">The construction call message.</param>
        public override void GetPropertiesForNewContext(IConstructionCallMessage constructionCallMessage)
        {
            constructionCallMessage.ContextProperties.Add(new AOPMessageProcessor(this.Name, this.messageDelegates));
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        protected static string ApplicationName
        {
            get
            {
                string name = string.Empty;
                if (AppDomain.CurrentDomain != null)
                {
                    name = AppDomain.CurrentDomain.FriendlyName;
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    name = Assembly.GetEntryAssembly().FullName;
                }

                return name;
            }
        }
    }
}
