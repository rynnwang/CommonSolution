using System;
using System.Data.SqlClient;

namespace Beyova
{
    /// <summary>
    /// Class SqlTransactionScope.
    /// </summary>
    public class SqlTransactionScope : IDisposable
    {
        /// <summary>
        /// The SQL transaction
        /// </summary>
        [ThreadStatic]
        private static SqlTransaction _sqlTransaction;

        /// <summary>
        /// The _SQL command
        /// </summary>
        [ThreadStatic]
        private static SqlCommand _sqlCommand;

        /// <summary>
        /// The _current
        /// </summary>
        [ThreadStatic]
        private static SqlTransactionScope _current;

        /// <summary>
        /// Gets the SQL transaction.
        /// </summary>
        /// <value>The SQL transaction.</value>
        internal static SqlTransaction SqlTransaction { get { return _sqlTransaction; } }

        /// <summary>
        /// Gets the SQL command.
        /// </summary>
        /// <value>The SQL command.</value>
        internal static SqlCommand SqlCommand { get { return _sqlCommand; } }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        internal static SqlTransactionScope Current { get { return _current; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTransactionScope" /> class.
        /// </summary>
        /// <param name="sqlTransaction">The SQL transaction.</param>
        /// <param name="sqlCommand">The SQL command.</param>
        internal SqlTransactionScope(SqlTransaction sqlTransaction, SqlCommand sqlCommand)
        {
            InternalDispose();
            _sqlTransaction = sqlTransaction;
            _sqlCommand = sqlCommand;
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            var sqlTransaction = _sqlTransaction;

            try
            {
                if (sqlTransaction == null)
                {
                    throw ExceptionFactory.CreateOperationException(minor: "DuplicatedTransactionCommit");
                }
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
            finally
            {
                _sqlTransaction.Dispose();
                _sqlTransaction = null;
            }
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        internal void Rollback()
        {
            var sqlTransaction = _sqlTransaction;

            try
            {
                if (sqlTransaction == null)
                {
                    throw ExceptionFactory.CreateOperationException(minor: "DuplicatedTransactionRollback");
                }

                sqlTransaction.Rollback();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
            finally
            {
                _sqlTransaction.Dispose();
                _sqlTransaction = null;
            }
        }

        /// <summary>
        /// Internals the dispose.
        /// </summary>
        protected void InternalDispose()
        {
            if (_sqlTransaction != null)
            {
                _sqlTransaction.Dispose();
                _sqlTransaction = null;
            }

            if (_sqlCommand != null)
            {
                _sqlCommand = null;
            }

            if (_current != null)
            {
                _current = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            InternalDispose();
        }
    }
}