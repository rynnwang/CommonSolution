using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class SimpleValueTypeDataContractDefinition.
    /// </summary>
    public class SimpleValueTypeDataContractDefinition : ApiDataContractDefinition, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleValueTypeDataContractDefinition" /> class.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        internal SimpleValueTypeDataContractDefinition(ApiContractDataType contractType)
            : base(contractType)
        {
        }

        /// <summary>
        /// Gets the string data contract definition.
        /// </summary>
        /// <value>The string data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition StringDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.String) { IsNullable = true }; } }

        /// <summary>
        /// Gets the integer data contract definition.
        /// </summary>
        /// <value>The integer data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition IntegerDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Integer); } }

        /// <summary>
        /// Gets the float data contract definition.
        /// </summary>
        /// <value>The float data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition FloatDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Float) { IsNullable = true }; } }

        /// <summary>
        /// Gets the unique identifier data contract definition.
        /// </summary>
        /// <value>The unique identifier data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition GuidDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Guid) { IsNullable = true }; } }

        /// <summary>
        /// Gets the URI data contract definition.
        /// </summary>
        /// <value>The URI data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition UriDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Uri) { IsNullable = true }; } }

        /// <summary>
        /// Gets the date time data contract definition.
        /// </summary>
        /// <value>The date time data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition DateTimeDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.DateTime) { IsNullable = true }; } }

        /// <summary>
        /// Gets the boolean data contract definition.
        /// </summary>
        /// <value>The boolean data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition BooleanDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Boolean) { IsNullable = true }; } }

        /// <summary>
        /// Gets the decimal data contract definition.
        /// </summary>
        /// <value>The decimal data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition DecimalDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Decimal) { IsNullable = true }; } }

        /// <summary>
        /// Gets the time span data contract definition.
        /// </summary>
        /// <value>The time span data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition TimeSpanDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.TimeSpan) { IsNullable = true }; } }

        /// <summary>
        /// Gets the binary data contract definition.
        /// </summary>
        /// <value>The binary data contract definition.</value>
        public static SimpleValueTypeDataContractDefinition BinaryDataContractDefinition { get { return new SimpleValueTypeDataContractDefinition(ApiContractDataType.Binary); } }


        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            return new SimpleValueTypeDataContractDefinition(this.DataType)
            {
                IsObsoleted = this.IsObsoleted,
                ObsoleteDescription = this.ObsoleteDescription,
                Name = this.Name,
                UniqueName = this.UniqueName,
                Namespace = this.Namespace,
                Type = this.Type,
                IsNullable = this.IsNullable
            };
        }
    }
}
