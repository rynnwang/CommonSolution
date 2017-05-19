using System.Collections.Generic;
using Beyova.RestApi;
using Beyova.Api;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface IBinaryStorageService
    /// </summary>
    /// <typeparam name="TBinaryStorageObject">The generic type of the binary storage object.</typeparam>
    /// <typeparam name="TBinaryStorageCriteria">The generic type of the binary storage criteria.</typeparam>
    public interface IBinaryStorageService<TBinaryStorageObject, TBinaryStorageCriteria>
        where TBinaryStorageObject : BinaryStorageMetaData
        where TBinaryStorageCriteria : BinaryStorageMetaDataCriteria
    {
        /// <summary>
        /// Requests the binary upload credential.
        /// </summary>
        /// <param name="meteData">The mete data.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorageCredential, HttpConstants.HttpMethod.Post, "Upload")]
        [TokenRequired(true)]
        BinaryStorageActionCredential RequestBinaryUploadCredential(TBinaryStorageObject meteData);

        /// <summary>
        /// Requests the binary download credential.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorageCredential, HttpConstants.HttpMethod.Post, "Download")]
        BinaryStorageActionCredential RequestBinaryDownloadCredential(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Queries the binary storage. (User based)
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TBinaryStorageObject&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorage, HttpConstants.HttpMethod.Post, "User")]
        [TokenRequired(true)]
        List<TBinaryStorageObject> QueryUserBinaryStorage(TBinaryStorageCriteria criteria);

        /// <summary>
        /// Queries the binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TBinaryStorageObject&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorage, HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        [ApiPermission(CommonServiceConstants.Permission.BinaryStorageDataAdministrator, ApiPermission.Required)]
        List<TBinaryStorageObject> QueryBinaryStorage(TBinaryStorageCriteria criteria);

        /// <summary>
        /// Gets the binary storage by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TBinaryStorageObject.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorage, HttpConstants.HttpMethod.Get)]
        TBinaryStorageObject GetBinaryStorageByKey(string key);

        /// <summary>
        /// Gets the binary storage by keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>List&lt;TBinaryStorageObject&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorage, HttpConstants.HttpMethod.Post, "GetByBatch")]
        List<TBinaryStorageObject> GetBinaryStorageByKeys(List<string> keys);

        /// <summary>
        /// Commits the binary storage.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>BinaryStorageIdentifier.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorage, HttpConstants.HttpMethod.Put)]
        [TokenRequired(true)]
        TBinaryStorageObject CommitBinaryStorage(BinaryStorageCommitRequest request);

        /// <summary>
        /// Deletes the binary storage.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        [ApiOperation(CommonServiceConstants.ResourceName.BinaryStorage, HttpConstants.HttpMethod.Delete)]
        [TokenRequired(true)]
        void DeleteBinaryStorage(string identifier);
    }
}
