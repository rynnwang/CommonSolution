using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityProtocolAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class GravityProtocolAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sealed.
        /// </summary>
        /// <value><c>true</c> if this instance is sealed; otherwise, <c>false</c>.</value>
        public bool IsSealed { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityProtocolAttribute" /> class.
        /// </summary>
        /// <param name="configurationPath">The configuration path.</param>
        /// <param name="isSealed">if set to <c>true</c> [is sealed].</param>
        public GravityProtocolAttribute(string configurationPath = null, bool isSealed = false)
            : this(GravityEntryFile.Load(configurationPath), isSealed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityProtocolAttribute" /> class.
        /// </summary>
        /// <param name="serviceUri">The service URI.</param>
        /// <param name="memberIdentifiableKey">The member identifiable key.</param>
        /// <param name="isSealed">if set to <c>true</c> [is sealed].</param>
        public GravityProtocolAttribute(Uri serviceUri, string memberIdentifiableKey, bool isSealed = false)
            : this(new GravityEntryObject
            {
                GravityServiceUri = serviceUri,
                MemberIdentifiableKey = memberIdentifiableKey
            }, isSealed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityProtocolAttribute"/> class.
        /// </summary>
        /// <param name="entryObject">The entry object.</param>
        /// <param name="isSealed">if set to <c>true</c> [is sealed].</param>
        protected internal GravityProtocolAttribute(GravityEntryObject entryObject, bool isSealed)
        {
            this.Entry = entryObject;
            this.IsSealed = isSealed;
        }
    }
}