using System;
using System.Collections.Generic;
using Beyova;
using Beyova.Api;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface ITransactionService
    /// </summary>
    /// <seealso cref="CommonServiceInterface.ITransactionService{TransactionRequest, TransactionAudit, TransactionCriteria}" />
    public interface ITransactionService : ITransactionService<TransactionRequest, TransactionAudit, TransactionCriteria>
    {
    }

    /// <summary>
    /// Interface ITransactionService
    /// </summary>
    /// <typeparam name="TTransactionRequest">The type of the t transaction request.</typeparam>
    /// <typeparam name="TTransactionAudit">The type of the t transaction audit.</typeparam>
    /// <typeparam name="TTransactionCriteria">The type of the t transaction criteria.</typeparam>
    [ApiContract("v1", "TransactionService")]
    public interface ITransactionService<TTransactionRequest, TTransactionAudit, TTransactionCriteria>
           where TTransactionRequest : ITransactionRequest
           where TTransactionAudit : ITransactionAudit
    {
        /// <summary>
        /// Creates the transaction.
        /// </summary>
        /// <param name="transactionRequest">The transaction request.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation("Transaction", HttpConstants.HttpMethod.Put, "Create")]
        Guid? CreateTransaction(TTransactionRequest transactionRequest);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="transactionKey">The transaction key.</param>
        /// <returns>TTransactionAudit.</returns>
        [ApiOperation("Transaction", HttpConstants.HttpMethod.Post, "Commit")]
        TTransactionAudit CommitTransaction(Guid? transactionKey);

        /// <summary>
        /// Queries the transaction audit.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TTransactionAudit&gt;.</returns>
        [ApiOperation("Transaction", HttpConstants.HttpMethod.Post, "Query")]
        List<TTransactionAudit> QueryTransactionAudit(TTransactionCriteria criteria);

        /// <summary>
        /// Cancels the transaction.
        /// </summary>
        /// <param name="transactionKey">The transaction key.</param>
        /// <returns>TTransactionAudit.</returns>
        [ApiOperation("Transaction", HttpConstants.HttpMethod.Delete)]
        TTransactionAudit CancelTransaction(Guid? transactionKey);
    }
}
