using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.RestApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class AbstractApiContractDescription.
    /// </summary>
    [JsonConverter(typeof(ApiContractDefinitionJsonConverter))]
    public abstract class AbstractApiContractDescription : IApiContractDescription, IJsonSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary.
        /// </summary>
        /// <value>The name of the primary.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ApiContractType Type { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is obsoleted.
        /// </summary>
        /// <value><c>true</c> if this instance is obsoleted; otherwise, <c>false</c>.</value>
        public bool IsObsoleted { get; set; }

        /// <summary>
        /// Gets or sets the obsolete description.
        /// </summary>
        /// <value>The obsolete description.</value>
        public string ObsoleteDescription { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractApiContractDescription"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        protected AbstractApiContractDescription(ApiContractType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// To the JToken.
        /// </summary>
        /// <returns>JToken.</returns>
        public virtual JToken ToJToken()
        {
            return this.ToJson();
        }

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        public virtual void FillPropertyValuesByJToken(JToken jToken)
        {
            if (jToken != null)
            {
                this.Namespace = jToken.Value<string>("Namespace");
                this.Name = jToken.Value<string>("Name");
                this.IsObsoleted = jToken.Value<bool>("IsObsoleted");
                this.ObsoleteDescription = jToken.Value<string>("ObsoleteDescription");
            }
        }        
    }
}
