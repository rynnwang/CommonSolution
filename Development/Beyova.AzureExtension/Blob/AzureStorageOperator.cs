using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Beyova;
using Beyova.Api;
using Beyova.ExceptionSystem;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Beyova.AzureExtension
{
    /// <summary>
    /// Class AzureStorageOperator.
    /// </summary>
    public class AzureStorageOperator : CloudBinaryStorageOperator<CloudBlobContainer, CloudBlockBlob>
    {
        #region Fields

        /// <summary>
        /// The storage account
        /// </summary>
        internal CloudStorageAccount storageAccount;

        /// <summary>
        /// The BLOB client
        /// </summary>
        internal CloudBlobClient blobClient;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageOperator" /> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string. Example: Region=China;DefaultEndpointsProtocol=https;AccountName=YOURACCOUNTNAME;AccountKey=YOURACCOUNTKEY</param>
        public AzureStorageOperator(string storageConnectionString)
            : this(storageConnectionString.ConnectionStringToCredential())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageOperator"/> class.
        /// ApiEndpoint.Host=CustomizeHost;ApiEndpoint.Version=Region;ApiEndpoint.Account=AccountName,ApiEndpoint.Token=AccountKey;Prototol=https/http
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public AzureStorageOperator(ApiEndpoint endpoint)
            : this(endpoint.ConnectionStringToCredential())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageOperator" /> class.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="region">The region.</param>
        /// <param name="useHttps">The use HTTPS.</param>
        public AzureStorageOperator(StorageCredentials credential, AzureServiceProviderRegion region = AzureServiceProviderRegion.Global, bool useHttps = true)
            : this(new CloudStorageAccount(
                credential,
                AzureStorageExtension.GetStorageEndpointUri(useHttps, credential.AccountName, "blob", region),
                AzureStorageExtension.GetStorageEndpointUri(useHttps, credential.AccountName, "queue", region),
                AzureStorageExtension.GetStorageEndpointUri(useHttps, credential.AccountName, "table", region),
                AzureStorageExtension.GetStorageEndpointUri(useHttps, credential.AccountName, "file", region)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageOperator" /> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public AzureStorageOperator(CloudStorageAccount account)
        {
            storageAccount = account;
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Fetches the meta.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
        protected IDictionary<string, string> FetchMeta(string containerName, string blobIdentifier)
        {
            try
            {
                containerName.CheckEmptyString("containerName");
                blobIdentifier.CheckEmptyString("blobIdentifier");

                var container = blobClient.GetContainerReference(containerName);
                var blob = container.GetBlockBlobReference(blobIdentifier);
                blob.FetchAttributes();

                return blob.Metadata;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, blobIdentifier });
            }
        }

        /// <summary>
        /// Creates the BLOB URI.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="permission">The permission.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        /// <exception cref="InvalidObjectException">Container;null</exception>
        protected BinaryStorageActionCredential CreateBlobUri(string containerName, string blobIdentifier, int expireOffsetInMinute = 10, SharedAccessBlobPermissions permission = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                var container = blobClient.GetContainerReference(containerName);
                if (!container.Exists())
                {
                    throw new InvalidObjectException("Container", null, containerName);
                }

                return GenerateBlobUri(container, blobIdentifier, expireOffsetInMinute, permission);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, expireOffsetInMinute, permission });
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the container URI.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="permission">The permission.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="InvalidObjectException">Container;null</exception>
        public string CreateContainerUri(string containerName, int expireOffsetInMinute = 10, SharedAccessBlobPermissions permission = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                if (!container.Exists())
                {
                    throw new InvalidObjectException("Container", null, containerName);
                }

                return GenerateContainerUri(container, expireOffsetInMinute, permission);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, expireOffsetInMinute, permission });
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Downloads the BLOB.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="fetchAttribute">if set to <c>true</c> [fetch attribute].</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.Byte[].</returns>
        protected static byte[] DownloadBlob(CloudBlockBlob blob, bool fetchAttribute, out IDictionary<string, string> metaData)
        {
            try
            {
                blob.CheckNullObject("blob");

                if (fetchAttribute)
                {
                    blob.FetchAttributes();
                    metaData = blob.Metadata;
                }
                else
                {
                    metaData = null;
                }

                var msRead = new MemoryStream { Position = 0 };
                using (msRead)
                {
                    blob.DownloadToStream(msRead);
                    return msRead.ToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(blob.Uri.ToString());
            }
        }

        /// <summary>
        /// Gets the container sas URI.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="permission">The permission.</param>
        /// <returns>System.String.</returns>
        protected static string GenerateContainerUri(CloudBlobContainer container, int expireOffsetInMinute = 10, SharedAccessBlobPermissions permission = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read)
        {
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            var sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(expireOffsetInMinute);
            sasConstraints.Permissions = permission;

            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }

        /// <summary>
        /// Generates the BLOB URI.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="permission">The permission.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        protected static BinaryStorageActionCredential GenerateBlobUri(CloudBlobContainer container, string blobIdentifier, int expireOffsetInMinute = 10, SharedAccessBlobPermissions permission = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read)
        {
            var blob = container.GetBlockBlobReference(blobIdentifier);

            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            var sasConstraints = new SharedAccessBlobPolicy();
            var expiredStamp = DateTime.UtcNow.AddMinutes(expireOffsetInMinute);
            sasConstraints.SharedAccessExpiryTime = expiredStamp;
            sasConstraints.Permissions = permission;

            string sasContainerToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return new BinaryStorageActionCredential()
            {
                CredentialUri = blob.Uri + sasContainerToken,
                StorageUri = blob.Uri.ToString(),
                Container = container.Name,
                Identifier = blobIdentifier,
                CredentialExpiredStamp = expiredStamp
            };
        }

        /// <summary>
        /// Creates the shared access BLOB policy.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>SharedAccessBlobPolicy.</returns>
        protected static SharedAccessBlobPolicy CreateSharedAccessBlobPolicy(SharedAccessBlobPermissions permissions, int expireOffsetInMinute = 10)
        {
            return new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(expireOffsetInMinute),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List
            };
        }

        /// <summary>
        /// Applies the BLOB policy.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="permissions">The permissions.</param>
        protected static void ApplyBlobPolicy(CloudBlobContainer container, string policyName, int expireOffsetInMinute = 10, SharedAccessBlobPermissions permissions = SharedAccessBlobPermissions.List)
        {
            container.CheckNullObject("container");
            policyName.CheckEmptyString("policyName");

            //Get the container's existing permissions.
            var containerPermissions = new BlobContainerPermissions();

            var sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(expireOffsetInMinute),
                Permissions = permissions
            };

            //Add the new policy to the container's permissions.
            containerPermissions.SharedAccessPolicies.Clear();
            containerPermissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            container.SetPermissions(containerPermissions);
        }

        #endregion

        #region Service Properties

        /// <summary>
        /// Sets the service properties.
        /// </summary>
        /// <param name="serviceProperty">The service property.</param>
        public void SetServiceProperties(ServiceProperties serviceProperty)
        {
            try
            {
                serviceProperty.CheckNullObject("serviceProperty");
                this.blobClient.SetServiceProperties(serviceProperty);
            }
            catch (Exception ex)
            {
                throw ex.Handle(serviceProperty);
            }
        }

        /// <summary>
        /// Gets the service properties.
        /// </summary>
        /// <returns>Microsoft.WindowsAzure.Storage.Shared.Protocol.ServiceProperties.</returns>
        public ServiceProperties GetServiceProperties()
        {
            try
            {
                return this.blobClient.GetServiceProperties();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Sets the service version to 2013-08-15.
        /// </summary>
        public void SetServiceVersionTo20130815()
        {
            try
            {
                var p = GetServiceProperties();
                p.DefaultServiceVersion = "2013-08-15";
                SetServiceProperties(p);
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion

        #region CloudBinaryStorageOperator interfaces

        /// <summary>
        /// Creates the BLOB upload credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">The hash. (this value would be ignored.)</param>
        /// <param name="contentType">Type of the content. This value is used only when blob service provider needs to set content type (MIME) when creating credential of upload action.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public override BinaryStorageActionCredential CreateBlobUploadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute, string hash = null, string contentType = null)
        {
            return CreateBlobUri(containerName, blobIdentifier.SafeToString(Guid.NewGuid().ToString()), expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write);
        }

        /// <summary>
        /// Creates the BLOB download credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public override BinaryStorageActionCredential CreateBlobDownloadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute)
        {
            return CreateBlobUri(containerName, blobIdentifier, expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read);
        }

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public override void DeleteBlob(BinaryStorageIdentifier storageIdentifier)
        {
            try
            {
                storageIdentifier.CheckNullObject("storageIdentifier");
                storageIdentifier.Container.CheckEmptyString("storageIdentifier.Container");
                storageIdentifier.Identifier.CheckEmptyString("storageIdentifier.Identifier");

                if (Exists(storageIdentifier))
                {
                    var container = blobClient.GetContainerReference(storageIdentifier.Container);
                    var blob = container.GetBlockBlobReference(storageIdentifier.Identifier);
                    blob?.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(storageIdentifier);
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>List&lt;BinaryStorageMetaBase&gt;.</returns>
        public override List<BinaryStorageMetaData> QueryBinaryBlobByContainer(string containerName, string contentType, string md5,
            long? length, int limitCount = 100)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                var blobs = QueryBlob(this.blobClient.GetContainerReference(containerName), contentType, md5, length, limitCount);
                return (from i in blobs
                        select new BinaryStorageMetaData
                        {
                            Container = containerName,
                            Identifier = i.Name,
                            Mime = i.Properties.ContentType,
                            Name = i.Properties.ContentDisposition.ConvertContentDispositionToName(),
                            Length = i.Properties.Length,
                            Hash = i.Properties.ContentMD5
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, limitCount });
            }
        }

        /// <summary>
        /// Fetches the cloud meta. Returned object would only includes (md5, length, name, content type).
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Beyova.BinaryStorageMetaData.</returns>
        public override BinaryStorageMetaData FetchCloudMeta(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                var container = blobClient.GetContainerReference(identifier.Container);
                var blob = container.GetBlockBlobReference(identifier.Identifier);
                blob.FetchAttributes();

                return new BinaryStorageMetaData(identifier)
                {
                    Hash = blob.Properties.ContentMD5,
                    Length = blob.Properties.Length,
                    Name = blob.Properties.ContentDisposition,
                    Mime = blob.Properties.ContentType
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>System.Collections.Generic.List&lt;System.String&gt;.</returns>
        public override List<string> GetContainers()
        {
            var containers = blobClient.ListContainers();
            return (from one in containers select one.Name).ToList();
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
        public override IEnumerable<CloudBlockBlob> QueryBlob(CloudBlobContainer container, string contentType, string md5, long? length, int limitCount)
        {
            try
            {
                container.CheckNullObject("container");
                if (string.IsNullOrWhiteSpace(contentType) && string.IsNullOrWhiteSpace(md5) && length == null)
                {
                    return container.ListBlobs().OfType<CloudBlockBlob>().Take(limitCount);
                }
                else
                {
                    return container.ListBlobs(blobListingDetails: BlobListingDetails.Metadata)
                        .OfType<CloudBlockBlob>()
                        .Where(item =>
                        {
                            return ((string.IsNullOrWhiteSpace(contentType) ||
                                     item.Properties.ContentType.Equals(contentType,
                                         StringComparison.InvariantCultureIgnoreCase))
                                    &&
                                    (string.IsNullOrWhiteSpace(md5) ||
                                     item.Properties.ContentMD5.Equals(md5,
                                         StringComparison.InvariantCultureIgnoreCase))
                                    && (length == null || item.Properties.Length == length.Value));
                        }).Take(limitCount);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { container = container == null ? null : container.Uri.ToString(), contentType, md5, length, limitCount });
            }
        }

        /// <summary>
        /// Check Existence of the specified identifier.
        /// </summary>
        /// <param name="identifier">The storage identifier.</param>
        /// <returns><c>true</c> if existed, <c>false</c> otherwise.</returns>
        public override bool Exists(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject("identifier");

                if (!string.IsNullOrWhiteSpace(identifier.Container))
                {
                    var container = blobClient.GetContainerReference(identifier.Container);
                    CloudBlockBlob blob = null;

                    if (!string.IsNullOrEmpty(identifier.Identifier))
                    {
                        blob = container.GetBlockBlobReference(identifier.Identifier);
                    }

                    return blob != null && blob.Exists();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }

            return false;
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;CloudBlockBlob&gt;.</returns>
        public override IEnumerable<CloudBlockBlob> QueryBlob(string containerName, string contentType, string md5, long? length, int limitCount = 100)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                return QueryBlob(this.blobClient.GetContainerReference(containerName), contentType, md5, length, limitCount);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, limitCount });
            }
        }

        /// <summary>
        /// Uploads the binary stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public override string UploadBinaryStreamByCredentialUri(string blobUri, Stream stream, string contentType, string fileName = null)
        {
            try
            {
                blobUri.CheckEmptyString("blobUri");
                stream.CheckNullObject("stream");

                var metaData = new BinaryStorageMetaData { Mime = contentType.SafeToString(HttpConstants.ContentType.BinaryDefault), Name = fileName };
                var blob = new CloudBlockBlob(new Uri(blobUri));
                blob.Properties.FillMeta(metaData);

                stream.Position = 0;
                blob.UploadFromStream(stream);

                return blob.Properties.ETag;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { blobUri, contentType, fileName });
            }
        }

        /// <summary>
        /// Uploads the binary bytes by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public override string UploadBinaryBytesByCredentialUri(string blobUri, byte[] dataBytes, string contentType, string fileName = null)
        {
            try
            {
                blobUri.CheckEmptyString("blobUri");
                dataBytes.CheckNullObject("dataBytes");

                using (var stream = dataBytes.ToStream())
                {
                    return UploadBinaryStreamByCredentialUri(blobUri, stream, contentType, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { blobUri, contentType, fileName });
            }
        }

        /// <summary>
        /// Downloads the binary bytes by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.Byte[].</returns>
        public override byte[] DownloadBinaryBytesByCredentialUri(string blobUri)
        {
            try
            {
                using (var stream = DownloadBinaryStreamByCredentialUri(blobUri))
                {
                    return stream.ToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(blobUri);
            }
        }

        /// <summary>
        /// Downloads the binary stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.IO.Stream.</returns>
        public override Stream DownloadBinaryStreamByCredentialUri(string blobUri)
        {
            try
            {
                blobUri.CheckEmptyString("blobUri");

                var blob = new CloudBlockBlob(new Uri(blobUri));
                //blobProperties = blob.Properties;

                blob.CheckNullObject("blob");

                var stream = new MemoryStream { Position = 0 };

                blob.DownloadToStream(stream);
                return stream;
            }
            catch (Exception ex)
            {
                throw ex.Handle(blobUri);
            }
        }

        #endregion
    }
}
