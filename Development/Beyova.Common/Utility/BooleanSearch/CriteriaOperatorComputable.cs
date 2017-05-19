using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Class CriteriaOperatorComputable.
    /// </summary>
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
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        public bool Compute(JObject json)
        {
            return BooleanSearchCore.BooleanCompute(this, json);
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if validation passed, <c>false</c> otherwise.</returns>
        public bool Validate()
        {
            return BooleanSearchCore.IsKeyValid(this);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return (!string.IsNullOrWhiteSpace(Item1) && !string.IsNullOrWhiteSpace(Item2))
                ? string.Format(BooleanSearchCore.expressionFormat, BooleanSearchCore.StringFieldToExpressionString(Item1), Operator.ToString(), BooleanSearchCore.StringFieldToExpressionString(Item2))
                : string.Empty;
        }
    }
}
