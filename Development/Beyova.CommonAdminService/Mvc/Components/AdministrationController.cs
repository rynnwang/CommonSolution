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
        public JsonResult GetUserInfo(Guid? key)
        {
            return Json(service.GetAdminUserByKey(key));
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