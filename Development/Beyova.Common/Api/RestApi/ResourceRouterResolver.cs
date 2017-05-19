using System;
using System.Collections.Generic;
using System.Reflection;
using Beyova.Api;
using Beyova.Cache;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ResourceRouterResolver.
    /// </summary>
    public class ResourceRouterResolver
    {
        /// <summary>
        /// The routes
        /// </summary>
        protected Dictionary<string, RuntimeRoute> _routes = new Dictionary<string, RuntimeRoute>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>
        /// The name of the resource.
        /// </value>
        public string ResourceName { get; protected set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRouterResolver"/> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        public ResourceRouterResolver(string resourceName)
        {
            this.ResourceName = resourceName;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var resolver = obj as ResourceRouterResolver;
            return resolver != null && this.ResourceName.Equals(resolver.ResourceName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.ResourceName.GetHashCode();
        }
    }
}
