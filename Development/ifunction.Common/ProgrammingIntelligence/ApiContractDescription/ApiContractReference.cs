using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.RestApi;
using Newtonsoft.Json;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContractReference.
    /// </summary>
    [JsonConverter(typeof(ApiContractReferenceJsonConverter))]
    public class ApiContractReference
    {
        /// <summary>
        /// Gets or sets the name of the reference.
        /// </summary>
        /// <value>The name of the reference.</value>
        public string ReferenceName { get; set; }

        /// <summary>
        /// Gets the reference instance.
        /// </summary>
        /// <value>The reference instance.</value>
        public ApiDataContractDefinition ReferenceInstance
        {
            get
            {
                return ApiContract.GetApiDataContractDefinition(ReferenceName);
            }
        }
    }
}
