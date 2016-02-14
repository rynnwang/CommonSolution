using System;
using System.Web.Mvc;
using Beyova;
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
    [RestApiSessionConsistence(null)]
    public class AdministrationController : AdminBaseController
    {
        static CommonAdminService service = new CommonAdminService();

        #region User

        [HttpGet]
        public ActionResult AdminUser()
        {
            return View(Constants.ViewNames.AdminUserPanel);
        }

        [HttpPost]
        public ActionResult GetUserInfo(Guid? key)
        {
            ApiHandlerBase.PackageResponse(this.Response, service.GetAdminUserByKey(key));
            return null;
        }

        [HttpPost]
        public PartialViewResult QueryAdminUser(AdminUserCriteria criteria)
        {
            try
            {
                var result = service.QueryAdminUser(criteria);
                return PartialView(Constants.ViewNames.AdminUserList, result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, Request.HttpMethod, "QueryAdminUser", criteria);
            }
        }

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
                exception = ex.Handle("CreateOrUpdateAdminUser", user);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Binds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
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
                exception = ex.Handle("BindRoleOnUser", binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Unbinds the role on user.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
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
                exception = ex.Handle("UnbindRoleOnUser", binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, "", exception);
            return null;
        }

        #endregion

        #region Role

        [HttpGet]
        public ActionResult AdminRole()
        {
            return View(Constants.ViewNames.AdminRolePanel);
        }

        [HttpPost]
        public JsonResult GetRole(Guid? key)
        {
            return Json(service.GetAdminRoleByKey(key));
        }

        [HttpPost]
        public PartialViewResult QueryAdminRole(AdminRoleCriteria criteria)
        {
            try
            {
                var result = service.QueryAdminRole(criteria);
                return PartialView(Constants.ViewNames.AdminRoleList, result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, Request.HttpMethod, "QueryAdminRole", criteria);
            }
        }

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
                exception = ex.Handle("CreateOrUpdateAdminRole", role);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Binds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
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
                exception = ex.Handle("BindPermissionOnRole", binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Unbinds the permission on role.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>JsonResult.</returns>
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
                exception = ex.Handle("UnbindPermissionOnRole", binding);
            }

            ApiHandlerBase.PackageResponse(this.Response, "", exception);
            return null;
        }

        #endregion

        #region Permission

        [HttpGet]
        public ActionResult AdminPermission()
        {
            return View(Constants.ViewNames.AdminPermissionPanel);
        }

        [HttpPost]
        public JsonResult GetPermission(Guid? key)
        {
            return Json(service.GetAdminPermissionByKey(key));
        }

        [HttpPost]
        public PartialViewResult QueryAdminPermission(AdminPermissionCriteria criteria)
        {
            try
            {
                var result = service.QueryAdminPermission(criteria);
                return PartialView(Constants.ViewNames.AdminPermissionList, result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, Request.HttpMethod, "QueryAdminPermission", criteria);
            }
        }

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
                exception = ex.Handle("CreateOrUpdateAdminPermission", permission);
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        #endregion
    }
}