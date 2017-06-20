using System;

namespace Beyova
{
    /// <summary>
    /// Interface ITransactionRequest
    /// </summary>
    public interface ITransactionRequest
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
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>
        /// The owner key.
        /// </value>
        Guid? OwnerKey { get; set; }
    }
}