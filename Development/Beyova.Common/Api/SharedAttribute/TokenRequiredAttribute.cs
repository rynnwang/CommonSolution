using System;

namespace Beyova.Api
{
    /// <summary>
    /// Class TokenRequiredAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class TokenRequiredAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether [token required].
        /// </summary>
        /// <value><c>true</c> if [token required]; otherwise, <c>false</c>.</value>
        public bool TokenRequired { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRequiredAttribute" /> class.
        /// </summary>
        /// <param name="tokenRequired">if set to <c>true</c> [token required].</param>
        public TokenRequiredAttribute(bool tokenRequired = true)
            : base()
        {
            this.TokenRequired = tokenRequired;
        }
    }
}
