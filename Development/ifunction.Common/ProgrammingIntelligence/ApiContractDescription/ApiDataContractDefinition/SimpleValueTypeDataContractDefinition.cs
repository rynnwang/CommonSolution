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
    /// Class SimpleValueTypeDataContractDefinition.
    /// </summary>
    public class SimpleValueTypeDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleValueTypeDataContractDefinition" /> class.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        protected SimpleValueTypeDataContractDefinition(ApiContractDataType contractType)
            : base(contractType, false, true)
        {
            this.Name = contractType.ToString();
        }

        /// <summary>
        /// Gets the string data contract definition.
        /// </summary>
        /// <value>The string data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition StringDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.String); } }

        /// <summary>
        /// Gets the integer data contract definition.
        /// </summary>
        /// <value>The integer data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition IntegerDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Integer); } }

        /// <summary>
        /// Gets the float data contract definition.
        /// </summary>
        /// <value>The float data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition FloatDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Single); } }

        /// <summary>
        /// Gets the unique identifier data contract definition.
        /// </summary>
        /// <value>The unique identifier data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition GuidDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Guid); } }

        /// <summary>
        /// Gets the URI data contract definition.
        /// </summary>
        /// <value>The URI data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition UriDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Uri); } }

        /// <summary>
        /// Gets the date time data contract definition.
        /// </summary>
        /// <value>The date time data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition DateTimeDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.DateTime); } }

        /// <summary>
        /// Gets the boolean data contract definition.
        /// </summary>
        /// <value>The boolean data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition BooleanDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Boolean); } }

        /// <summary>
        /// Gets the decimal data contract definition.
        /// </summary>
        /// <value>The decimal data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition DecimalDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Decimal); } }

        /// <summary>
        /// Gets the time span data contract definition.
        /// </summary>
        /// <value>The time span data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition TimeSpanDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.TimeSpan); } }

        /// <summary>
        /// Gets the binary data contract definition.
        /// </summary>
        /// <value>The binary data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition BinaryDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Binary); } }

        ///// <summary>
        ///// Writes the json.
        ///// </summary>
        ///// <param name="writer">The writer.</param>
        ///// <param name="value">The value.</param>
        ///// <param name="serializer">The serializer.</param>
        //public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //{
        //    var definition = value as SimpleValueTypeDataContractDefinition;

        //    if (definition != null)
        //    {
        //        serializer.Serialize(writer, definition.DataType.ToString());
        //    }
        //}

        ///// <summary>
        ///// Fills the property values by JToken.
        ///// </summary>
        ///// <param name="jToken">The j token.</param>
        //public override void FillPropertyValuesByJToken(JToken jToken)
        //{
        //    ApiContractDataType type;

        //    if (Enum.TryParse<ApiContractDataType>(jToken.Value<string>("DataType"), out type))
        //    {
        //        this.DataType = type;
        //    }
        //}

        /// <summary>
        /// Writes the customized json.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        protected override void WriteCustomizedJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //Do nothing
        }
    }
}
