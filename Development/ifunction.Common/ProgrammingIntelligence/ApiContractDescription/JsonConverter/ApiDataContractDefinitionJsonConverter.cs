using System;
using ifunction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiDataContractDefinitionJsonConverter.
    /// </summary>
    public class ApiDataContractDefinitionJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType != null && objectType.InheritsFrom(typeof(ApiDataContractDefinition));
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
                return ReadApiContractDefinition(jsonObject);
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
            var jsonSerializable = value as IJsonSerializable;

            if (jsonSerializable != null)
            {
                jsonSerializable.WriteJson(writer, value, serializer);
            }
            else
            {
                writer.WriteStartObject();

                foreach (var one in value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance))
                {
                    writer.WritePropertyName(one.Name);
                    serializer.Serialize(writer, one.GetValue(value));
                }

                writer.WriteEndObject();
            }
        }

        /// <summary>
        /// Reads the API contract definition.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns>IApiContractDescription.</returns>
        protected ApiDataContractDefinition ReadApiContractDefinition(JObject jsonObject)
        {
            ApiDataContractDefinition result = null;

            var dataType = jsonObject.GetProperty("DataType").Value<ApiContractDataType>();
            var dataContractDefinition = CreateApiDataContractDefinitionInstance(dataType);
            result = dataContractDefinition;

            result?.FillPropertyValuesByJToken(jsonObject);
            return result;
        }

        /// <summary>
        /// Creates the API data contract definition instance.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiDataContractDefinition.</returns>
        protected ApiDataContractDefinition CreateApiDataContractDefinitionInstance(ApiContractDataType dataType)
        {
            ApiDataContractDefinition result = null;

            try
            {
                switch (dataType)
                {
                    case ApiContractDataType.Array:
                        result = new ArrayListDataContractDefinition();
                        break;
                    case ApiContractDataType.Enum:
                        result = new EnumDataContractDefinition();
                        break;
                    case ApiContractDataType.ComplexObject:
                        result = new ComplexObjectDataContractDefinition();
                        break;
                    case ApiContractDataType.DynamicObject:
                        result = DynamicObjectDataContractDefinition.GetDynamicObjectDataContractDefinition();
                        break;
                    case ApiContractDataType.Dictionary:
                        result = new DictionaryDataContractDefinition();
                        break;
                    case ApiContractDataType.Binary:
                    case ApiContractDataType.Boolean:
                    case ApiContractDataType.DateTime:
                    case ApiContractDataType.Decimal:
                    case ApiContractDataType.Single:
                    case ApiContractDataType.Guid:
                    case ApiContractDataType.Integer:
                    case ApiContractDataType.String:
                    case ApiContractDataType.TimeSpan:
                    case ApiContractDataType.Uri:
                        result = ApiContract.GetSimpleValueTypeDataContractDefinition(dataType);
                        break;
                    case ApiContractDataType.Undefined:
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateApiDataContractDefinitionInstance", new { dataType });
            }

            return result;
        }
    }
}
