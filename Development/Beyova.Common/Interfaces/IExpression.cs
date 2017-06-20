namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TItem1">The type of the item.</typeparam>
    /// <typeparam name="TItem2">The type of the item2.</typeparam>
    /// <typeparam name="TOperator">The type of the operator.</typeparam>
    public interface IExpression<TItem1, TItem2, TOperator>
    {
        /// <summary>
        /// Gets or sets the item1.
        /// </summary>
        /// <value>
        /// The item1.
        /// </value>
        TItem1 Item1 { get; set; }

        /// <summary>
        /// Gets or sets the item2.
        /// </summary>
        /// <value>
        /// The item2.
        /// </value>
        TItem2 Item2 { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        TOperator Operator { get; set; }
    }
}