using System;
using System.Collections.Generic;
using System.Linq;
using Beyova.AzureExtension;
using Beyova;
using Beyova.BinaryStorage;
using Beyova.CommonService.DataAccessController;

namespace Beyova.CommonService
{
    /// <summary>
    /// Class AzureBinaryStorageHelper.
    /// </summary>
    public class AzureBinaryStorageHelper<TResource, TResourceCriteria, TDataAccessController>
        where TResource : BinaryStorageMetaData, new()
        where TResourceCriteria : BinaryStorageMetaDataCriteria, new()
        where TDataAccessController : BinaryStorageMetaDataBaseAccessController<TResource, TResourceCriteria>, new()
    {
        /// <summary>
        /// Gets or sets the authentication expire offset.
        /// </summary>
        /// <value>The authentication expire offset.</value>
        public int AuthenticationExpireOffset
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the azure storage manager.
        /// </summary>
        /// <value>The azure storage manager.</value>
        public AzureStorageManager AzureStorageManager
        {
            get; protected set;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AzureBinaryStorageHelper" /> class from being created.
        /// <remarks>
        /// It would try to load configuration <c>AzureStorageConnectionString</c>.
        /// </remarks>
        /// </summary>
        /// <param name="azureStorageConnectionString">The azure storage connection string.</param>
        /// <param name="credentialExpireOffset">The credential expire offset.</param>
        public AzureBinaryStorageHelper(string azureStorageConnectionString, int credentialExpireOffset = 30)
        {
            if (!string.IsNullOrWhiteSpace(azureStorageConnectionString))
            {
                this.AzureStorageManager = new AzureStorageManager(azureStorageConnectionString);
            }
        }

        /// <summary>
        /// Gets the upload credential.
        /// <remarks>
        /// This method is to create meta in database and get authenticated uri for client to upload blob.
        /// Before you call <c>CommitBinaryStorage</c> to set state as <c>Committed</c>, the state of blob is <c>CommitPending</c>, and might be deleted by maintenance script.
        /// And, the uri can be expired according to service security settings. By default, it is 10 min since created stamp.
        /// </remarks>
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="expireOffset">The expire offset.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential GetUploadCredential(BinaryStorageMetaData meta, int? expireOffset = null)
        {
            try
            {
                meta.CheckNullObject("meta");
                meta.Container.CheckEmptyString("meta.Container");

                meta.OwnerKey = ContextHelper.GetCurrentOperatorKey();
                meta.State = BinaryStorageState.CommitPending;

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    meta = controller.CreateOrUpdateBinaryStorageMetaData(meta, ContextHelper.GetCurrentOperatorKey());
                    return AzureStorageManager.CreateBlobUriForUpload(meta.Container, meta.Identifier,
                        expireOffset ?? AuthenticationExpireOffset);

                };
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetUploadCredential", meta);
            }
        }

        /// <summary>
        /// Gets the download credential.
        /// <remarks>
        /// This method is to get authenticated uri for client to download blob.
        /// Only when the blob information can be found in database and the state is valid for download, a Uri would be returned.
        /// And, the uri can be expired according to service security settings. By default, it is 10 min since created stamp.
        /// </remarks>
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="expireOffset">The expire offset.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential GetDownloadCredential(BinaryStorageIdentifier identifier, int? expireOffset = null)
        {
            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    var binary = controller.QueryBinaryStorageMetaData(new BinaryStorageMetaDataCriteria
                    {
                        Container = identifier.Container,
                        Identifier = identifier.Identifier
                    }).FirstOrDefault();

                    if (binary != null)
                    {
                        return AzureStorageManager.CreateBlobUriDownload(binary.Container, binary.Identifier, expireOffset ?? AuthenticationExpireOffset);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetDownloadCredential", identifier);
            }
        }

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="identifier">The meta.</param>
        public void DeleteData(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject("meta");
                identifier.Container.CheckEmptyString("meta.Container");
                identifier.Identifier.CheckNullObject("meta.Identifier");

                var meta = new BinaryStorageMetaData
                {
                    OwnerKey = ContextHelper.GetCurrentOperatorKey(),
                    State = BinaryStorageState.DeletePending,
                    Identifier = identifier.Identifier,
                    Container = identifier.Container
                };

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    identifier = controller.CreateOrUpdateBinaryStorageMetaData(meta, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("DeleteData", identifier);
            }
        }

        /// <summary>
        /// Queries the binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        public List<BinaryStorageMetaData> QueryBinaryStorage(BinaryStorageMetaDataCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                criteria.Container.CheckEmptyString("criteria.Container");

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    return controller.QueryBinaryStorageMetaData(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryBinaryStorage", criteria);
            }
        }

        /// <summary>
        /// Fills the binary storage meta data.
        /// <remarks>This method is used when external service can provide only Container and Identifier, but need all meta info.</remarks>
        /// </summary>
        /// <param name="metaConnection">The meta connection.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        public List<BinaryStorageMetaData> FillBinaryStorageMetaData(List<BinaryStorageMetaData> metaConnection)
        {
            try
            {
                metaConnection.CheckNullObject("metaConnection");

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    return controller.GetBinaryStorageMetaDataByIdentifiers(metaConnection);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryBinaryStorage", metaConnection);
            }
        }

        public string MergeBinaryStorageObject(BinaryStorageMetaData meta, IEnumerable<string> identifiers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the binary storage object.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageObject.</returns>
        public BinaryStorageObject GetBinaryStorageObject(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckNullObject("identifier.Identifier");

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    var binaryMeta = controller.QueryBinaryStorageMetaData(new BinaryStorageMetaDataCriteria
                    {
                        Container = identifier.Container,
                        Identifier = identifier.Identifier
                    }).FirstOrDefault();

                    if (binaryMeta != null)
                    {
                        var downloadUri = AzureStorageManager.CreateBlobUriDownload(binaryMeta.Container, binaryMeta.Identifier);
                        var data = AzureStorageManager.DownloadBlobByBlobUri(downloadUri.CredentialUri);

                        return new BinaryStorageObject(binaryMeta) { DataInBytes = data };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetBinaryStorageObject", identifier);
            }
        }

        /// <summary>
        /// Puts the binary storage object.
        /// <remarks>Only <c>Container</c>, <c>Identifier</c> and <c>Data</c> would take effect is this method.</remarks>
        /// </summary>
        /// <param name="storageObject">The storage object.</param>
        /// <returns>ETag value.</returns>
        public string PutBinaryStorageObject(BinaryStorageObject storageObject)
        {
            try
            {
                storageObject.CheckNullObject("meta");
                storageObject.Container.CheckEmptyString("meta.Container");
                storageObject.Identifier.CheckNullObject("meta.Identifier");

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    var binaryMeta = controller.QueryBinaryStorageMetaData(new BinaryStorageMetaDataCriteria
                    {
                        Container = storageObject.Container,
                        Identifier = storageObject.Identifier
                    }).FirstOrDefault();

                    if (binaryMeta != null)
                    {
                        var uploadUri = AzureStorageManager.CreateBlobUriForUpload(binaryMeta.Container, binaryMeta.Identifier);
                        var eTag = AzureStorageManager.UploadBlobByBlobUri(uploadUri.CredentialUri, storageObject.DataInBytes, storageObject, ToMetaDataCollection(storageObject));

                        return eTag;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("PutBinaryStorageObject", storageObject as BinaryStorageMetaData);
            }
        }

        /// <summary>
        /// Commits the binary storage.
        /// <remarks>
        /// This method would try to update state of binary storage to <c>Committed</c>.
        /// </remarks>
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageMetaData.</returns>
        public BinaryStorageMetaData CommitBinaryStorage(BinaryStorageIdentifier identifier)
        {
            BinaryStorageMetaData result = null;

            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                try
                {
                    string eTag, hash, name;
                    long length;

                    result = new BinaryStorageMetaData
                    {
                        Identifier = identifier.Identifier,
                        Container = identifier.Container,
                        Mime = AzureStorageManager.FetchProperty(identifier.Container, identifier.Identifier, out eTag,
                            out hash, out length, out name),
                        Hash = hash,
                        Name = name,
                        State = BinaryStorageState.Committed
                    };
                }
                catch
                {
                    if (result != null)
                    {
                        result.State = BinaryStorageState.Invalid;
                    }
                }

                using (var controller = new BinaryStorageMetaDataAccessController())
                {
                    return controller.CreateOrUpdateBinaryStorageMetaData(result, ContextHelper.GetCurrentOperatorKey());
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle("CommitBinaryStorage", identifier);
            }
        }

        /// <summary>
        /// To the meta data collection.
        /// </summary>
        /// <param name="storageObject">The storage object.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> ToMetaDataCollection(BinaryStorageObject storageObject)
        {
            var meta = new Dictionary<string, string>();

            meta.FillMeta(storageObject);

            if (storageObject != null && storageObject.DataInBytes != null)
            {
                meta.Merge("Content-Length", storageObject.DataInBytes.Length.ToString());
            }

            return meta;
        }

        /// <summary>
        /// To the meta data collection.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> ToMetaDataCollection(BinaryStorageMetaData metaData)
        {
            var meta = new Dictionary<string, string>();
            meta.FillMeta(metaData);

            return meta;
        }

        /// <summary>
        /// To the binary storage meta data.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        /// <returns>Beyova.BinaryStorage.BinaryStorageMetaData.</returns>
        public BinaryStorageMetaData ToBinaryStorageMetaData(Dictionary<string, string> metaData)
        {
            var meta = new BinaryStorageMetaData();
            meta.FillMeta(metaData);

            return meta;
        }

        /// <summary>
        /// Gets the binary capacity summary.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>BinaryCapacitySummary.</returns>
        public BinaryCapacitySummary GetBinaryCapacitySummary(BinaryCapacityCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");

                using (var controller = new BinaryCapacitySummaryDataAccessController())
                {
                    return controller.GetBinaryCapacitySummary(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetBinaryCapacitySummary", criteria);
            }
        }
    }
}
