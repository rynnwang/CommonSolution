using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiDataContractDefinition.
    /// </summary>
    public abstract class ApiDataContractDefinition : AbstractApiContractDescription,ICloneable
    {
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public ApiContractDataType DataType { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value><c>true</c> if this instance is nullable; otherwise, <c>false</c>.</value>
        public virtual bool IsNullable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDataContractDefinition"/> class.
        /// </summary>
        protected ApiDataContractDefinition(ApiContractDataType dataType)
            : base(ApiContractType.DataContract)
        {
            this.DataType = dataType;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", this.Namespace, this.Name);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        public override void FillPropertyValuesByJToken(JToken jToken)
        {
            base.FillPropertyValuesByJToken(jToken);
            this.IsNullable = jToken.Value<bool>("IsNullable");
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public abstract object Clone();
    }
}
