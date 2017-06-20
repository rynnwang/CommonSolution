namespace Beyova
{
    /// <summary>
    /// Interface ITransactionAudit
    /// </summary>
    public interface ITransactionAudit : ITransactionRequest, ISimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the state of the transaction.
        /// </summary>
        /// <value>The state of the transaction.</value>
        TransactionState TransactionState { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>The balance.</value>
        decimal? Balance { get; set; }
    }
}