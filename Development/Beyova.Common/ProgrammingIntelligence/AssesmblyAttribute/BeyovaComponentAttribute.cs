using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class BeyovaComponentAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class BeyovaComponentAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; protected set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the API tracking.
        /// </summary>
        /// <value>The type of the API tracking.</value>
        public Type ApiTrackingType { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="apiTrackingType">Type of the API tracking, which should implement <see cref="IApiTracking"/> with 0 parameter constructor.</param>
        public BeyovaComponentAttribute(string id, string version, Type apiTrackingType = null)
        {
            this.Version = version;
            this.Id = id;
            this.ApiTrackingType = apiTrackingType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute"/> class.
        /// </summary>
        /// <param name="apiTrackingType">Type of the API tracking.</param>
        public BeyovaComponentAttribute(Type apiTrackingType) : this(null, null, apiTrackingType)
        {

        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}: {1}", Id.SafeToString("Not specified"), Version);
        }

        /// <summary>
        /// Gets the API tracking instance.
        /// </summary>
        /// <returns>IApiTracking.</returns>
        internal IApiTracking GetApiTrackingInstance()
        {
            if (this.ApiTrackingType != null)
            {
                try
                {
                    return Activator.CreateInstance(this.ApiTrackingType) as IApiTracking;
                }
                catch { }
            }

            return null;
        }
    }
}
