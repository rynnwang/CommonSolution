using System;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class ApiDescriptionAttribute. This attribute is used for generating API documentation only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class ApiDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDescriptionAttribute" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public ApiDescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}
