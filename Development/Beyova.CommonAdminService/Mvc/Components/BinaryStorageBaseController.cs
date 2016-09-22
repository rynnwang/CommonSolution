using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova.AzureExtension;
using Beyova.CommonAdminService;
using Beyova.CommonServiceInterface;
using Beyova.ExceptionSystem;
using Beyova.RestApi;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class BinaryStorageBaseController.
    /// </summary>
    /// <typeparam name="TBinaryStorageObject">The type of the t binary storage object.</typeparam>
    /// <typeparam name="TBinaryStorageCriteria">The type of the t binary storage criteria.</typeparam>
    public abstract class BinaryStorageBaseController<TBinaryStorageObject, TBinaryStorageCriteria> : EnvironmentBaseController
        where TBinaryStorageObject : BinaryStorageMetaData, new()
        where TBinaryStorageCriteria : BinaryStorageMetaDataCriteria, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageBaseController{TBinaryStorageObject,TBinaryStorageCriteria}" /> class.
        /// </summary>
        public BinaryStorageBaseController() : base("BinaryStorage")
        {
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>IBinaryStorageService&lt;TBinaryStorageObject, TBinaryStorageCriteria&gt;.</returns>
        protected abstract IBinaryStorageService<TBinaryStorageObject, TBinaryStorageCriteria> GetClient(EnvironmentEndpoint endpoint);

        /// <summary>
        /// Gets the BLOB helper.
        /// </summary>
        /// <returns></returns>
        protected abstract ICloudBinaryStorageOperator GetCloudBinaryStorageOperator();

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Query()
        {
            return View(GetViewFullPath(Constants.ViewNames.BinaryStoragePanelView));
        }

        /// <summary>
        /// Queries the binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult QueryBinaryStorage(TBinaryStorageCriteria criteria, string environment)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                var endpoint = this.GetEnvironmentEndpoint(environment);
                var client = GetClient(endpoint);
                var meta = client.QueryBinaryStorage(criteria);

                return PartialView(GetViewFullPath(Constants.ViewNames.BinaryStorageListView), meta);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { criteria, environment });
            }
        }

        /// <summary>
        /// Downloads the binary storage.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult DownloadBinaryStorage(string container, string identifier, string environment)
        {
            try
            {
                identifier.CheckEmptyString("identifier");

                var endpoint = this.GetEnvironmentEndpoint(environment);
                var client = GetClient(endpoint);
                var credential = client.RequestBinaryDownloadCredential(new BinaryStorageIdentifier { Identifier = identifier, Container = container });

                return credential == null ? this.RenderAsNotFoundPage(string.Format("Binary [{0}] is not found", identifier)) : Redirect(credential.CredentialUri);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { identifier, environment });
            }
        }

        /// <summary>
        /// News the binary storage.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult NewBinaryStorage(string environment)
        {
            return View(GetViewFullPath(Constants.ViewNames.NewBinaryStorageView), model: environment);
        }

        /// <summary>
        /// Creates the upload credential.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult CreateUploadCredential(TBinaryStorageObject meta, string environment)
        {
            object returnedObject = null;
            BaseException exception = null;
            try
            {
                meta.CheckNullObject("meta");

                var endpoint = this.GetEnvironmentEndpoint(environment);
                var client = GetClient(endpoint);
                returnedObject = client.RequestBinaryUploadCredential(meta);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { meta, environment });
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Uploads the binary storage.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult UploadBinaryStorage(string container, string identifier, string contentType, int? width, int? height, int? duration, string environment)
        {
            object returnedObject = null;
            BaseException exception = null;
            TBinaryStorageObject meta = null;

            try
            {
                container.CheckEmptyString("container");

                meta = new TBinaryStorageObject
                {
                    Container = container,
                    Identifier = identifier,
                    Mime = contentType,
                    Width = width,
                    Height = height,
                    Duration = duration
                };

                if (Request.Files.Count > 0)
                {
                    var endpoint = this.GetEnvironmentEndpoint(environment);
                    var client = GetClient(endpoint);
                    var bytes = Request.Files[0].InputStream.ToBytes();
                    meta.Hash = bytes.ToBase64Md5();

                    var credential = client.RequestBinaryUploadCredential(meta);

                    this.GetCloudBinaryStorageOperator().UploadBinaryBytesByCredentialUri(credential.CredentialUri, bytes, contentType);

                    client.CommitBinaryStorage(new BinaryStorageCommitRequest
                    {
                        Identifier = credential.Identifier,
                        Container = credential.Container,
                        CommitOption = BinaryStorageCommitOption.ShareDuplicatedInstance
                    });

                    returnedObject = new
                    {
                        uri = credential.StorageUri
                    };
                }
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { meta, environment });
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Deletes the binary storage.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult DeleteBinaryStorage(string identifier, string environment)
        {
            object returnedObject = null;
            BaseException exception = null;

            try
            {
                identifier.CheckNullObject("identifier");

                var endpoint = this.GetEnvironmentEndpoint(environment);
                var client = GetClient(endpoint);
                client.DeleteBinaryStorage(identifier);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new
                {
                    identifier,
                    environment
                });
            }

            ApiHandlerBase.PackageResponse(this.Response, returnedObject, exception);
            return null;
        }

        /// <summary>
        /// Redirects the specified identifier.
        /// </summary>
        /// <param name="key">The identifier.</param>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        public ActionResult Redirect(string key, string environment)
        {
            try
            {
                key.CheckNullObject(nameof(key));

                var endpoint = this.GetEnvironmentEndpoint(environment);
                var client = GetClient(endpoint);
                var credential = client.RequestBinaryDownloadCredential(new BinaryStorageIdentifier { Identifier = key });

                credential.CheckNullObject(nameof(credential));

                return Redirect(credential.CredentialUri);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, new { key, environment });
            }
        }

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "BinaryStorage", viewName);
        }
    }
}