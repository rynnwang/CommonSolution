using System;
using System.Collections.Generic;
using Beyova.Api;
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
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserRoleView)]
        List<BaseObject<AdminUserInfo>> QueryAdminUser(AdminUserCriteria criteria);

        /// <summary>
        /// Creates the or update admin user.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminUserInfo, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserAdministration)]
        Guid? CreateOrUpdateAdminUser(AdminUserInfo userInfo);

        /// <summary>
        /// Deletes the admin user.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminUserInfo, HttpConstants.HttpMethod.Delete)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserAdministration)]
        void DeleteAdminUser(Guid? userKey);

        /// <summary>
        /// Binds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "BindOnUser")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserAdministration)]
        Guid? BindRoleOnUser(AdminRoleBinding binding);

        /// <summary>
        /// Gets the user role bindings.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Get, "UserRoleBinding")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserAdministration)]
        List<AdminRoleBinding> GetUserRoleBindings(Guid? userKey);

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "UnbindOnUser")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserAdministration)]
        void UnbindRoleOnUser(AdminRoleBinding binding);

        /// <summary>
        /// Requests the admin password reset.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminUserInfo, HttpConstants.HttpMethod.Post, "ResetPassword")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserAdministration)]
        string RequestAdminPasswordReset(Guid? userKey);

        /// <summary>
        /// Queries the admin role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminRole&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminRole, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminUserRoleView)]
        List<BaseObject<AdminRole>> QueryAdminRole(AdminRoleCriteria criteria);

        /// <summary>
        /// Creates the or update admin role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminRole, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        Guid? CreateOrUpdateAdminRole(AdminRole role);

        /// <summary>
        /// Binds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "BindOnRole")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        Guid? BindPermissionOnRole(AdminPermissionBinding binding);

        /// <summary>
        /// Unbinds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Post, "UnbindOnRole")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        void UnbindPermissionOnRole(AdminPermissionBinding binding);

        /// <summary>
        /// Deletes the admin role.
        /// </summary>
        /// <param name="roleKey">The role key.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Delete)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        void DeleteAdminRole(Guid? roleKey);

        /// <summary>
        /// Gets the role permission bindings.
        /// </summary>
        /// <param name="roleKey">The role key.</param>
        /// <returns></returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPrivilege, HttpConstants.HttpMethod.Get, "RolePermissionBinding")]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        List<AdminPermissionBinding> GetRolePermissionBindings(Guid? roleKey);

        /// <summary>
        /// Gets the admin permission by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>AdminPermission.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Get)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        AdminPermission GetAdminPermissionByKey(Guid? key);

        /// <summary>
        /// Queries the admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AdminPermission&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        List<AdminPermission> QueryAdminPermission(AdminPermissionCriteria criteria);

        /// <summary>
        /// Creates the or update admin permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        Guid? CreateOrUpdateAdminPermission(AdminPermission permission);

        /// <summary>
        /// Deletes the admin permission.
        /// </summary>
        /// <param name="permissionKey">The permission key.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.AdminPermission, HttpConstants.HttpMethod.Delete)]
        [ApiPermission(CommonServiceConstants.Permission.AdminSystem.AdminRolePermissionAdministration)]
        void DeleteAdminPermission(Guid? permissionKey);
    }
}