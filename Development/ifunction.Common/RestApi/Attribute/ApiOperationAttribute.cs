using System;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class ApiUriAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiOperationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName { get; protected set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; protected set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiOperationAttribute" /> class.
        /// </summary>
        /// <param name="resourceName">The type.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="action">The action.</param>
        public ApiOperationAttribute(string resourceName, string httpMethod, string action = null)
        {
            this.ResourceName = resourceName;
            this.HttpMethod = httpMethod;
            this.Action = action;
        }

        #region Hidden feature. Keep for future.

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiOperationAttribute" /> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="action">The action.</param>
        protected ApiOperationAttribute(Type resourceType, string httpMethod, string action = null)
            : this(resourceType.Name, httpMethod, action)
        {
        }

        #endregion
    }
}
