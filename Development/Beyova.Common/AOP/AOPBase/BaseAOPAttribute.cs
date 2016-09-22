using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace Beyova.AOP
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
    }
}
