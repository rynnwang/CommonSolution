using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.ProgrammingIntelligence;

namespace ifunction.Common.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContractProperty.
    /// </summary>
    public class ApiContractProperty
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the API data contract definition.
        /// </summary>
        /// <value>The API data contract definition.</value>
        public ApiDataContractDefinition ApiDataContractDefinition { get; set; }
    }
}
