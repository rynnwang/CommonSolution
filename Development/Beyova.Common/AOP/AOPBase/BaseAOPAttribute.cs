using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace Beyova.AOP
{
    /// <summary>
    /// Class BaseAOPAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Obsolete("MarshalObject Based AOP is retired. Please use Proxy based AOP")]
    public abstract class BaseAOPAttribute : ContextAttribute
    {
        /// <summary>
        /// The message delegates
        /// </summary>
        protected MethodInjectionDelegates messageDelegates;

        /// <summary>
        /// Gets the method injection delegates.
        /// </summary>
        /// <value>The method injection delegates.</value>
        internal MethodInjectionDelegates MethodInjectionDelegates { get { return messageDelegates; } }

        /// <summary>
        /// Gets the method message injection delegates.
        /// </summary>
        /// <value>
        /// The method message injection delegates.
        /// </value>
        internal MethodMessageInjectionDelegates MethodMessageInjectionDelegates { get { return messageDelegates as MethodMessageInjectionDelegates; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAOPAttribute"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="messageDelegates">The message delegates.</param>
        public BaseAOPAttribute(string attributeName, MethodMessageInjectionDelegates messageDelegates)
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
            constructionCallMessage.ContextProperties.Add(new AOPMessageProcessor(this.Name, this.MethodMessageInjectionDelegates));
        }
    }
}