using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiOperationDefinition.
    /// </summary>
    public class ApiOperationDefinition : AbstractApiContractDescription
    {
        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public Dictionary<string, ApiDataContractDefinition> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>The return value.</value>
        public ApiDataContractDefinition ReturnValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [token required].
        /// </summary>
        /// <value><c>true</c> if [token required]; otherwise, <c>false</c>.</value>
        public bool? TokenRequired { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public List<string> Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is obsoleted.
        /// </summary>
        /// <value><c>true</c> if this instance is obsoleted; otherwise, <c>false</c>.</value>
        public bool IsObsoleted { get; set; }

        /// <summary>
        /// Gets or sets the obsolete description.
        /// </summary>
        /// <value>The obsolete description.</value>
        public string ObsoleteDescription { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiOperationDefinition"/> class.
        /// </summary>
        public ApiOperationDefinition()
            : base(ApiContractType.ApiOperation)
        {
        }
    }
}
