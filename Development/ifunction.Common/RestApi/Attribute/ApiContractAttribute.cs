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
        /// Gets or sets the name.
        /// This name is used for as service identifier in <see cref="ifunction.ApiTracking.Model.ApiEventLog"/> model.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContractAttribute" /> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        public ApiContractAttribute(string version, string name = null)
        {
            this.Version = version;
            this.Name = name;
        }
    }
}
