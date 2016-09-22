using Beyova.CommonService.DataAccessController;
using Beyova.Api;

namespace Beyova.CommonService
{
    ///// <summary>
    ///// Class AzureBinaryStorageHelper.
    ///// </summary>
    //public class AzureBinaryStorageHelper : AzureBinaryStorageHelper<BinaryStorageMetaData, BinaryStorageMetaDataCriteria, BinaryStorageMetaDataAccessController>
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="AzureBinaryStorageHelper"/> class.
    //    /// </summary>
    //    /// <param name="apiEndpoint">The API endpoint.</param>
    //    public AzureBinaryStorageHelper(ApiEndpoint apiEndpoint)
    //        : base(apiEndpoint)
    //    {
    //    }
    //}

    ///// <summary>
    ///// Class AzureBinaryStorageHelper.
    ///// </summary>
    //public class AzureBinaryStorageHelper<TBinaryStorageMetaData, TBinaryStoageCriteria, TDataAccessController>
    //    : BinaryStorageHelper<CloudBlobContainer, CloudBlockBlob, TBinaryStorageMetaData, TBinaryStoageCriteria, TDataAccessController>
    //    where TBinaryStorageMetaData : BinaryStorageMetaData, new()
    //    where TBinaryStoageCriteria : BinaryStorageMetaDataCriteria, new()
    //    where TDataAccessController : BinaryStorageMetaDataBaseAccessController<TBinaryStorageMetaData, TBinaryStoageCriteria>, new()
    //{
    //    protected int credentialExpireOffset;

    //    /// <summary>
    //    /// Gets or sets the credential expire offset.
    //    /// </summary>
    //    /// <value>The credential expire offset.</value>
    //    protected override int CredentialExpireOffset
    //    {
    //        get { return credentialExpireOffset; }
    //    }

    //    /// <summary>
    //    /// Gets the azure storage manager.
    //    /// </summary>
    //    /// <value>The azure storage manager.</value>
    //    public AzureStorageOperator AzureStorageOperator
    //    {
    //        get { return this.cloudBinaryStorageOperator as AzureStorageOperator; }
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="AzureBinaryStorageHelper{TResource, TResourceCriteria, TDataAccessController}" /> class.
    //    /// </summary>
    //    /// <param name="endpoint">The endpoint.</param>
    //    /// <param name="credentialExpireOffset">The credential expire offset.</param>
    //    protected AzureBinaryStorageHelper(ApiEndpoint endpoint, int credentialExpireOffset = 30) : base(endpoint)
    //    {
    //        this.credentialExpireOffset = credentialExpireOffset;
    //    }

    //    /// <summary>
    //    /// Gets the cloud binary storage operator.
    //    /// </summary>
    //    /// <param name="endpoint">The endpoint.</param>
    //    /// <returns>Beyova.CloudBinaryStorageOperator&lt;Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer, Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob&gt;.</returns>
    //    protected override CloudBinaryStorageOperator<CloudBlobContainer, CloudBlockBlob> GetCloudBinaryStorageOperator(ApiEndpoint endpoint)
    //    {
    //        return new AzureStorageOperator(endpoint);
    //    }
    //}
}
