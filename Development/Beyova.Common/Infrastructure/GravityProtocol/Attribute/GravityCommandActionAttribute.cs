using System;
using System.Collections.Generic;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandActionAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class GravityCommandActionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the invoker.
        /// </summary>
        /// <value>The invoker.</value>
        public HashSet<IGravityCommandInvoker> Invokers { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityCommandActionAttribute"/> class.
        /// </summary>
        /// <param name="invokers">The invokers.</param>
        public GravityCommandActionAttribute(params IGravityCommandInvoker[] invokers)
        {
            this.Invokers = new HashSet<IGravityCommandInvoker>(invokers, new GenericEqualityComparer<IGravityCommandInvoker, string>((x) => x.Action, StringComparer.OrdinalIgnoreCase));
        }
    }
}
