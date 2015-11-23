using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class DynamicObjectDataContractDefinition.
    /// </summary>
    public class DynamicObjectDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value><c>true</c> if this instance is nullable; otherwise, <c>false</c>.</value>
        public override bool IsNullable
        {
            get
            {
                return true;
            }
            set
            {
                //Do nothing
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicObjectDataContractDefinition" /> class.
        /// </summary>
        public DynamicObjectDataContractDefinition()
                    : base(ApiContractDataType.DynamicObject)
        {
        }
    }
}
