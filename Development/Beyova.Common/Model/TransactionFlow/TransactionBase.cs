namespace Beyova
{
    /// <summary>
    /// Class TransactionBase.
    /// </summary>
    public class TransactionBase
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the party transaction identifier.
        /// </summary>
        /// <value>
        /// The party transaction identifier.
        /// </value>
        public string PartyTransactionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the related party identifier.
        /// </summary>
        /// <value>
        /// The related party identifier.
        /// </value>
        public string RelatedPartyIdentifier { get; set; }
    }
}