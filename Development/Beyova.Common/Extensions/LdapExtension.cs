using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace Beyova
{
    /// <summary>
    /// Class LdapExtension.
    /// </summary>
    public static class LdapExtension
    {
        private const string ldapProtocol = "LDAP://";

        private const string cn = "CN=";

        /// <summary>
        /// Gets the LDAP path.
        /// </summary>
        /// <param name="ldapPath">The LDAP path.</param>
        /// <returns>System.String.</returns>
        private static string GetLdapPath(string ldapPath = null)
        {
            if (string.IsNullOrWhiteSpace(ldapPath))
            {
                ldapPath = ldapProtocol + Domain.GetCurrentDomain()?.Name;
            }
            else if (!ldapPath.StartsWith(ldapProtocol, StringComparison.OrdinalIgnoreCase))
            {
                ldapPath = ldapProtocol + ldapPath;
            }
            else
            {
                ldapPath = ldapProtocol + ldapPath.Substring(ldapProtocol.Length);
            }

            return ldapPath;
        }

        /// <summary>
        /// Gets the directory entry.
        /// </summary>
        /// <param name="ldapPath">The LDAP path.Example: LDAP://yourdomain.com </param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns>System.DirectoryServices.DirectoryEntry.</returns>
        private static DirectoryEntry GetDirectoryEntry(string ldapPath = null, AuthenticationTypes authenticationType = AuthenticationTypes.Secure)
        {
            return new DirectoryEntry(GetLdapPath(ldapPath));
        }

        /// <summary>
        /// Validates the directory entry.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="ldapPath">The LDAP path.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns>System.DirectoryServices.DirectoryEntry.</returns>
        public static DirectoryEntry ValidateDirectoryEntry(string userName, string password, string ldapPath = null, AuthenticationTypes authenticationType = AuthenticationTypes.Secure)
        {
            try
            {
                userName.CheckEmptyString(nameof(userName));
                password.CheckEmptyString(nameof(password));

                return new DirectoryEntry()
                {
                    Path = GetLdapPath(ldapPath),
                    Username = userName,
                    Password = password,
                    AuthenticationType = authenticationType
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { ldapPath, userName, password, authenticationType });
            }
        }

        /// <summary>
        /// Searches the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="ldapPath">The LDAP path.</param>
        /// <returns>System.DirectoryServices.DirectoryEntry.</returns>
        public static DirectoryEntry SearchUser(string userName, string ldapPath = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return null;
                }

                userName = userName.TrimStart(cn);

                using (var de = GetDirectoryEntry(ldapPath))
                {
                    using (var directorySearcher = new DirectorySearcher(de))
                    {
                        directorySearcher.Filter = string.Format("(&(objectClass=person)(objectCategory=user)(sAMAccountname={0}))", userName);
                        SearchResult searchResult = directorySearcher.FindOne();

                        return searchResult?.GetDirectoryEntry();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { userName, ldapPath });
            }
        }

        /// <summary>
        /// Gets the member groups.
        /// </summary>
        /// <param name="userName">Name of the user. Example: Domain\Account</param>
        /// <param name="ldapPath">The LDAP path. Example: LDAP://yourdomain.com</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns>List&lt;DirectoryEntry&gt;.</returns>
        public static List<DirectoryEntry> GetMemberGroups(string userName, string ldapPath = null, AuthenticationTypes authenticationType = AuthenticationTypes.Secure)
        {
            try
            {
                var entry = SearchUser(userName);
                List<DirectoryEntry> result = new List<DirectoryEntry>();

                foreach (string path in entry.Properties["memberOf"])
                {
                    result.Add(new DirectoryEntry(path));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { ldapPath, userName });
            }
        }

        /// <summary>
        /// Gets the member group names.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="ldapPath">The LDAP path.</param>
        /// <param name="authenticationType">Type of the authentication.</param>
        /// <returns>System.Collections.Generic.List&lt;System.String&gt;.</returns>
        public static List<string> GetMemberGroupNames(string userName, string ldapPath = null, AuthenticationTypes authenticationType = AuthenticationTypes.Secure)
        {
            try
            {
                var entry = SearchUser(userName);
                List<string> result = new List<string>();

                foreach (string path in entry.Properties["memberOf"])
                {
                    result.AddIfNotNull(InternalGetCNValue(path));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { ldapPath, userName, authenticationType });
            }
        }

        /// <summary>
        /// Tries the authenticate.
        /// </summary>
        /// <param name="ldapPath">The LDAP path. Example: LDAP://yourdomain.com </param>
        /// <param name="userName">Name of the user. Example: Domain\Account</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if authentication is passed, <c>false</c> otherwise.</returns>
        public static bool TryAuthenticate(string ldapPath, string userName, string password)
        {
            try
            {
                var adEntry = ValidateDirectoryEntry(userName, password, ldapPath);

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

        #region Extension

        /// <summary>
        /// Gets the cn value.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>System.String.</returns>
        public static string GetCNValue(this DirectoryEntry entry)
        {
            return InternalGetCNValue(entry?.Path);
        }

        /// <summary>
        /// Gets the cn value.
        /// </summary>
        /// <param name="entryPath">The entry path.</param>
        /// <returns>System.String.</returns>
        private static string InternalGetCNValue(string entryPath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(entryPath))
                {
                    var segments = entryPath.Split(','.AsArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var one in segments)
                    {
                        var value = one.Trim();
                        if (value.StartsWith(cn, StringComparison.OrdinalIgnoreCase))
                        {
                            return value.Substring(cn.Length).Trim();
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle(entryPath);
            }
        }

        #endregion Extension

        #region LDAP Account

        /// <summary>
        /// Gets the domain account.
        /// </summary>
        /// <param name="accessCredential">The access credential.</param>
        /// <param name="defaultDomain">The default domain.</param>
        /// <param name="omitException">The omit exception.</param>
        /// <param name="domainMapping">The domain mapping.</param>
        /// <returns>System.String.</returns>
        public static string GetDomainAccount(this AccessCredential accessCredential, string defaultDomain = null, bool omitException = false, Dictionary<string, string> domainMapping = null)
        {
            var unifiedAccount = UnifyAccountName(accessCredential, defaultDomain, omitException, domainMapping);
            return unifiedAccount != null ? string.Format("{0}\\{1}", unifiedAccount.Domain, unifiedAccount.AccessIdentifier) : null;
        }

        /// <summary>
        /// Unifies the name of the account.
        /// </summary>
        /// <param name="accessCredential">The access credential.</param>
        /// <param name="defaultDomain">The default domain.</param>
        /// <param name="omitException">The omit exception.</param>
        /// <param name="domainMapping">The domain mapping.</param>
        /// <returns>Beyova.AccessCredential.</returns>
        public static AccessCredential UnifyAccountName(this AccessCredential accessCredential, string defaultDomain = null, bool omitException = false, Dictionary<string, string> domainMapping = null)
        {
            try
            {
                accessCredential.CheckNullObject(nameof(accessCredential));
                accessCredential.AccessIdentifier.CheckEmptyString(accessCredential.AccessIdentifier);

                string domain, accountName;

                if (accessCredential.AccessIdentifier.IsEmailAddress())
                {
                    accountName = accessCredential.AccessIdentifier.SubStringBeforeFirstMatch('@');
                    domain = accessCredential.AccessIdentifier.SubStringAfterFirstMatch('@').TrimEnd(".com", true);

                    string mappedDomain;
                    if (domainMapping != null && domainMapping.TryGetValue(domain, out mappedDomain))
                    {
                        domain = mappedDomain;
                    }
                }
                else if (accessCredential.AccessIdentifier.Contains('\\'))
                {
                    accountName = accessCredential.AccessIdentifier.SubStringAfterFirstMatch('\\');
                    domain = accessCredential.AccessIdentifier.SubStringBeforeFirstMatch('\\');
                }
                else
                {
                    domain = accessCredential.Domain;
                    accountName = accessCredential.AccessIdentifier;
                }

                accountName.CheckEmptyString(nameof(accountName));
                domain = domain.SafeToString(defaultDomain);
                return new AccessCredential
                {
                    Domain = defaultDomain,
                    AccessIdentifier = accountName,
                    Token = accessCredential.Token
                };
            }
            catch (Exception ex)
            {
                if (omitException)
                {
                    return null;
                }
                throw ex.Handle(new { accessCredential, defaultDomain });
            }
        }

        #endregion LDAP Account
    }
}