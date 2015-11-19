using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        public BeyovaComponentAttribute(string id, string version)
        {
            this.Version = version;
            this.Id = id;
        }
    }
}
