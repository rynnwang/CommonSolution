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
    /// Class ApiOperationDefinition.
    /// </summary>
    public class ApiOperationDefinition : AbstractApiContractDescription
    {
        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, ApiDataContractDefinition> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>The return value.</value>
        public ApiDataContractDefinition ReturnValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [token required].
        /// </summary>
        /// <value><c>true</c> if [token required]; otherwise, <c>false</c>.</value>
        public bool? TokenRequired { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public List<string> Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiOperationDefinition"/> class.
        /// </summary>
        public ApiOperationDefinition()
            : base()
        {
            this.Parameters = new Dictionary<string, ApiDataContractDefinition>();
        }

        /// <summary>
        /// Writes the customized json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        protected override void WriteCustomizedJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ApiOperationDefinition definition = value as ApiOperationDefinition;

            if (definition != null)
            {
                writer.WritePropertyName("TokenRequired");
                serializer.Serialize(writer, definition.TokenRequired);

                writer.WritePropertyName("Description");
                serializer.Serialize(writer, definition.Description);

                writer.WritePropertyName("ReturnValue");
                serializer.Serialize(writer, definition.ReturnValue);

                writer.WritePropertyName("Parameters");
                serializer.Serialize(writer, definition.Parameters);
            }
        }

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        public override void FillPropertyValuesByJToken(JToken jToken)
        {
            base.FillPropertyValuesByJToken(jToken);
            this.TokenRequired = jToken.Value<bool?>("TokenRequired");
            this.Description = jToken.Value<List<string>>("Description");
            this.ReturnValue = jToken.Value<ApiDataContractDefinition>("ReturnValue");
            this.Parameters = jToken.Value<Dictionary<string, ApiDataContractDefinition>>("Parameters");
        }
    }
}
