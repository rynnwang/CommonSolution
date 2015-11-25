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
    /// Class DynamicObjectDataContractDefinition.
    /// </summary>
    public class DynamicObjectDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicObjectDataContractDefinition" /> class.
        /// </summary>
        protected DynamicObjectDataContractDefinition()
                    : base(ApiContractDataType.DynamicObject, false, true)
        {
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static readonly DynamicObjectDataContractDefinition instance = new DynamicObjectDataContractDefinition();

        /// <summary>
        /// Gets the dynamic object data contract definition.
        /// </summary>
        /// <returns>DynamicObjectDataContractDefinition.</returns>
        public static DynamicObjectDataContractDefinition GetDynamicObjectDataContractDefinition() { return instance; }

        /// <summary>
        /// Writes the customized json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        protected override void WriteCustomizedJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //Do nothing.
        }
    }
}
