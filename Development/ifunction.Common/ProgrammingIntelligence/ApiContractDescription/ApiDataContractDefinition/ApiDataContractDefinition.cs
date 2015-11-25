using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiDataContractDefinition.
    /// </summary>
    public abstract class ApiDataContractDefinition : AbstractApiContractDescription
    {
        /// <summary>
        /// The _namespace
        /// </summary>
        protected string _namespace;

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public override string Namespace
        {
            get
            {
                return this.OmitNamespace ? null : _namespace.SafeToString();
            }

            set
            {
                this._namespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public ApiContractDataType DataType { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [display as reference].
        /// </summary>
        /// <value><c>true</c> if [display as reference]; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool DisplayAsReference { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [omit namespace].
        /// </summary>
        /// <value><c>true</c> if [omit namespace]; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool OmitNamespace { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDataContractDefinition" /> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="displayAsReference">if set to <c>true</c> [display as reference].</param>
        protected ApiDataContractDefinition(ApiContractDataType dataType, bool displayAsReference, bool omitNamespace)
            : base(ApiContractType.DataContract)
        {
            this.DataType = dataType;
            this.DisplayAsReference = displayAsReference;
            this.OmitNamespace = omitNamespace;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return OmitNamespace ? this.DataType.ToString() : string.Format("{0}.{1}", this.Namespace, this.Name);
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
        }
    }
}
