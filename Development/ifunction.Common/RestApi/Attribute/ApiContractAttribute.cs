using System;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class ApiContractAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiContractAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContractAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public ApiContractAttribute(string version)
        {
            this.Version = version;
        }
    }
}
