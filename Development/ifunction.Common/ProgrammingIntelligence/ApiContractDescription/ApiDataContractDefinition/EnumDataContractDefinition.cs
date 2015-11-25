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
    /// Class EnumDataContractDefinition.
    /// </summary>
    public class EnumDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Gets or sets the enum values.
        /// </summary>
        /// <value>The enum values.</value>
        public Dictionary<long, string> EnumValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataContractDefinition"/> class.
        /// </summary>
        public EnumDataContractDefinition()
            : base(ApiContractDataType.Enum, true, false)
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
            EnumDataContractDefinition definition = value as EnumDataContractDefinition;

            if (definition != null)
            {
                writer.WritePropertyName("EnumValues");
                serializer.Serialize(writer, definition.EnumValues);
            }
        }

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        public override void FillPropertyValuesByJToken(JToken jToken)
        {
            base.FillPropertyValuesByJToken(jToken);
            this.EnumValues = jToken.Value<Dictionary<long, string>>("EnumValues");
        }
    }
}
