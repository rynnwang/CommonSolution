using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova.Api;
using Beyova.CodeSmith;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class CodeSmithController.
    /// </summary>
    //[TokenRequired]
    public class CodeSmithController : AdminBaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View(GetViewFullPath(Constants.ViewNames.CodeSmithPanel));
        }

        /// <summary>
        /// Uploads the library.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult UploadLibrary()
        {
            try
            {
                Dictionary<string, byte[]> assemblyBytes = new Dictionary<string, byte[]>();

                if (Request.Files != null && Request.Files.Keys != null)
                {
                    var key = Request.Files.Keys[0];

                    foreach (HttpPostedFileBase one in Request.Files.GetMultiple(key))
                    {
                        assemblyBytes.Add(string.IsNullOrWhiteSpace(one.FileName) ? string.Empty : Path.GetFileNameWithoutExtension(one.FileName), one.InputStream.ToBytes());
                    }
                }

                if (!assemblyBytes.HasItem())
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(assemblyBytes));
                }

                var workshopKey = WorkshopManager.InitializeWorkshop(assemblyBytes);
                return RedirectToAction("Workshop", new { key = workshopKey });
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex);
            }
        }

        /// <summary>
        /// Workshops the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Workshop(Guid? key)
        {
            try
            {
                var workshop = WorkshopManager.GetWorkshop(key);

                if (workshop == null)
                {
                    return this.RenderAsNotFoundPage("Workshop is not found.");
                }

                return View(GetViewFullPath(Constants.ViewNames.CodeWorkshop), workshop);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, key);
            }
        }

        /// <summary>
        /// Gets the interfaces.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult GetInterfaces(Guid? key)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            BaseException exception = null;

            try
            {
                var workshop = WorkshopManager.GetWorkshop(key);

                if (workshop != null)
                {
                    foreach (var one in workshop.GetInterfaces())
                    {
                        if (!one.Value.Contains("`"))
                        {
                            result.Add(one.Key, one.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, key);
            }

            ApiHandlerBase.PackageResponse(this.Response, result, exception);
            return null;
        }

        /// <summary>
        /// Generates as objective c.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="setting">The setting.</param>
        /// <returns>System.Web.Mvc.ActionResult.</returns>
        [HttpPost]
        public ActionResult GenerateAsObjectiveC(Guid? key, string setting)
        {
            try
            {
                var workshop = WorkshopManager.GetWorkshop(key);

                if (workshop == null)
                {
                    return this.RenderAsNotFoundPage("Workshop is not found.");
                }

                var settingObject = setting.TryConvertJsonToObject<ObjectiveCCodeGeneratorSetting>();
                settingObject.CheckNullObject(nameof(settingObject));

                return File(workshop.GenerateObjectiveCCodeAsZip(settingObject), HttpConstants.ContentType.ZipFile, string.Format("{0}{1}.zip", settingObject?.Prefix, DateTime.UtcNow.ToString("yyyyMMddHHddssfff")));
            }
            catch (Exception ex)
            {
                ApiHandlerBase.PackageResponse(this.Response, null, ex.Handle(new { key, setting }));
                return null;
            }
        }

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "CodeSmith", viewName);
        }


    }
}