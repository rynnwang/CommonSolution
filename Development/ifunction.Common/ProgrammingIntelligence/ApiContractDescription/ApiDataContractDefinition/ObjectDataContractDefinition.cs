using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ObjectDataContractDefinition.
    /// </summary>
    public class ObjectDataContractDefinition : ApiDataContractDefinition
    {
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public Dictionary<string, ApiDataContractDefinition> Children { get; set; }

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
        /// Initializes a new instance of the <see cref="ObjectDataContractDefinition"/> class.
        /// </summary>
        public ObjectDataContractDefinition()
            : base(ApiContractDataType.ComplexObject)
        {
            this.Children = new Dictionary<string, ApiDataContractDefinition>();
        }
    }
}
