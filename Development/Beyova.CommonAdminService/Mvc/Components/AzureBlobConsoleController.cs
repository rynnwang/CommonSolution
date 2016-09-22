using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova.AzureExtension;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AzureBlobConsoleController.
    /// </summary>
    public class AzureBlobConsoleController : AdminBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobConsoleController" /> class.
        /// </summary>
        public AzureBlobConsoleController() : base()
        {
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Query()
        {
            return View(GetViewFullPath(Constants.ViewNames.AzureBlobPanelView));
        }

        /// <summary>
        /// Gets the BLOB container.
        /// </summary>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <returns>JsonResult.</returns>
        public JsonResult GetBlobContainer(string azureConnectionString)
        {
            List<string> containers = null;

            try
            {
                azureConnectionString.CheckEmptyString("azureConnectionString");
                AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);

                containers = manager.GetContainers().ToList();
            }
            catch
            {
                containers = new List<string>();
            }

            return Json(containers);
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="count">The count.</param>
        /// <returns>ActionResult.</returns>
        public PartialViewResult QueryBlob(string container, string azureConnectionString, string contentType = null, string md5 = null, long? length = null, int? count = null)
        {
            try
            {
                azureConnectionString.CheckEmptyString("azureConnectionString");
                container.CheckEmptyString("container");

                ViewBag.azureConnectionString = azureConnectionString;
                AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);

                var items = manager.QueryBinaryBlobByContainer(container, contentType, md5, length, count ?? 100);
                return PartialView(GetViewFullPath(Constants.ViewNames.AzureBlobListView), items);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { container, azureConnectionString, contentType, md5, length, count });
            }
        }

        /// <summary>
        /// Accesses the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult AccessBlob(string container, string identifier, string azureConnectionString)
        {
            try
            {
                azureConnectionString.CheckEmptyString("azureConnectionString");
                container.CheckEmptyString("container");
                identifier.CheckEmptyString("identifier");

                AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);

                var credential = manager.CreateBlobDownloadCredential(container, identifier, 10);
                return Redirect(credential.CredentialUri);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { container, azureConnectionString, identifier });
            }
        }

        /// <summary>
        /// News the BLOB.
        /// </summary>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult NewBlob(string azureConnectionString)
        {
            if (string.IsNullOrWhiteSpace(azureConnectionString))
            {
                return this.HttpNotFound();
            }
            else
            {
                try
                {
                    AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);
                    ViewBag.azureConnectionString = azureConnectionString;
                    return View(GetViewFullPath(Constants.ViewNames.AzureBlobNewItemView), manager.GetContainers().ToList());
                }
                catch (Exception ex)
                {
                    return this.HandleExceptionToRedirection(ex, azureConnectionString);
                }
            }
        }

        /// <summary>
        /// Creates the upload credential.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <returns>JsonResult.</returns>
        public JsonResult CreateUploadCredential(string container, string identifier, string azureConnectionString)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                container.CheckEmptyString("container");
                AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);
                var credential = manager.CreateBlobUploadCredential(container, identifier.SafeToString(Guid.NewGuid().ToString()), 10);
                returnedObject = credential;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { container, identifier, azureConnectionString });
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Commits the new BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>System.Web.Mvc.JsonResult.</returns>
        [HttpPost]
        public JsonResult CommitNewBlob(string container, string identifier, string azureConnectionString, string contentType = null)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                container.CheckEmptyString("container");

                if ((Request.Files?.Count ?? 0) > 0)
                {
                    AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);
                    var uriCredential = manager.CreateBlobUploadCredential(container, identifier, 10);
                    var etag = manager.UploadBinaryStreamByCredentialUri(uriCredential.CredentialUri, Request.Files[0].InputStream, Request.Files[0].ContentType.SafeToString(contentType).SafeToString("application/octet-stream"));
                    Request.Files[0].InputStream.Close();

                    returnedObject = new
                    {
                        uri = uriCredential.StorageUri,
                        etag
                    };
                }
                else
                {
                    throw new InvalidObjectException("Uploaded File.");
                }
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { container, identifier, azureConnectionString });
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="azureConnectionString">The azure connection string.</param>
        /// <returns>System.Web.Mvc.JsonResult.</returns>
        public JsonResult DeleteBlob(string container, string identifier, string azureConnectionString)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                container.CheckEmptyString("container");
                identifier.CheckEmptyString("identifier");


                AzureStorageOperator manager = new AzureStorageOperator(azureConnectionString);
                manager.DeleteBlob(new BinaryStorageIdentifier
                {
                    Container = container,
                    Identifier = identifier
                });

                returnedObject = string.Empty;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new
                {
                    container,
                    identifier,
                    azureConnectionString
                });
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
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "AzureBlob", viewName);
        }
    }
}