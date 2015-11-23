using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ComplexObjectDataContractDefinition.
    /// </summary>
    public class ComplexObjectDataContractDefinition : ApiDataContractDefinition, ICloneable
    {
        /// <summary>
        /// Gets or sets the inherition.
        /// </summary>
        /// <value>The inherition.</value>
        public List<ApiContractReference> Inherition { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public Dictionary<string, ApiContractReference> Children { get; set; }

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
        /// Initializes a new instance of the <see cref="ComplexObjectDataContractDefinition"/> class.
        /// </summary>
        public ComplexObjectDataContractDefinition()
            : base(ApiContractDataType.ComplexObject)
        {
            this.Children = new Dictionary<string, ApiContractReference>();
            this.Inherition = new List<ApiContractReference>();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            return new ComplexObjectDataContractDefinition
            {
                IsObsoleted = this.IsObsoleted,
                ObsoleteDescription = this.ObsoleteDescription,
                Name = this.Name,
                Namespace = this.Namespace,
                Inherition = this.Inherition,
                Children = this.Children,
                IsNullable = this.IsNullable
            };
        }
    }
}
