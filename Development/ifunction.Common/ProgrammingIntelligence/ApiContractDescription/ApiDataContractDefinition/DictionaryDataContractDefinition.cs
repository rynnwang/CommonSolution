using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class DictionaryDataContractDefinition.
    /// </summary>
    public class DictionaryDataContractDefinition : ApiDataContractDefinition, ICloneable
    {
        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public ApiContractReference KeyType { get; set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public ApiContractReference ValueType { get; set; }

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
        /// Initializes a new instance of the <see cref="DictionaryDataContractDefinition" /> class.
        /// </summary>
        public DictionaryDataContractDefinition()
                    : base(ApiContractDataType.Dictionary)
        {
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public override object Clone()
        {
            return new DictionaryDataContractDefinition
            {
                IsObsoleted = this.IsObsoleted,
                ObsoleteDescription = this.ObsoleteDescription,
                Name = this.Name,
                Namespace = this.Namespace,
                KeyType = this.KeyType,
                ValueType = this.ValueType,
                IsNullable = this.IsNullable
            };
        }
    }
}
