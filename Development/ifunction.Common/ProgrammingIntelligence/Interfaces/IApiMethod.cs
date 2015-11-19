using System.Reflection;

namespace ifunction.RestApi
{
    /// <summary>
    /// Interface IApiMethod
    /// </summary>
    public interface IApiMethod
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>The HTTP method.</value>
        string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [token required].
        /// </summary>
        /// <value><c>null</c> if [token required] contains no value, <c>true</c> if [token required]; otherwise, <c>false</c>.</value>
        bool? TokenRequired { get; set; }
    }
}
