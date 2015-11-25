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
    /// Class ArrayListDataContractDefinition.
    /// </summary>
    public class ArrayListDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ApiContractReference ValueType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayListDataContractDefinition"/> class.
        /// </summary>
        public ArrayListDataContractDefinition()
            : base(ApiContractDataType.Array, false, true)
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
            ArrayListDataContractDefinition definition = value as ArrayListDataContractDefinition;

            if (definition != null)
            {
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
            this.ValueType = jToken.Value<ApiContractReference>("ValueType");
        }
    }
}
