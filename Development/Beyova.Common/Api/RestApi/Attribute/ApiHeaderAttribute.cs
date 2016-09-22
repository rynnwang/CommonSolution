using System;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiHeaderAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class ApiHeaderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the header key.
        /// </summary>
        /// <value>The header key.</value>
        public string HeaderKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiHeaderAttribute"/> class.
        /// </summary>
        /// <param name="headerKey">The header key.</param>
        public ApiHeaderAttribute(string headerKey)
        {
            this.HeaderKey = headerKey;
        }
    }
}
