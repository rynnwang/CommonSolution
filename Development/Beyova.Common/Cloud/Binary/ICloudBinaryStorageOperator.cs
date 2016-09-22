using System;
using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// 
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
    }
}
