using System;

namespace Beyova
{
    /// <summary>
    /// Class TransactionAudit.
    /// </summary>
    public class TransactionAudit : TransactionBase, ITransactionAudit, IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>
        /// The last updated stamp.
        /// </value>
        public DateTime? LastUpdatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>
        /// The owner key.
        /// </value>
        public Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public ObjectState State { get; set; }

        /// <summary>
        /// Gets or sets the state of the transaction.
        /// </summary>
        /// <value>
        /// The state of the transaction.
        /// </value>
        public TransactionState TransactionState { get; set; }
    }
}