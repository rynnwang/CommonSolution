
using System;
using System.Data.SqlClient;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class SqlTransactionScope.
    /// </summary>
    public class SqlTransactionScope : IDisposable
    {
        /// <summary>
        /// The SQL transction
        /// </summary>
        [ThreadStatic]
        private static SqlTransaction _sqlTransction;

        /// <summary>
        /// The _SQL command
        /// </summary>
        [ThreadStatic]
        private static SqlCommand _sqlCommand;

        /// <summary>
        /// Gets the SQL transaction.
        /// </summary>
        /// <value>The SQL transaction.</value>
        internal static SqlTransaction SqlTransaction { get { return _sqlTransction; } }

        /// <summary>
        /// Gets the SQL command.
        /// </summary>
        /// <value>The SQL command.</value>
        internal static SqlCommand SqlCommand { get { return _sqlCommand; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTransactionScope" /> class.
        /// </summary>
        /// <param name="sqlTransction">The SQL transction.</param>
        /// <param name="sqlCommand">The SQL command.</param>
        internal SqlTransactionScope(SqlTransaction sqlTransction, SqlCommand sqlCommand)
        {
            _sqlTransction = sqlTransction;
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <exception cref="OperationFailureException">Commit;null</exception>
        public void Commit()
        {
            var sqlTransaction = _sqlTransction;

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
                _sqlTransction.Dispose();
                _sqlTransction = null;
            }
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        /// <exception cref="OperationFailureException">Rollback;null</exception>
        internal void Rollback()
        {
            var sqlTransaction = _sqlTransction;

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
                _sqlTransction.Dispose();
                _sqlTransction = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_sqlTransction != null)
            {
                _sqlTransction.Dispose();
                _sqlTransction = null;
            }

            if (_sqlCommand != null)
            {
                _sqlCommand = null;
            }
        }
    }
}
