using System;

namespace Beyova
{
    /// <summary>
    /// Interface ITransactionCriteria
    /// </summary>
    public interface ITransactionCriteria : ICriteria
    {
        /// <summary>
        /// Gets or sets the related party identifier.
        /// </summary>
        /// <value>The related party identifier.</value>
        string RelatedPartyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the party transaction identifier.
        /// </summary>
        /// <value>The party transaction identifier.</value>
        string PartyTransactionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        string Currency { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        decimal? Amount { get; set; }

        /// <summary>
        /// Gets or sets the state of the transaction.
        /// </summary>
        /// <value>The state of the transaction.</value>
        TransactionState? TransactionState { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        DateTime?  FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [order descending].
        /// </summary>
        /// <value><c>true</c> if [order descending]; otherwise, <c>false</c>.</value>
        bool OrderDescending { get; set; }
    }
}
