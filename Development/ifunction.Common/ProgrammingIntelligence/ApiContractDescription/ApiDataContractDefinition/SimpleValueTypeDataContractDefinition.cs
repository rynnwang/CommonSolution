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
    public class SimpleValueTypeDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleValueTypeDataContractDefinition"/> class.
        /// </summary>
        public SimpleValueTypeDataContractDefinition(ApiContractDataType contractType)
            : base(contractType)
        {
        }
    }
}
