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
        public virtual string Namespace { get; set; }

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
        /// Writes the json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            AbstractApiContractDescription contractDefinition = value as AbstractApiContractDescription;

            if (contractDefinition != null)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Namespace");
                serializer.Serialize(writer, contractDefinition.Namespace);

                writer.WritePropertyName("Name");
                serializer.Serialize(writer, contractDefinition.Name);

                writer.WritePropertyName("ObsoleteDescription");
                serializer.Serialize(writer, contractDefinition.ObsoleteDescription);

                writer.WritePropertyName("Type");
                serializer.Serialize(writer, contractDefinition.Type);

                WriteCustomizedJson(writer, value, serializer);

                writer.WriteEndObject();
            }
        }

        /// <summary>
        /// Writes the customized json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        protected abstract void WriteCustomizedJson(JsonWriter writer, object value, JsonSerializer serializer);

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
                this.ObsoleteDescription = jToken.Value<string>("ObsoleteDescription");
            }
        }
    }
}
