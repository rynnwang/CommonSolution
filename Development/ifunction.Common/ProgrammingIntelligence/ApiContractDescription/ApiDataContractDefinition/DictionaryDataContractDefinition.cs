using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class DictionaryDataContractDefinition.
    /// </summary>
    public class DictionaryDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public ApiDataContractDefinition KeyType { get; set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ApiDataContractDefinition ValueType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryDataContractDefinition" /> class.
        /// </summary>
        public DictionaryDataContractDefinition() : base(ApiContractDataType.Dictionary, true, true)
        {
        }

        /// <summary>
        /// Writes the customized json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        protected override void WriteCustomizedJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DictionaryDataContractDefinition definition = value as DictionaryDataContractDefinition;

            if (definition != null)
            {
                writer.WritePropertyName("KeyType");
                serializer.Serialize(writer, definition.KeyType);

                writer.WritePropertyName("ValueType");
                serializer.Serialize(writer, definition.ValueType);
            }
        }

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        public override void FillPropertyValuesByJToken(JToken jToken)
        {
            base.FillPropertyValuesByJToken(jToken);

            this.KeyType = jToken.Value<ApiDataContractDefinition>("KeyType");
            this.ValueType = jToken.Value<ApiDataContractDefinition>("ValueType");
        }
    }
}
