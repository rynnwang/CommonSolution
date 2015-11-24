using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ifunction
{
    /// <summary>
    /// Interface IJsonSerializable
    /// </summary>
    public interface IJsonSerializable
    {
        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        void FillPropertyValuesByJToken(JToken jToken);
    }
}
