using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using Newtonsoft.Json;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Enum ApiContractType
    /// </summary>
    [JsonConverter(typeof(ApiContractType))]
    public enum ApiContractType
    {
        /// <summary>
        /// Value indicating it is undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating it is API contract
        /// </summary>
        ApiContract = 1,
        /// <summary>
        /// Value indicating it is API operation
        /// </summary>
        ApiOperation = 2,
        /// <summary>
        /// Value indicating it is data contract
        /// </summary>
        DataContract = 3,
    }
}
