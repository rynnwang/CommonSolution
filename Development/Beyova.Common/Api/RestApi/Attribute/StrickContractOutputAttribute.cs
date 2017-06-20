using System;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class StrickContractOutputAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class StrickContractOutputAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrickContractOutputAttribute"/> class.
        /// </summary>
        public StrickContractOutputAttribute()
            : base()
        {
        }
    }
}