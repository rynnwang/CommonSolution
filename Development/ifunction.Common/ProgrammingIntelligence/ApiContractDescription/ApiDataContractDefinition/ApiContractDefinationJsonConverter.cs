using System;
using ifunction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContractDefinitionJsonConverter.
    /// </summary>
    public class ApiContractDefinitionJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType != null && objectType.InheritsFrom(typeof(AbstractApiContractDescription));
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
            if (jsonObject != null)
            {
                var type = jsonObject.GetProperty("Type").Value<ApiContractType>();

                switch (type)
                {
                    case ApiContractType.ApiContract:
                        return jsonObject.Value<ApiContractDefinition>();
                    case ApiContractType.ApiOperation:
                        return jsonObject.Value<ApiOperationDefinition>();
                    case ApiContractType.DataContract:
                        var dataType = jsonObject.GetProperty("DataType").Value<ApiContractDataType>();
                        switch (dataType)
                        {
                            case ApiContractDataType.Array:
                                return jsonObject.Value<ArrayListDataContractDefinition>();
                            case ApiContractDataType.Dictionary:
                                return jsonObject.Value<DictionaryDataContractDefinition>();
                            case ApiContractDataType.Enum:
                                return jsonObject.Value<EnumDataContractDefinition>();
                            //case ApiContractDataType.SimpleValueType:
                            //    return jsonObject.Value<SimpleValueTypeDataContractDefinition>();
                            case ApiContractDataType.ComplexObject:
                                return jsonObject.Value<ObjectDataContractDefinition>();
                            case ApiContractDataType.AnyJson:
                                return jsonObject.Value<JsonDataContractDefinition>();
                            default:
                                break;
                        }
                        return null;
                    default:
                        break;
                }
            }

            return null;
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
