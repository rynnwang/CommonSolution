using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Beyova.BooleanSearch.ICriteriaOperatorComputable" />
    public class CriteriaOperatorComputable : ICriteriaOperatorComputable
    {
        /// <summary>
        /// Gets or sets the item1.
        /// </summary>
        /// <value>
        /// The item1.
        /// </value>
        public string Item1 { get; set; }

        /// <summary>
        /// Gets or sets the item2.
        /// </summary>
        /// <value>
        /// The item2.
        /// </value>
        public string Item2 { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public ComputeOperator Operator { get; set; }

        /// <summary>
        /// Computes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public bool Compute(JObject json)
        {
            return BooleanSearchCore.BooleanCompute(this, json);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return BooleanSearchCore.IsKeyValid(this);
        }
    }
}
