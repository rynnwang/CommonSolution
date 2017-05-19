using System;

namespace Beyova
{
    /// <summary>
    /// Class TransactionRequest.
    /// </summary>
    public class TransactionRequest : TransactionBase, ITransactionRequest, IOwnerIdentifiable
    {
        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>
        /// The owner key.
        /// </value>
        public Guid? OwnerKey { get; set; }
    }
}