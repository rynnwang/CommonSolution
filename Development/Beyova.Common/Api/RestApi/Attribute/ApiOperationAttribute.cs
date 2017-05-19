using System;

namespace Beyova.RestApi
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
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data sensitive. When it is set as true, EventLog would not log inbound/outbound body.
        /// </summary>
        /// <value><c>true</c> if this instance is data sensitive; otherwise, <c>false</c>.</value>
        public bool IsDataSensitive { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiOperationAttribute" /> class.
        /// </summary>
        /// <param name="resourceName">The type.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="action">The action.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="isDataSensitive">if set to <c>true</c> [is data sensitive].</param>
        public ApiOperationAttribute(string resourceName, string httpMethod, string action = null, string contentType = null, bool isDataSensitive = false)
        {
            this.ResourceName = resourceName;
            this.HttpMethod = httpMethod;
            this.Action = action;
            this.ContentType = contentType.SafeToString(HttpConstants.ContentType.Json);
            this.IsDataSensitive = isDataSensitive;
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
