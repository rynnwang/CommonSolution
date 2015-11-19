using System;
using System.Reflection;
using Beyova.ProgrammingIntelligence;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class ApiMethod.
    /// </summary>
    public class ApiMethod : IApiMethod
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the method information.
        /// </summary>
        /// <value>The method information.</value>
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [token required].
        /// </summary>
        /// <value><c>null</c> if [token required] contains no value, <c>true</c> if [token required]; otherwise, <c>false</c>.</value>
        public bool? TokenRequired { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("/{0}/{1}/{2}/{3}/", HttpMethod, Version, ResourceName, Action);
        }

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public object Invoke(params object[] parameters)
        {
            return MethodInfo != null ? MethodInfo.Invoke(Instance, parameters) : null;
        }
    }
}
