using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Beyova.BooleanSearch.IRelationshipOperatorComputable" />
    public class RelationshipOperatorComputable : IRelationshipOperatorComputable
    {
        /// <summary>
        /// Gets or sets the item1.
        /// </summary>
        /// <value>
        /// The item1.
        /// </value>
        public IBooleanComputable Item1 { get; set; }

        /// <summary>
        /// Gets or sets the item2.
        /// </summary>
        /// <value>
        /// The item2.
        /// </value>
        public IBooleanComputable Item2 { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public RelationshipOperator Operator { get; set; }

        /// <summary>
        /// Computes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public bool Compute(JObject json)
        {
            return BooleanSearchCore.BooleanCompute(this, json);
        }
    }
}
