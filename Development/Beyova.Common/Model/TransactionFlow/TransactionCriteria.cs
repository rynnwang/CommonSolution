using System;

namespace Beyova
{
    /// <summary>
    /// Class TransactionCriteria.
    /// </summary>
    public class TransactionCriteria : TransactionBase, ITransactionCriteria
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [order descending].
        /// </summary>
        /// <value><c>true</c> if [order descending]; otherwise, <c>false</c>.</value>
        public bool OrderDescending { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets the state of the transaction.
        /// </summary>
        /// <value>The state of the transaction.</value>
        public TransactionState? TransactionState { get; set; }
    }
}