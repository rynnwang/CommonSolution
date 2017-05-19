using System;
using System.Collections.Generic;
using Beyova.RestApi;

namespace Beyova.Gravity
{
    /// <summary>
    /// Interface ICentralAuthenticationManagementService
    /// </summary>
    [ApiContract("v1", "CentralAuthenticationManagementService")]
    public interface ICentralAuthenticationManagementService
    {
        /// <summary>
        /// Queries the admin user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminUserInfo&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminUserInfo, HttpConstants.HttpMethod.Post)]
        List<BaseObject<AdminUserInfoBase>> QueryAdminUser(AdminUserCriteria criteria);

        /// <summary>
        /// Creates the or update admin user.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminUserInfo, HttpConstants.HttpMethod.Put)]
        Guid? CreateOrUpdateAdminUser(AdminUserInfoBase userInfo);

        /// <summary>
        /// Binds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "BindOnUser")]
        Guid? BindRoleOnUser(AdminRoleBinding binding);

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "UnbindOnUser")]
        void UnbindRoleOnUser(AdminRoleBinding binding);

        /// <summary>
        /// Requests the admin password reset.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns>System.String.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminUserInfo, HttpConstants.HttpMethod.Post, "ResetPassword")]
        string RequestAdminPasswordReset(string loginName);

        /// <summary>
        /// Queries the admin role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminRole&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminRole, HttpConstants.HttpMethod.Post)]
        List<BaseObject<AdminRole>> QueryAdminRole(AdminRoleCriteria criteria);

        /// <summary>
        /// Creates the or update admin role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminRole, HttpConstants.HttpMethod.Put)]
        Guid? CreateOrUpdateAdminRole(AdminRole role);

        /// <summary>
        /// Binds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "BindOnRole")]
        Guid? BindPermissionOnRole(AdminPermissionBinding binding);

        /// <summary>
        /// Unbinds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "UnbindOnRole")]
        void UnbindPermissionOnRole(AdminPermissionBinding binding);

        /// <summary>
        /// Gets the admin permission by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>AdminPermission.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Get)]
        AdminPermission GetAdminPermissionByKey(Guid? key);

        /// <summary>
        /// Queries the admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminPermission&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Post)]
        List<AdminPermission> QueryAdminPermission(AdminPermissionCriteria criteria);

        /// <summary>
        /// Creates the or update admin permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Put)]
        Guid? CreateOrUpdateAdminPermission(AdminPermission permission);
    }
}
