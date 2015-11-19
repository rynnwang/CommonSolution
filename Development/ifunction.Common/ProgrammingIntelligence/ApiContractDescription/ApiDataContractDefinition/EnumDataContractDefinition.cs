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
    public class EnumDataContractDefinition : ApiDataContractDefinition
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
    }
}
