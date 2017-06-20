using System;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiModuleAttribute. This attribute is for indicating which module current interface are in, which is specified in Api Event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ApiModuleAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// <remarks>
        /// This field is for tracking only.</remarks>
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiModuleAttribute" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        public ApiModuleAttribute(string moduleName)
        {
            this.ModuleName = moduleName;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.ModuleName.SafeToString();
        }
    }
}