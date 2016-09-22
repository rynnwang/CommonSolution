using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using System.Linq;
using Beyova.Api;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class EnvironmentController.
    /// </summary>
    [TokenRequired]
    public class EnvironmentEndpointController : AdminBaseController
    {
        /// <summary>
        /// The service
        /// </summary>
        static CommonAdminService service = new CommonAdminService();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentEndpointController"/> class.
        /// </summary>
        public EnvironmentEndpointController() : base() { }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View(GetViewFullPath(Constants.ViewNames.EnvironmentEndpointPanelView));
        }

        /// <summary>
        /// Selections this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Selection()
        {
            var endpoints = service.QueryEnvironmentEndpoint(null, null, null);
            return View(GetViewFullPath(Constants.ViewNames.EnvironmentView), endpoints);
        }

        /// <summary>
        /// Queries the environment endpoint.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="code">The code.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>PartialViewResult.</returns>
        public PartialViewResult QueryEnvironmentEndpoint(Guid? key = null, string code = null, string environment = null)
        {
            try
            {
                var result = service.QueryEnvironmentEndpoint(key, code, environment);
                return PartialView(GetViewFullPath(Constants.ViewNames.EnvironmentEndpointListView), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { key, code, environment });
            }
        }

        /// <summary>
        /// Gets the environment endpoint.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult GetEnvironmentEndpoint(Guid? key)
        {
            try
            {
                var result = service.QueryEnvironmentEndpoint(key).FirstOrDefault();
                return Json(result);
            }
            catch
            {
                return Json(null);
            }
        }

        /// <summary>
        /// Creates the or update environment endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CreateOrUpdateEnvironmentEndpoint(EnvironmentEndpoint endpoint)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                endpoint.CheckNullObject("endpoint");
                returnedObject = service.CreateOrUpdateEnvironmentEndpoint(endpoint);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(endpoint);
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
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "EnvironmentEndpoint", viewName);
        }
    }
}