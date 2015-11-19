using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContractDefinition.
    /// </summary>
    public class ApiContractDefinition : AbstractApiContractDescription
    {
        /// <summary>
        /// Gets or sets a value indicating whether [token required].
        /// </summary>
        /// <value><c>null</c> if [token required] contains no value, <c>true</c> if [token required]; otherwise, <c>false</c>.</value>
        public bool? TokenRequired { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the API data contracts.
        /// </summary>
        /// <value>The API data contracts.</value>
        public List<ApiDataContractDefinition> ApiDataContracts { get; set; }

        /// <summary>
        /// Gets or sets the API operations.
        /// </summary>
        /// <value>The API operations.</value>
        public List<ApiOperationDefinition> ApiOperations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContractDefinition"/> class.
        /// </summary>
        public ApiContractDefinition()
            : base(ApiContractType.ApiContract)
        {
            this.ApiOperations = new List<ApiOperationDefinition>();
            this.ApiDataContracts = new List<ApiDataContractDefinition>();
        }
    }
}
