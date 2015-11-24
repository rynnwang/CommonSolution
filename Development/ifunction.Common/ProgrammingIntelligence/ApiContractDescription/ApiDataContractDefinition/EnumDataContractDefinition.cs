using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class EnumDataContractDefinition.
    /// </summary>
    public class EnumDataContractDefinition : ApiDataContractDefinition, ICloneable
    {
        /// <summary>
        /// Gets or sets the enum values.
        /// </summary>
        /// <value>The enum values.</value>
        public Dictionary<long, string> EnumValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataContractDefinition"/> class.
        /// </summary>
        public EnumDataContractDefinition()
            : base(ApiContractDataType.Enum)
        {
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            return new EnumDataContractDefinition
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
