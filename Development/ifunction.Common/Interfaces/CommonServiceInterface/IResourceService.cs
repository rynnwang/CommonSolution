using System;
using System.Collections.Generic;
using ifunction.BinaryStorage;
using ifunction.Model;
using ifunction.RestApi;

namespace ifunction.CommonServiceInterface
{
    /// <summary>
    /// Interface IResourceService
    /// </summary>
    /// <typeparam name="TResource">The type of the t resource.</typeparam>
    /// <typeparam name="TResourceCriteria">The type of the t resource criteria.</typeparam>
    public interface IResourceService<TResource, TResourceCriteria>
        where TResource : BinaryStorageMetaBase
        where TResourceCriteria : BinaryStorageMetaDataCriteria
    {
        /// <summary>
        /// Requests the resource upload credential.
        /// </summary>
        /// <param name="meteData">The mete data.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        [ApiOperation("ResourceCredential", HttpConstants.HttpMethod.Post, "Upload")]
        [TokenRequired(true)]
        BinaryStorageActionCredential RequestResourceUploadCredential(TResource meteData);

        /// <summary>
        /// Requests the resource download credential.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        [ApiOperation("ResourceCredential", HttpConstants.HttpMethod.Post, "Download")]
        BinaryStorageActionCredential RequestResourceDownloadCredential(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Queries the resource.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TResource&gt;.</returns>
        [ApiOperation("Resource", HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        List<TResource> QueryResource(TResourceCriteria criteria);

        /// <summary>
        /// Gets the resource by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>TResource.</returns>
        [ApiOperation("Resource", HttpConstants.HttpMethod.Get)]
        TResource GetResourceByKey(string key);

        /// <summary>
        /// Gets the resource by keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>List&lt;TResource&gt;.</returns>
        [ApiOperation("Resource", HttpConstants.HttpMethod.Post, "GetByBatch")]
        List<TResource> GetResourceByKeys(List<string> keys);

        /// <summary>
        /// Commits the resource.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        [ApiOperation("Resource", HttpConstants.HttpMethod.Put)]
        [TokenRequired(true)]
        void CommitResource(BinaryStorageIdentifier identifier);
    }
}
