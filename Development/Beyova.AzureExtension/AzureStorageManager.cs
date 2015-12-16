using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Beyova;
using Beyova.BinaryStorage;
using Beyova.ExceptionSystem;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Beyova.AzureExtension
{
    /// <summary>
    /// Class AzureStorageManager.
    /// </summary>
    public class AzureStorageManager
    {
        #region Fields

        /// <summary>
        /// The storage account
        /// </summary>
        protected CloudStorageAccount storageAccount;

        /// <summary>
        /// The BLOB client
        /// </summary>
        protected CloudBlobClient blobClient;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageManager" /> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string. Example: Region=China;DefaultEndpointsProtocol=https;AccountName=YOURACCOUNTNAME;AccountKey=YOURACCOUNTKEY</param>
        public AzureStorageManager(string storageConnectionString)
            : this(ConnectionStringToCredential(storageConnectionString))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageManager" /> class.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="region">The region.</param>
        /// <param name="useHttps">The use HTTPS.</param>
        public AzureStorageManager(StorageCredentials credential, AzureServiceProviderRegion region = AzureServiceProviderRegion.Global, bool useHttps = true)
            : this(new CloudStorageAccount(
                credential,
                GetStorageEndpointUri(useHttps, credential.AccountName, "blob", region),
                GetStorageEndpointUri(useHttps, credential.AccountName, "queue", region),
                GetStorageEndpointUri(useHttps, credential.AccountName, "table", region),
                GetStorageEndpointUri(useHttps, credential.AccountName, "file", region)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageManager" /> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public AzureStorageManager(CloudStorageAccount account)
        {
            storageAccount = account;
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the container list.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetContainerList()
        {
            var containers = blobClient.ListContainers();
            return (from one in containers select one.Name).ToList();
        }

        /// <summary>
        /// Fetches the meta.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
        public IDictionary<string, string> FetchMeta(string containerName, string blobIdentifier)
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
                throw ex.Handle("FetchMeta", new { containerName, blobIdentifier });
            }
        }

        /// <summary>
        /// Fetches the property.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="eTag">The etag.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public string FetchProperty(string containerName, string blobIdentifier, out string eTag, out string md5, out long length, out string name)
        {
            try
            {
                containerName.CheckEmptyString("containerName");
                blobIdentifier.CheckEmptyString("blobIdentifier");

                var container = blobClient.GetContainerReference(containerName);
                var blob = container.GetBlockBlobReference(blobIdentifier);
                blob.FetchAttributes();

                eTag = blob.Properties.ETag;
                md5 = blob.Properties.ContentMD5;
                length = blob.Properties.Length;
                name = blob.Properties.ContentDisposition;

                return blob.Properties.ContentType;
            }
            catch (Exception ex)
            {
                throw ex.Handle("FetchProperty", new { containerName, blobIdentifier });
            }
        }

        /// <summary>
        /// Creates the container URI for upload.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>System.String.</returns>
        public string CreateContainerUriForUpload(string containerName, int expireOffsetInMinute = 10)
        {
            return CreateContainerUri(containerName, expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write);
        }

        /// <summary>
        /// Creates the container URI for download.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>System.String.</returns>
        public string CreateContainerUriForDownload(string containerName, int expireOffsetInMinute = 10)
        {
            return CreateContainerUri(containerName, expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read);
        }

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
                throw ex.Handle("CreateContainerUri", new { containerName, expireOffsetInMinute, permission });
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
                throw ex.Handle("CreateBlobUri", new { containerName, expireOffsetInMinute, permission });
            }
        }

        /// <summary>
        /// Creates the BLOB URI for upload.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobUriForUpload(string containerName, string blobIdentifier, int expireOffsetInMinute = 10)
        {
            if (blobIdentifier.IsNullOrWhiteSpace())
            {
                blobIdentifier = Guid.NewGuid().ToString();
            }
            return CreateBlobUri(containerName, blobIdentifier, expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write);
        }

        /// <summary>
        /// Creates the BLOB URI for upload.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobUriForUpload(string containerName, Guid? blobIdentifier, int expireOffsetInMinute = 10)
        {
            return CreateBlobUri(containerName, (blobIdentifier ?? Guid.NewGuid()).ToString(), expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write);
        }

        /// <summary>
        /// Creates the container URI for download.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobUriDownload(string containerName, Guid blobIdentifier, int expireOffsetInMinute = 10)
        {
            return CreateBlobUri(containerName, blobIdentifier.ToString(), expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read);
        }

        /// <summary>
        /// Creates the BLOB URI download.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobUriDownload(string containerName, string blobIdentifier, int expireOffsetInMinute = 10)
        {
            return CreateBlobUri(containerName, blobIdentifier, expireOffsetInMinute, SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read);
        }

        /// <summary>
        /// Creates the BLOB URI download.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential CreateBlobUriDownload(BinaryStorageIdentifier storageIdentifier, int expireOffsetInMinute = 10)
        {
            storageIdentifier.CheckNullObject("storageIdentifier");
            return CreateBlobUriDownload(storageIdentifier.Container, storageIdentifier.Identifier, expireOffsetInMinute);
        }

        /// <summary>
        /// Gets the storage URL.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <returns>System.String.</returns>
        public string GetStorageUrl(BinaryStorageIdentifier storageIdentifier)
        {
            return storageIdentifier == null ? string.Empty : GetStorageUrl(storageIdentifier.Container, storageIdentifier.Identifier);
        }

        /// <summary>
        /// Gets the storage URL.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <returns>System.String.</returns>
        public string GetStorageUrl(string containerName, string blobIdentifier)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(containerName))
                {
                    var container = blobClient.GetContainerReference(containerName);
                    CloudBlockBlob blob = null;

                    if (!string.IsNullOrEmpty(blobIdentifier))
                    {
                        blob = container.GetBlockBlobReference(blobIdentifier);
                    }

                    return blob != null ? blob.Uri.ToString() : container.Uri.ToString();
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetStorageUrl", new { blobIdentifier, containerName });
            }
        }
        /// <summary>
        /// Exists the specified storage identifier.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public bool Exists(BinaryStorageIdentifier storageIdentifier)
        {
            return storageIdentifier != null && Exists(storageIdentifier.Container, storageIdentifier.Identifier);
        }

        /// <summary>
        /// Exists the specified container name.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public bool Exists(string containerName, string blobIdentifier)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(containerName))
                {
                    var container = blobClient.GetContainerReference(containerName);
                    CloudBlockBlob blob = null;

                    if (!string.IsNullOrEmpty(blobIdentifier))
                    {
                        blob = container.GetBlockBlobReference(blobIdentifier);
                    }

                    return blob != null && blob.Exists();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("Exists", new { blobIdentifier, containerName });
            }

            return false;
        }

        #endregion

        #region Protected methods

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

        #region Upload Blob By Uri

        /// <summary>
        /// Uploads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>System.String.</returns>
        public static string UploadBlobByBlobUri(string blobUri, byte[] dataBytes, string contentType)
        {
            try
            {
                return UploadBlobByBlobUri(blobUri, dataBytes, new BinaryStorageMetaData { Mime = contentType });
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlobByBlobUri", new { blobUri, contentType });
            }
        }

        /// <summary>
        /// Uploads the BLOB.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String for ETag.</returns>
        public static string UploadBlob(CloudBlockBlob blob, byte[] dataBytes, IDictionary<string, string> metaData)
        {
            try
            {
                if (metaData != null)
                {
                    foreach (var key in metaData.Keys)
                    {
                        blob.Metadata.Merge(key, metaData[key]);
                    }
                }

                var msWrite = new MemoryStream(dataBytes);
                msWrite.Position = 0;
                using (msWrite)
                {
                    blob.UploadFromStream(msWrite);
                }

                return blob.Properties.ETag;
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlob", new { blob = blob.Uri.ToString(), metaData = metaData });
            }
        }

        /// <summary>
        /// Uploads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String for ETag.</returns>
        public static string UploadBlobByBlobUri(string blobUri, byte[] dataBytes, BinaryStorageMetaBase meta = null, IDictionary<string, string> metaData = null)
        {
            try
            {
                blobUri.CheckEmptyString("blobUri");

                var blob = new CloudBlockBlob(new Uri(blobUri));
                blob.Properties.FillMeta(meta);

                return UploadBlob(blob, dataBytes, metaData);
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlobByBlobUri", new { blobUri, metaData });
            }
        }

        /// <summary>
        /// Uploads the BLOB by container URI.
        /// </summary>
        /// <param name="containerUri">The container URI.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String for ETag.</returns>
        public static string UploadBlobByContainerUri(string containerUri, string blobIdentifier, byte[] dataBytes, BinaryStorageMetaBase meta = null, IDictionary<string, string> metaData = null)
        {
            try
            {
                containerUri.CheckEmptyString("containerUri");
                blobIdentifier.CheckEmptyString("blobIdentifier");

                var container = new CloudBlobContainer(new Uri(containerUri));
                var blob = container.GetBlockBlobReference(blobIdentifier);
                blob.Properties.FillMeta(meta);

                return UploadBlob(blob, dataBytes, metaData);
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlobByContainerUri", new { containerUri, blobIdentifier });
            }
        }

        /// <summary>
        /// Uploads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String for ETag.</returns>
        public static string UploadBlobByBlobUri(string blobUri, string content, Encoding encoding = null, BinaryStorageMetaBase meta = null, IDictionary<string, string> metaData = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                content.CheckEmptyString("content");

                return UploadBlobByBlobUri(blobUri, encoding.GetBytes(content), meta, metaData);
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlobByBlobUri", ex);
            }
        }

        /// <summary>
        /// Uploads the BLOB by container URI.
        /// </summary>
        /// <param name="containerUri">The container URI.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String for ETag.</returns>
        public static string UploadBlobByContainerUri(string containerUri, string blobIdentifier, string content, Encoding encoding = null, BinaryStorageMetaData meta = null, IDictionary<string, string> metaData = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                content.CheckEmptyString("content");

                return UploadBlobByContainerUri(containerUri, blobIdentifier, encoding.GetBytes(content), meta, metaData);
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlobByContainerUri", new { containerUri, blobIdentifier });
            }
        }

        #endregion

        #region Download Blob By Uri

        /// <summary>
        /// Downloads the BLOB.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="fetchAttribute">if set to <c>true</c> [fetch attribute].</param>
        /// <returns>BinaryStorageObject.</returns>
        public static BinaryStorageObject DownloadBlob(CloudBlockBlob blob, bool fetchAttribute)
        {
            var result = new BinaryStorageObject();

            IDictionary<string, string> metaDictionary;
            var blobProperties = blob.Properties;
            result.FillMeta(blobProperties);

            result.DataInBytes = DownloadBlob(blob, fetchAttribute, out metaDictionary);
            result.FillMeta(blobProperties);

            return result;
        }

        /// <summary>
        /// Downloads the BLOB.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="fetchAttribute">if set to <c>true</c> [fetch attribute].</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlob(CloudBlockBlob blob, bool fetchAttribute, out IDictionary<string, string> metaData)
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
                throw ex.Handle("DownloadBlob", blob.Uri.ToString());
            }
        }

        /// <summary>
        /// Downloads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="fetchAttribute">if set to <c>true</c> [fetch attribute].</param>
        /// <param name="blobProperties">The BLOB properties.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByBlobUri(string blobUri, bool fetchAttribute, out BlobProperties blobProperties, out IDictionary<string, string> metaData)
        {
            try
            {
                blobUri.CheckEmptyString("blobUri");

                var blob = new CloudBlockBlob(new Uri(blobUri));
                blobProperties = blob.Properties;

                return DownloadBlob(blob, fetchAttribute, out metaData);
            }
            catch (Exception ex)
            {
                throw ex.Handle("UploadBlobByBlobUri", blobUri);
            }
        }

        /// <summary>
        /// Downloads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="blobProperties">The BLOB properties.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByBlobUri(string blobUri, out BlobProperties blobProperties, out IDictionary<string, string> metaData)
        {
            return DownloadBlobByBlobUri(blobUri, true, out blobProperties, out metaData);
        }

        /// <summary>
        /// Downloads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="blobProperties">The BLOB properties.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByBlobUri(string blobUri, out BlobProperties blobProperties)
        {
            IDictionary<string, string> metaData = null;
            return DownloadBlobByBlobUri(blobUri, false, out blobProperties, out metaData);
        }

        /// <summary>
        /// Downloads the BLOB by BLOB URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByBlobUri(string blobUri)
        {
            IDictionary<string, string> metaData = null;
            BlobProperties blobProperties;
            return DownloadBlobByBlobUri(blobUri, false, out blobProperties, out metaData);
        }

        /// <summary>
        /// Downloads the BLOB by container URI.
        /// </summary>
        /// <param name="containerUri">The container URI.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="fetchAttribute">if set to <c>true</c> [fetch attribute].</param>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByContainerUri(string containerUri, string blobIdentifier, bool fetchAttribute, out BinaryStorageMetaData meta, out IDictionary<string, string> metaData)
        {
            try
            {
                containerUri.CheckEmptyString("containerUri");
                blobIdentifier.CheckEmptyString("blobIdentifier");

                var container = new CloudBlobContainer(new Uri(containerUri));
                var blob = container.GetBlockBlobReference(blobIdentifier);
                meta = new BinaryStorageMetaData();
                blob.Properties.FillMeta(meta);

                return DownloadBlob(blob, fetchAttribute, out metaData);
            }
            catch (Exception ex)
            {
                throw ex.Handle("DownloadBlobByContainerUri", new { containerUri, blobIdentifier });
            }
        }

        /// <summary>
        /// Downloads the BLOB by container URI.
        /// </summary>
        /// <param name="containerUri">The container URI.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByContainerUri(string containerUri, string blobIdentifier)
        {
            IDictionary<string, string> metaData = null;
            return DownloadBlobByContainerUri(containerUri, blobIdentifier, out metaData);
        }

        /// <summary>
        /// Downloads the BLOB by container URI.
        /// </summary>
        /// <param name="containerUri">The container URI.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] DownloadBlobByContainerUri(string containerUri, string blobIdentifier, out IDictionary<string, string> metaData)
        {
            BinaryStorageMetaData meta;

            return DownloadBlobByContainerUri(containerUri, blobIdentifier, true, out meta, out metaData);
        }

        #endregion

        #region DeleteBlob

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public void DeleteBlob(BinaryStorageIdentifier storageIdentifier)
        {
            if (storageIdentifier != null)
            {
                DeleteBlob(storageIdentifier.Container, storageIdentifier.Identifier);
            }
        }

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        public void DeleteBlob(string containerName, string blobIdentifier)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                var container = blobClient.GetContainerReference(containerName);
                DeleteBlob(container, blobIdentifier);
            }
            catch (Exception ex)
            {
                throw ex.Handle("DeleteBlob", new { containerName, blobIdentifier });
            }
        }

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        public static void DeleteBlob(CloudBlobContainer container, string blobIdentifier)
        {
            try
            {
                container.CheckNullObject("container");

                var blob = container.GetBlockBlobReference(blobIdentifier);
                blob.Delete();
            }
            catch (Exception ex)
            {
                throw ex.Handle("DeleteBlob", new { container = container == null ? null : container.Uri.ToString(), blobIdentifier = blobIdentifier });
            }
        }

        #endregion

        #region QueryBlob

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="filterFunction">The filter function.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;CloudBlockBlob&gt;.</returns>
        public static IEnumerable<CloudBlockBlob> QueryBlob(CloudBlobContainer container, Func<CloudBlockBlob, bool> filterFunction, int? limitCount = null)
        {
            try
            {
                container.CheckNullObject("container");

                if (filterFunction != null)
                {
                    return limitCount == null ? container.ListBlobs().OfType<CloudBlockBlob>().Where(filterFunction) : container.ListBlobs().OfType<CloudBlockBlob>().Where(filterFunction).Take(limitCount.Value);
                }
                else
                {
                    return limitCount == null ? container.ListBlobs().OfType<CloudBlockBlob>() : container.ListBlobs().OfType<CloudBlockBlob>().Take(limitCount.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryBlob", new { container = container == null ? null : container.Uri.ToString(), limitCount = limitCount });
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="filterFunction">The filter function.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> QueryBlob(string containerName, Func<IListBlobItem, bool> filterFunction, int limitCount = 100)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                var blobs = QueryBlob(this.blobClient.GetContainerReference(containerName), filterFunction, limitCount);
                return new List<string>(from i in blobs select i.Name);
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryBlob", new { containerName, limitCount });
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
        public List<BinaryStorageMetaBase> QueryBlob(string containerName, string contentType, string md5,
            long? length, int limitCount = 100)
        {
            try
            {
                containerName.CheckEmptyString("containerName");

                var blobs = QueryBlob(this.blobClient.GetContainerReference(containerName), contentType, md5, length, limitCount);
                return (from i in blobs
                        select new BinaryStorageMetaBase
                            {
                                Container = containerName,
                                Identifier = i.Name,
                                Mime = i.Properties.ContentType,
                                Name = i.Properties.ContentDisposition.ConvertContentDispositionToName(),
                                Length = i.Properties.Length
                            }).ToList();
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryBlob", new { containerName, limitCount });
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
        /// <returns>IEnumerable&lt;CloudBlockBlob&gt;.</returns>
        public static IEnumerable<CloudBlockBlob> QueryBlob(CloudBlobContainer container, string contentType, string md5, long? length, int limitCount = 100)
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
                throw ex.Handle("QueryBlob", new { container = container == null ? null : container.Uri.ToString(), contentType, md5, length, limitCount });
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// Connections the string to credential.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <returns>Microsoft.WindowsAzure.Storage.CloudStorageAccount.</returns>
        protected static CloudStorageAccount ConnectionStringToCredential(string storageConnectionString)
        {
            var keyValues = storageConnectionString.SafeToString().ParseToKeyValuePairCollection(';');
            var useHttps = keyValues.Get("DefaultEndpointsProtocol").Equals("https", StringComparison.InvariantCultureIgnoreCase);
            var accountKey = keyValues.Get("AccountKey");
            var accountName = keyValues.Get("AccountName");
            var customBlobDomain = keyValues.Get("CustomBlobDomain");
            AzureServiceProviderRegion region;
            Enum.TryParse(keyValues.Get("Region"), true, out region);

            return new CloudStorageAccount(
                new StorageCredentials(accountName, accountKey),
                string.IsNullOrWhiteSpace(customBlobDomain)
                    ? GetStorageEndpointUri(useHttps, accountName, "blob", region)
                    : new Uri(customBlobDomain),
                GetStorageEndpointUri(useHttps, accountName, "queue", region),
                GetStorageEndpointUri(useHttps, accountName, "table", region),
                GetStorageEndpointUri(useHttps, accountName, "file", region));
        }

        /// <summary>
        /// Gets the storage endpoint URI.
        /// </summary>
        /// <param name="useHttps">if set to <c>true</c> [use HTTPS].</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="feature">The feature.</param>
        /// <param name="region">The region.</param>
        /// <returns>Uri.</returns>
        protected static Uri GetStorageEndpointUri(bool useHttps, string accountName, string feature, AzureServiceProviderRegion region)
        {
            string uriFormat;

            switch (region)
            {
                case AzureServiceProviderRegion.China:
                    uriFormat = "{0}://{1}.{2}.core.chinacloudapi.cn";
                    break;
                default:
                    uriFormat = "{0}://{1}.{2}.core.windows.net";
                    break;
            }

            return new Uri(string.Format(uriFormat, useHttps ? "https" : "http", accountName, feature));
        }

        #endregion

        /// <summary>
        /// Creates the manager.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string. Example: Region=China;DefaultEndpointsProtocol=https;AccountName=YOURACCOUNTNAME;AccountKey=YOURACCOUNTKEY</param>
        /// <returns>AzureStorageManager.</returns>
        public static AzureStorageManager CreateManager(string storageConnectionString)
        {
            return new AzureStorageManager(storageConnectionString);
        }
    }
}
