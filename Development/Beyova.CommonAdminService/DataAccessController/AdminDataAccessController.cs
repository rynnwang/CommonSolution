using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.CommonAdminService.DataAccessController
{
    /// <summary>
    /// Class AdminDataAccessController.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AdminDataAccessController<T> : BaseDataAccessController<T>
    {
        #region Constants

        /// <summary>
        /// The column_ login name
        /// </summary>
        protected const string column_LoginName = "LoginName";

        /// <summary>
        /// The column_ permission identifier
        /// </summary>
        protected const string column_PermissionIdentifier = "PermissionIdentifier";

        /// <summary>
        /// The column_ by admin user
        /// </summary>
        protected const string column_ByAdminUser = "ByAdminUser";

        /// <summary>
        /// The column_ by admin role
        /// </summary>
        protected const string column_ByAdminRole = "ByAdminRole";

        /// <summary>
        /// The column_ by permission identifier
        /// </summary>
        protected const string column_ByPermissionIdentifier = "ByPermissionIdentifier";

        /// <summary>
        /// The column_ role key
        /// </summary>
        protected const string column_RoleKey = "RoleKey";

        /// <summary>
        /// The column_ permission key
        /// </summary>
        protected const string column_PermissionKey = "PermissionKey";

        /// <summary>
        /// The column_ is unique
        /// </summary>
        protected const string column_IsUnique = "IsUnique";

        /// <summary>
        /// The column_ password
        /// </summary>
        protected const string column_Password = "Password";

        /// <summary>
        /// The column_ active only
        /// </summary>
        protected const string column_ActiveOnly = "ActiveOnly";

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDataAccessController{T}" /> class.
        /// </summary>
        protected AdminDataAccessController()
            : base()
        {
        }

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="rawPassword">The raw password.</param>
        /// <returns>System.String.</returns>
        protected string HashPassword(string rawPassword)
        {
            return rawPassword.SafeToString().ToSHA1(Encoding.UTF8);
        }
    }
}
