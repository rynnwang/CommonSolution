using System;
using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Interface ICloudBinaryStorageOperator
    /// </summary>
    public interface ICloudBinaryStorageOperator
    {
        /// <summary>
        /// Downloads the binary bytes by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.Byte[].</returns>
        byte[] DownloadBinaryBytesByCredentialUri(string blobUri);

        /// <summary>
        /// Downloads the binary stream by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>Stream.</returns>
        Stream DownloadBinaryStreamByCredentialUri(string blobUri);

        /// <summary>
        /// Uploads the binary bytes by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        string UploadBinaryBytesByCredentialUri(string blobUri, byte[] dataBytes, string contentType, string fileName = null);

        /// <summary>
        /// Uploads the binary stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        string UploadBinaryStreamByCredentialUri(string blobUri, Stream stream, string contentType, string fileName = null);

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        void DeleteBlob(BinaryStorageIdentifier storageIdentifier);

        /// <summary>
        /// Creates the BLOB upload credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        BinaryStorageActionCredential CreateBlobUploadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute, string hash = null, string contentType = null);

        /// <summary>
        /// Creates the BLOB download credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        BinaryStorageActionCredential CreateBlobDownloadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute);

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> GetContainers();

        /// <summary>
        /// Existses the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        bool Exists(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Fetches the cloud meta.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageMetaData.</returns>
        BinaryStorageMetaData FetchCloudMeta(BinaryStorageIdentifier identifier);
    }
}
