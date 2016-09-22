using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Class CloudBinaryStorageOperator.
    /// </summary>
    /// <typeparam name="TCloudContainer">The type of the T cloud container.</typeparam>
    /// <typeparam name="TCloudBlobObject">The type of the T cloud BLOB object.</typeparam>
    public abstract class CloudBinaryStorageOperator<TCloudContainer, TCloudBlobObject> : ICloudBinaryStorageOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudBinaryStorageOperator{TCloudContainer, TCloudBlobObject}"/> class.
        /// </summary>
        protected CloudBinaryStorageOperator()
        {
        }

        /// <summary>
        /// Creates the BLOB upload credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">The hash. This value is used only when blob service provider needs to set hash (MD5) when creating credential of upload action.</param>
        /// <param name="contentType">Type of the content. This value is used only when blob service provider needs to set content type (MIME) when creating credential of upload action.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public abstract BinaryStorageActionCredential CreateBlobUploadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute, string hash = null, string contentType = null);

        /// <summary>
        /// Creates the BLOB download credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public abstract BinaryStorageActionCredential CreateBlobDownloadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute);

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public abstract void DeleteBlob(BinaryStorageIdentifier storageIdentifier);

        /// <summary>
        /// Downloads the binary bytes by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.Byte[].</returns>
        public abstract byte[] DownloadBinaryBytesByCredentialUri(string blobUri);

        /// <summary>
        /// Downloads the binary stream by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>Stream.</returns>
        public abstract Stream DownloadBinaryStreamByCredentialUri(string blobUri);

        /// <summary>
        /// Uploads the binary bytes by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public abstract string UploadBinaryBytesByCredentialUri(string blobUri, byte[] dataBytes, string contentType, string fileName = null);

        /// <summary>
        /// Uploads the binary stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public abstract string UploadBinaryStreamByCredentialUri(string blobUri, Stream stream, string contentType, string fileName = null);

        /// <summary>
        /// Fetches the cloud meta. Returned object would only includes (md5, length, name, content type).
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageMetaBase.</returns>
        public abstract BinaryStorageMetaData FetchCloudMeta(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public abstract List<string> GetContainers();

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;TCloudBlobObject&gt;.</returns>
        public abstract IEnumerable<TCloudBlobObject> QueryBlob(TCloudContainer container, string contentType, string md5, long? length, int limitCount);

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;TCloudBlobObject&gt;.</returns>
        public abstract IEnumerable<TCloudBlobObject> QueryBlob(string container, string contentType, string md5, long? length, int limitCount);

        /// <summary>
        /// Queries the binary BLOB by container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="md5">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>List&lt;BinaryStorageMeta&gt;.</returns>
        public abstract List<BinaryStorageMetaData> QueryBinaryBlobByContainer(string containerName, string contentType, string md5, long? length, int limitCount);

        /// <summary>
        /// Check Existence of the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns><c>true</c> if existed, <c>false</c> otherwise.</returns>
        public abstract bool Exists(BinaryStorageIdentifier identifier);
    }
}
