using System;
using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Interface IBinaryStorageHelper
    /// </summary>
    /// <typeparam name="TBinaryStorageMetaData">The type of the t binary storage meta data.</typeparam>
    /// <typeparam name="TBinaryStorageCriteria">The type of the t binary storage criteria.</typeparam>
    public interface IBinaryStorageHelper<TBinaryStorageMetaData, TBinaryStorageCriteria>
        where TBinaryStorageMetaData : BinaryStorageMetaData, new()
        where TBinaryStorageCriteria : BinaryStorageMetaDataCriteria, new()
    {
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
        BinaryStorageActionCredential CreateBinaryUploadCredential(TBinaryStorageMetaData meta, int? expireOffset = null);

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
        BinaryStorageActionCredential CreateBinaryDownloadCredential(BinaryStorageIdentifier identifier, int? expireOffset = null);

        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        void DeleteBinaryStorage(Guid? identifier);

        /// <summary>
        /// Queries the binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        List<TBinaryStorageMetaData> QueryBinaryStorage(TBinaryStorageCriteria criteria);

        /// <summary>
        /// Queries the user binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TBinaryStorageMetaData&gt;.</returns>
        List<TBinaryStorageMetaData> QueryUserBinaryStorage(TBinaryStorageCriteria criteria);

        /// <summary>
        /// Commits the binary storage.
        /// <remarks>
        /// This method would try to update state of binary storage to <c>Committed</c>.
        /// </remarks></summary>
        /// <param name="request">The request.</param>
        /// <returns>BinaryStorageMetaData.</returns>
        TBinaryStorageMetaData CommitBinaryStorage(BinaryStorageCommitRequest request);

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>IEnumerable&lt;TCloudContainer&gt;.</returns>
        IEnumerable<string> GetContainers();

        /// <summary>
        /// Creates the BLOB download credential. This is blob operation only, no database involved.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        BinaryStorageActionCredential CreateBlobDownloadCredential(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Creates the BLOB upload credential. This is blob operation only, no database involved.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="hash">The hash. This value is used only when blob service provider needs to set hash (MD5) when creating credential of upload action.</param>
        /// <param name="contentType">Type of the content. This value is used only when blob service provider needs to set content type (MIME) when creating credential of upload action.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        BinaryStorageActionCredential CreateBlobUploadCredential(BinaryStorageIdentifier identifier, string hash = null, string contentType = null);

        /// <summary>
        /// Uploads the binary by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>System.String.</returns>
        string UploadBinaryByCredential(BinaryStorageActionCredential credential, byte[] data, string contentType);

        /// <summary>
        /// Downloads the binary by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Stream.</returns>
        Stream DownloadBinaryStreamByCredential(BinaryStorageActionCredential credential);

        /// <summary>
        /// Downloads the binary bytes by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Byte[].</returns>
        Byte[] DownloadBinaryBytesByCredential(BinaryStorageActionCredential credential);

        /// <summary>
        /// Deletes the binary.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        void DeleteBinary(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Gets the binary storage meta data by identifiers.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>System.Collections.Generic.List&lt;TBinaryStorageMetaData&gt;.</returns>
        List<TBinaryStorageMetaData> GetBinaryStorageMetaDataByIdentifiers(List<BinaryStorageIdentifier> identifiers);
    }
}
