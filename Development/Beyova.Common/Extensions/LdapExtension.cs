using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Beyova.ExceptionSystem;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Beyova;
using Beyova.RestApi;
using System.Management;
using System.DirectoryServices;

namespace Beyova
{
    /// <summary>
    /// Class LdapExtension.
    /// </summary>
    public static class LdapExtension
    {
        /// <summary>
        /// Gets the directory entry.
        /// </summary>
        /// <param name="ldapPath">The LDAP path. Example: LDAP://yourdomain.com </param>
        /// <param name="userName">Name of the user. Exmaple: Domain\Account</param>
        /// <param name="password">The password. Sample: 123456</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns>DirectoryEntry.</returns>
        public static DirectoryEntry GetDirectoryEntry(string ldapPath, string userName, string password, AuthenticationTypes authenticationType = AuthenticationTypes.Secure)
        {
            try
            {
                ldapPath.CheckEmptyString("ldapPath");
                userName.CheckEmptyString("userName");
                password.CheckEmptyString("password");

                return new DirectoryEntry()
                {
                    Path = ldapPath,
                    Username = userName,
                    Password = password,
                    AuthenticationType = authenticationType
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetDirectoryEntry", new { ldapPath, userName, password });
            }
        }

        /// <summary>
        /// Tries the authenticate.
        /// </summary>
        /// <param name="ldapPath">The LDAP path. Example: LDAP://yourdomain.com </param>
        /// <param name="userName">Name of the user. Exmaple: Domain\Account</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if authentication is passed, <c>false</c> otherwise.</returns>
        public static bool TryAuthenticate(string ldapPath, string userName, string password)
        {
            try
            {
                var adEntry = GetDirectoryEntry(ldapPath, userName, password);

                if (adEntry != null)
                {
                    adEntry.RefreshCache();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the name of the user.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        ///   <c>true</c> if user name is existed, <c>false</c> otherwise.</returns>
        public static bool CheckUserName(this DirectoryEntry entry, string userName)
        {
            try
            {
                entry.CheckNullObject("entry");
                userName.CheckEmptyString("userName");

                var directorySearcher = new DirectorySearcher()
                {
                    SearchRoot = entry,
                    Filter = "(&(objectClass=user) (cn=" + userName + "))"
                };

                if ((directorySearcher.FindAll()?.Count ?? 0) > 0)
                {
                    return true;
                };

                return false;
            }
            catch (Exception ex)
            {
                throw ex.Handle("CheckUserName", new { userName });
            }
        }
    }
}
