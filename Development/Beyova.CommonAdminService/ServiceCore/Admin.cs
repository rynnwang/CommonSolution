using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.CommonAdminService.DataAccessController;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class CommonAdminService.
    /// </summary>
    partial class CommonAdminService
    {
        /// <summary>
        /// The admin expiration
        /// </summary>
        static int adminExpiration = 120;

        /// <summary>
        /// Authenticates the admin user.
        /// </summary>
        /// <param name="accessCredential">The access credential.</param>
        /// <returns>AdminSession.</returns>
        public AdminAuthenticationResult AuthenticateAdminUser(AccessCredential accessCredential)
        {
            try
            {
                accessCredential.CheckNullObject("accessCredential");
                AdminUserInfo userInfo = null;

                using (var controller = new AdminUserInfoAccessController())
                {
                    userInfo = controller.AuthenticateAdminUser(accessCredential);
                }

                if (userInfo != null)
                {
                    using (var controller = new AdminSessionAccessController(adminExpiration))
                    {
                        var session = controller.CreateAdminSession(new AdminSession
                        {
                            IpAddress = ContextHelper.IpAddress,
                            UserAgent = ContextHelper.UserAgent,
                            OwnerKey = userInfo.Key
                        });

                        return new AdminAuthenticationResult
                        {
                            TokenExpiredStamp = session.ExpiredStamp,
                            Token = session.Token,
                            UserInfo = userInfo
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( accessCredential);
            }

            return null;
        }

        /// <summary>
        /// Authenticates the third party identifier.
        /// </summary>
        /// <param name="thirdPartyId">The third party identifier.</param>
        /// <returns>AdminAuthenticationResult.</returns>
        public AdminAuthenticationResult AuthenticateThirdPartyId(string thirdPartyId)
        {
            try
            {
                thirdPartyId.CheckEmptyString("thirdPartyId");
                AdminUserInfo userInfo = null;

                using (var controller = new AdminUserInfoAccessController())
                {
                    userInfo = controller.QueryAdminUserInfo(new AdminUserCriteria { ThirdPartyId = thirdPartyId }).FirstOrDefault();
                }

                if (userInfo != null)
                {
                    using (var controller = new AdminSessionAccessController(adminExpiration))
                    {
                        var session = controller.CreateAdminSession(new AdminSession
                        {
                            IpAddress = ContextHelper.IpAddress,
                            UserAgent = ContextHelper.UserAgent,
                            OwnerKey = userInfo.Key
                        });

                        return new AdminAuthenticationResult
                        {
                            TokenExpiredStamp = session.ExpiredStamp,
                            Token = session.Token,
                            UserInfo = userInfo
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( thirdPartyId);
            }

            return null;
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        /// <param name="token">The token.</param>
        public void SignOut(string token)
        {
            try
            {
                token.CheckEmptyString("token");

                using (var controller = new AdminSessionAccessController(adminExpiration))
                {
                    controller.DeleteAdminSession(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( token);
            }
        }

        /// <summary>
        /// Gets the admin user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>AdminUserInfo.</returns>
        public AdminUserInfo GetAdminUserInfoByToken(string token)
        {
            try
            {
                token.CheckEmptyString("token");

                using (var controller = new AdminUserInfoAccessController())
                {
                    return controller.GetAdminUserInfoByToken(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( token);
            }
        }

        /// <summary>
        /// Gets the admin user by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>AdminUserInfo.</returns>
        public AdminUserInfo GetAdminUserByKey(Guid? key)
        {
            return key == null ? null : QueryAdminUser(new AdminUserCriteria { Key = key }).FirstOrDefault();
        }

        /// <summary>
        /// Queries the admin user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminUserInfo&gt;.</returns>
        public List<AdminUserInfo> QueryAdminUser(AdminUserCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                using (var controller = new AdminUserInfoAccessController())
                {
                    return controller.QueryAdminUserInfo(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( criteria);
            }
        }

        /// <summary>
        /// Creates the or update admin user.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateAdminUser(AdminUserInfo userInfo)
        {
            try
            {
                userInfo.CheckNullObject("userInfo");

                using (var controller = new AdminUserInfoAccessController())
                {
                    return controller.CreateOrUpdateAdminUserInfo(userInfo, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( userInfo);
            }
        }

        /// <summary>
        /// Binds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? BindRoleOnUser(AdminRoleBinding binding)
        {
            try
            {
                binding.CheckNullObject("binding");

                using (var controller = new AdminUserInfoAccessController())
                {
                    return controller.BindRoleOnUser(binding, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( binding);
            }
        }

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void UnbindRoleOnUser(AdminRoleBinding binding)
        {
            try
            {
                binding.CheckNullObject("binding");

                using (var controller = new AdminUserInfoAccessController())
                {
                    controller.UnbindRoleOnUser(binding, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( binding);
            }
        }

        /// <summary>
        /// Requests the admin password reset.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="expiration">The expiration.</param>
        /// <returns>System.String.</returns>
        public string RequestAdminPasswordReset(string loginName, int expiration)
        {
            try
            {
                loginName.CheckEmptyString("loginName");

                using (var controller = new AdminUserInfoAccessController())
                {
                    return controller.RequestAdminPasswordReset(loginName, expiration);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { loginName, expiration });
            }
        }

        /// <summary>
        /// Gets the admin role by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>AdminRole.</returns>
        public AdminRole GetAdminRoleByKey(Guid? key)
        {
            return key == null ? null : QueryAdminRole(new AdminRoleCriteria { Key = key }).FirstOrDefault();
        }

        /// <summary>
        /// Queries the admin role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminRole&gt;.</returns>
        public List<AdminRole> QueryAdminRole(AdminRoleCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                using (var controller = new AdminRoleAccessController())
                {
                    return controller.QueryAdminRole(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( criteria);
            }
        }

        /// <summary>
        /// Creates the or update admin role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateAdminRole(AdminRole role)
        {
            try
            {
                role.CheckNullObject("role");

                using (var controller = new AdminRoleAccessController())
                {
                    return controller.CreateOrUpdateAdminRole(role, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( role);
            }
        }

        /// <summary>
        /// Binds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? BindPermissionOnRole(AdminPermissionBinding binding)
        {
            try
            {
                binding.CheckNullObject("binding");

                using (var controller = new AdminRoleAccessController())
                {
                    return controller.BindPermissionOnRole(binding, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( binding);
            }
        }

        /// <summary>
        /// Unbinds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void UnbindPermissionOnRole(AdminPermissionBinding binding)
        {
            try
            {
                binding.CheckNullObject("binding");

                using (var controller = new AdminRoleAccessController())
                {
                    controller.UnbindPermissionOnRole(binding, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( binding);
            }
        }

        /// <summary>
        /// Gets the admin permission by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>AdminPermission.</returns>
        public AdminPermission GetAdminPermissionByKey(Guid? key)
        {
            return key == null ? null : QueryAdminPermission(new AdminPermissionCriteria { Key = key }).FirstOrDefault();
        }

        /// <summary>
        /// Queries the admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminPermission&gt;.</returns>
        public List<AdminPermission> QueryAdminPermission(AdminPermissionCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                using (var controller = new AdminPermissionAccessController())
                {
                    return controller.QueryAdminPermission(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( criteria);
            }
        }

        /// <summary>
        /// Creates the or update admin permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateAdminPermission(AdminPermission permission)
        {
            try
            {
                permission.CheckNullObject("permission");

                using (var controller = new AdminPermissionAccessController())
                {
                    return controller.CreateOrUpdateAdminPermission(permission, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle( permission);
            }
        }
    }
}
