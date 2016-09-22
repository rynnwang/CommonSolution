using System;
using System.Collections.Generic;
using System.Linq;
using Beyova;
using Beyova.CommonService.DataAccessController;
using Beyova.Api;
using System.IO;

namespace Beyova.CommonService
{
    /// <summary>
    /// Class AzureBinaryStorageHelper.
    /// </summary>
    /// <typeparam name="TCloudContainer">The type of the t cloud container.</typeparam>
    /// <typeparam name="TCloudBlobObject">The type of the t cloud BLOB object.</typeparam>
    /// <typeparam name="TBinaryStorageMetaData">The type of the t binary storage meta data.</typeparam>
    /// <typeparam name="TBinaryStorageCriteria">The type of the t binary storage criteria.</typeparam>
    /// <typeparam name="TDataAccessController">The type of the t data access controller.</typeparam>
    public abstract class BinaryStorageHelper<TCloudContainer, TCloudBlobObject, TBinaryStorageMetaData, TBinaryStorageCriteria, TDataAccessController>
        : IBinaryStorageHelper<TCloudContainer, TCloudBlobObject, TBinaryStorageMetaData, TBinaryStorageCriteria>
        where TBinaryStorageMetaData : BinaryStorageMetaData, new()
        where TBinaryStorageCriteria : BinaryStorageMetaDataCriteria, new()
        where TDataAccessController : BinaryStorageMetaDataBaseAccessController<TBinaryStorageMetaData, TBinaryStorageCriteria>, new()
    {
        /// <summary>
        /// The cloud binary storage operator
        /// </summary>
        protected CloudBinaryStorageOperator<TCloudContainer, TCloudBlobObject> cloudBinaryStorageOperator;

        /// <summary>
        /// Gets the credential expire offset.
        /// </summary>
        /// <value>The credential expire offset.</value>
        protected virtual int CredentialExpireOffset
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageHelper{TCloudContainer, TCloudBlobObject, TBinaryStorageMetaData, TBinaryStorageCriteria, TDataAccessController}" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        protected BinaryStorageHelper(ApiEndpoint endpoint)
        {
            cloudBinaryStorageOperator = GetCloudBinaryStorageOperator(endpoint);
        }

        /// <summary>
        /// Gets the cloud binary storage operator.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>CloudBinaryStorageOperator.</returns>
        protected abstract CloudBinaryStorageOperator<TCloudContainer, TCloudBlobObject> GetCloudBinaryStorageOperator(ApiEndpoint endpoint);

        /// <summary>
        /// Creates binary upload credential
        /// <remarks>
        /// This method is to create meta in database and get authenticated uri for client to upload blob.
        /// Before you call <c>CommitBinaryStorage</c> to set state as <c>Committed</c>, the state of blob is <c>CommitPending</c>, and might be deleted by maintenance script.
        /// And, the uri can be expired according to service security settings. By default, it is 10 min since created stamp.
        /// </remarks>
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="expireOffset">The expire offset.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBinaryUploadCredential(TBinaryStorageMetaData meta, int? expireOffset = null)
        {
            try
            {
                meta.CheckNullObject("meta");
                meta.Container.CheckEmptyString("meta.Container");

                meta.Identifier = meta.Identifier.SafeToString(Guid.NewGuid().ToString());

                using (var controller = new TDataAccessController())
                {
                    var createdMeta = controller.CreateBinaryStorageMetaData(meta, ContextHelper.GetCurrentOperatorKey());
                    return cloudBinaryStorageOperator.CreateBlobUploadCredential(createdMeta.Container, createdMeta.Identifier, expireOffset ?? 10, meta.Hash, meta.Mime);
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(meta);
            }
        }

        /// <summary>
        /// Creates binary download credential
        /// <remarks>
        /// This method is to get authenticated uri for client to download blob.
        /// Only when the blob information can be found in database and the state is valid for download, a Uri would be returned.
        /// And, the uri can be expired according to service security settings. By default, it is 10 min since created stamp.
        /// </remarks>
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="expireOffset">The expire offset.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBinaryDownloadCredential(BinaryStorageIdentifier identifier, int? expireOffset = null)
        {
            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                using (var controller = new TDataAccessController())
                {
                    var binary = controller.QueryBinaryStorageMetaData(new TBinaryStorageCriteria
                    {
                        Container = identifier.Container,
                        Identifier = identifier.Identifier
                    }).FirstOrDefault();

                    return binary != null ? cloudBinaryStorageOperator.CreateBlobDownloadCredential(binary.Container, binary.Identifier, expireOffset ?? 10) : null;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public void DeleteBinaryStorage(Guid? identifier)
        {
            try
            {
                identifier.CheckNullObject("identifier");

                using (var controller = new TDataAccessController())
                {
                    controller.DeleteBinaryStorage(identifier, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Commits the binary storage.
        /// <remarks>
        /// This method would try to update state of binary storage to <c>Committed</c>.
        /// </remarks></summary>
        /// <param name="request">The request.</param>
        /// <returns>BinaryStorageMetaData.</returns>
        public TBinaryStorageMetaData CommitBinaryStorage(BinaryStorageCommitRequest request)
        {
            try
            {
                request.CheckNullObject("request");
                request.Container.CheckEmptyString("request.Container");
                request.Identifier.CheckEmptyString("request.Identifier");

                var meta = cloudBinaryStorageOperator.FetchCloudMeta(request);
                meta.CheckNullObject("meta");

                using (var controller = new TDataAccessController())
                {
                    return controller.CommitBinaryStorage(request, meta.Mime, meta.Hash, meta.Length ?? 0, ContextHelper.GetCurrentOperatorKey());
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        /// <summary>
        /// Queries the binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;TBinaryStorageMetaData&gt;.</returns>
        public List<TBinaryStorageMetaData> QueryBinaryStorage(TBinaryStorageCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                using (var controller = new TDataAccessController())
                {
                    return controller.QueryBinaryStorageMetaData(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>System.Collections.Generic.IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetContainers()
        {
            try
            {
                return cloudBinaryStorageOperator.GetContainers();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Creates the BLOB download credential. This is blob operation only, no database involved.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobDownloadCredential(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                return cloudBinaryStorageOperator.CreateBlobDownloadCredential(identifier.Container, identifier.Identifier, CredentialExpireOffset);
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Creates the BLOB upload credential. This is blob operation only, no database involved.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="hash">The hash. This value is used only when blob service provider needs to set hash (MD5) when creating credential of upload action.</param>
        /// <param name="contentType">Type of the content. This value is used only when blob service provider needs to set content type (MIME) when creating credential of upload action.</param>
        /// <returns>Beyova.BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobUploadCredential(BinaryStorageIdentifier identifier, string hash = null, string contentType = null)
        {
            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                return cloudBinaryStorageOperator.CreateBlobUploadCredential(identifier.Container, identifier.Identifier, CredentialExpireOffset, hash, contentType);
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Uploads the binary by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>System.String.</returns>
        public string UploadBinaryByCredential(BinaryStorageActionCredential credential, byte[] data, string contentType)
        {
            try
            {
                credential.CheckNullObject("credential");
                data.CheckNullObject("data");

                return cloudBinaryStorageOperator.UploadBinaryBytesByCredentialUri(credential.CredentialUri, data, contentType);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { credential, contentType });
            }
        }

        /// <summary>
        /// Downloads the binary stream by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>System.IO.Stream.</returns>
        public Stream DownloadBinaryStreamByCredential(BinaryStorageActionCredential credential)
        {
            try
            {
                credential.CheckNullObject("credential");

                return cloudBinaryStorageOperator.DownloadBinaryStreamByCredentialUri(credential.CredentialUri);
            }
            catch (Exception ex)
            {
                throw ex.Handle(credential);
            }
        }

        /// <summary>
        /// Downloads the binary bytes by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] DownloadBinaryBytesByCredential(BinaryStorageActionCredential credential)
        {
            try
            {
                credential.CheckNullObject("credential");

                return cloudBinaryStorageOperator.DownloadBinaryBytesByCredentialUri(credential.CredentialUri);
            }
            catch (Exception ex)
            {
                throw ex.Handle(credential);
            }
        }

        /// <summary>
        /// Deletes the binary.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public void DeleteBinary(BinaryStorageIdentifier identifier)
        {
            try
            {
                cloudBinaryStorageOperator.DeleteBlob(identifier);
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;TCloudBlobObject&gt;.</returns>
        public IEnumerable<TCloudBlobObject> QueryBlob(TCloudContainer container, string contentType, string md5, long? length, int limitCount)
        {
            try
            {
                return cloudBinaryStorageOperator.QueryBlob(container, contentType, md5, length, limitCount);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { contentType, md5, length, limitCount });
            }
        }

        /// <summary>
        /// Gets the binary storage meta data by identifiers.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>System.Collections.Generic.List&lt;TBinaryStorageMetaData&gt;.</returns>
        public List<TBinaryStorageMetaData> GetBinaryStorageMetaDataByIdentifiers(List<BinaryStorageIdentifier> identifiers)
        {
            try
            {
                identifiers.CheckNullObject("identifiers");

                using (var controller = new TDataAccessController())
                {
                    return controller.GetBinaryStorageMetaDataByIdentifiers(identifiers);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifiers);
            }
        }

        /// <summary>
        /// Queries the user binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TBinaryStorageMetaData&gt;.</returns>
        public List<TBinaryStorageMetaData> QueryUserBinaryStorage(TBinaryStorageCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                using (var controller = new TDataAccessController())
                {
                    return controller.QueryUserBinaryStorageMetaData(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }
    }
}
