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
    /// Class ComplexObjectDataContractDefinition.
    /// </summary>
    public class ComplexObjectDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Gets or sets the inherition.
        /// </summary>
        /// <value>The inherition.</value>
        public ComplexObjectDataContractDefinition Inherition { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public Dictionary<string, ApiContractReference> Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexObjectDataContractDefinition"/> class.
        /// </summary>
        public ComplexObjectDataContractDefinition()
            : base(ApiContractDataType.ComplexObject, true, false)
        {
            this.Children = new Dictionary<string, ApiContractReference>();
        }

        /// <summary>
        /// Writes the customized json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        protected override void WriteCustomizedJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ComplexObjectDataContractDefinition definition = value as ComplexObjectDataContractDefinition;

            if (definition != null)
            {
                writer.WritePropertyName("Children");
                serializer.Serialize(writer, definition.Children);

                writer.WritePropertyName("Inherition");
                serializer.Serialize(writer, definition.Inherition);
            }
        }

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        public override void FillPropertyValuesByJToken(JToken jToken)
        {
            base.FillPropertyValuesByJToken(jToken);

            this.Children = jToken.Value<Dictionary<string, ApiContractReference>>("Children");
            this.Inherition = jToken.Value<ComplexObjectDataContractDefinition>("Inherition");
        }
    }
}
