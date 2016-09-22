using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    public class ElasticCriteriaObjectSerializer : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ElasticCriteriaObject);
        }

        /// <summary>
        /// Reads the json.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>System.Object.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var property = jsonObject.Properties().FirstOrDefault();
            var criteriaItem = property?.Value?.Value<KeyValuePair<string, object>>();

            return property != null ? new ElasticCriteriaObject
            {
                CriteriaType = property.Name,
                FieldName = criteriaItem.HasValue ? criteriaItem.Value.Key : null,
                CriteriaValue = property.Value.HasValues ? criteriaItem.Value.Value : null
            } : null;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var criteriaObject = value as ElasticCriteriaObject;
            writer.WriteStartObject();
            writer.WritePropertyName(criteriaObject.CriteriaType);

            writer.WriteStartObject();
            writer.WritePropertyName(criteriaObject.FieldName);

            serializer.Serialize(writer, criteriaObject.CriteriaValue);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
