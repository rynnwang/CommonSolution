using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Class RelationshipOperatorComputable.
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
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        public bool Compute(JObject json)
        {
            return BooleanSearchCore.BooleanCompute(this, json);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return (Item1 != null && Item2 != null) ? string.Format(BooleanSearchCore.expressionFormat, Item1.ToString(), Operator.ToString(), Item2.ToString()) : string.Empty;
        }

        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>IBooleanComputable.</returns>
        public static IBooleanComputable Parse(string expression)
        {
            var reader = new BooleanSearchExpressionReader();
            return reader.ReadAsObject(expression);
        }
    }
}