using System;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using Beyova.WebExtension;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AdministrationController.
    /// </summary>
    [TokenRequired]
    [ApiPermission(Constants.Permission.Administration, ApiPermission.Required)]
    [RestApiContextConsistence]
    public class AdministrationController : AdminBaseController
    {
        /// <summary>
        /// The service
        /// </summary>
        static CommonAdminService service = new CommonAdminService();

        #region User

        /// <summary>
        /// Admins the user.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult AdminUser()
        {
            return View(GetViewFullPath(Constants.ViewNames.AdminUserPanel));
        }

        /// <summary>
        /// Manages the role.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ManageRole(Guid? key)
        {
            return View(GetViewFullPath(Constants.ViewNames.AdminUserRoleBinding), key);
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult GetUserInfo(Guid? key)
        {
            ApiHandlerBase.PackageResponse(this.Response, service.GetAdminUserByKey(key));
            return null;
        }

        /// <summary>
        /// Queries the admin user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>PartialViewResult.</returns>
        [HttpPost]
        public PartialViewResult QueryAdminUser(AdminUserCriteria criteria)
        {
            try
            {
                var result = service.QueryAdminUser(criteria);
                return PartialView(GetViewFullPath(Constants.ViewNames.AdminUserList), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Creates the or update admin user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CreateOrUpdateAdminUser(AdminUserInfo user)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                user.CheckNullObject("user");
                returnedObject = service.CreateOrUpdateAdminUser(user);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(user);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Binds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult BindRoleOnUser(AdminRoleBinding binding)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                binding.CheckNullObject("binding");
                returnedObject = service.BindRoleOnUser(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult UnbindRoleOnUser(AdminRoleBinding binding)
        {
            BaseException exception = null;

            try
            {
                binding.CheckNullObject("binding");
                service.UnbindRoleOnUser(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, "", exception);
            return null;
        }

        #endregion

        #region Role

        /// <summary>
        /// Admins the role.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult AdminRole()
        {
            return View(GetViewFullPath(Constants.ViewNames.AdminRolePanel));
        }

        /// <summary>
        /// Manages the permission.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ManagePermission(Guid? key)
        {
            return View(GetViewFullPath(Constants.ViewNames.AdminRolePermissionBinding), key);
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult GetRole(Guid? key)
        {
            return Json(service.GetAdminRoleByKey(key));
        }

        /// <summary>
        /// Queries the admin role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>PartialViewResult.</returns>
        [HttpPost]
        public PartialViewResult QueryAdminRole(AdminRoleCriteria criteria)
        {
            try
            {
                var result = service.QueryAdminRole(criteria);
                return PartialView(GetViewFullPath(Constants.ViewNames.AdminRoleList), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Creates the or update admin role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CreateOrUpdateAdminRole(AdminRole role)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                role.CheckNullObject("role");
                returnedObject = service.CreateOrUpdateAdminRole(role);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(role);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Binds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult BindPermissionOnRole(AdminPermissionBinding binding)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                binding.CheckNullObject("binding");
                returnedObject = service.BindPermissionOnRole(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Unbinds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult UnbindPermissionOnRole(AdminPermissionBinding binding)
        {
            BaseException exception = null;

            try
            {
                binding.CheckNullObject("binding");
                service.UnbindPermissionOnRole(binding);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, "", exception);
            return null;
        }

        #endregion

        #region Permission

        /// <summary>
        /// Admins the permission.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult AdminPermission()
        {
            return View(GetViewFullPath(Constants.ViewNames.AdminPermissionPanel));
        }

        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult GetPermission(Guid? key)
        {
            return Json(service.GetAdminPermissionByKey(key));
        }

        /// <summary>
        /// Queries the admin permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>PartialViewResult.</returns>
        [HttpPost]
        public PartialViewResult QueryAdminPermission(AdminPermissionCriteria criteria)
        {
            try
            {
                var result = service.QueryAdminPermission(criteria);
                return PartialView(GetViewFullPath(Constants.ViewNames.AdminPermissionList), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Creates the or update admin permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CreateOrUpdateAdminPermission(AdminPermission permission)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                permission.CheckNullObject("permission");
                returnedObject = service.CreateOrUpdateAdminPermission(permission);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(permission);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "Admin", viewName);
        }

        #endregion
    }
}