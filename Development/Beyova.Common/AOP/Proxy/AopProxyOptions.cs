using System;

namespace Beyova.AOP
{
    /// <summary>
    /// Class AopProxyOptions.
    /// </summary>
    internal class AopProxyOptions
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public object Instance { get; set; }

        /// <summary>
        /// Gets or sets the type of the proxied.
        /// </summary>
        /// <value>
        /// The type of the proxied.
        /// </value>
        public Type ProxiedType { get; set; }

        /// <summary>
        /// Gets or sets the method injection delegates.
        /// </summary>
        /// <value>The method injection delegates.</value>
        public MethodInjectionDelegates MethodInjectionDelegates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AopProxyOptions"/> class.
        /// </summary>
        public AopProxyOptions()
        {
        }
    }
}