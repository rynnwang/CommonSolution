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
            writer.WriteStartObject();

            foreach (var one in value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance))
            {
                writer.WritePropertyName(one.Name);
                serializer.Serialize(writer, one.GetValue(value));
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads the API contract definition.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="jsonObject">The json object.</param>
        /// <returns>IApiContractDescription.</returns>
        protected IApiContractDescription ReadApiContractDefinition(ApiContractType contractType, JObject jsonObject)
        {
            IApiContractDescription result = null;

            switch (contractType)
            {
                case ApiContractType.ApiContract:
                    result = new ApiContractDefinition();
                    break;
                case ApiContractType.ApiOperation:
                    result = new ApiOperationDefinition();
                    break;
                case ApiContractType.DataContract:
                    var dataType = jsonObject.GetProperty("DataType").Value<ApiContractDataType>();
                    var uniqueName = jsonObject.GetProperty("UniqueName").Value<string>();
                    var dataContractDefinition = CreateApiDataContractDefinitionInstance(dataType);
                    dataContractDefinition.UniqueName = uniqueName;
                    result = dataContractDefinition;
                    break;
                default:
                    break;
            }

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
                        result = new DynamicObjectDataContractDefinition();
                        break;
                    case ApiContractDataType.Dictionary:
                        result = new DictionaryDataContractDefinition();
                        break;
                    case ApiContractDataType.Binary:
                    case ApiContractDataType.Boolean:
                    case ApiContractDataType.DateTime:
                    case ApiContractDataType.Decimal:
                    case ApiContractDataType.Float:
                    case ApiContractDataType.Guid:
                    case ApiContractDataType.Integer:
                    case ApiContractDataType.String:
                    case ApiContractDataType.TimeSpan:
                    case ApiContractDataType.Uri:
                        result = new SimpleValueTypeDataContractDefinition(dataType);
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
