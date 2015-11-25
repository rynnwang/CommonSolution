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
    public class DynamicObjectDataContractDefinition : ApiDataContractDefinition, ICloneable
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

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            return new DynamicObjectDataContractDefinition
            {
                IsObsoleted = this.IsObsoleted,
                ObsoleteDescription = this.ObsoleteDescription,
                Name = this.Name,
                UniqueName = this.UniqueName,
                Namespace = this.Namespace,
                IsNullable = this.IsNullable
            };
        }
    }
}
