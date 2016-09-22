using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Beyova;
using Beyova.Api;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Beyova.AzureExtension
{
    /// <summary>
    /// Class AzureStorageExtension.
    /// </summary>
    public static class AzureStorageExtension
    {
        const string metaDuration = "duration";

        const string metaHeight = "height";

        const string metaWidth = "width";

        #region Utility

        /// <summary>
        /// The disposition format
        /// </summary>
        private const string dispositionFormat = "attachment; filename=\"{0}\"";

        /// <summary>
        /// The regex
        /// </summary>
        static Regex regex = new Regex(string.Format(dispositionFormat.Replace(" ", "\\s").Replace("\"", "(\")?"), "?<name>([0-9a-zA-Z\\-_%@\\.]+)"), RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Converts the name to content disposition.
        /// </summary>
        /// <param name="anyBlobName">Name of any BLOB.</param>
        /// <returns>System.String.</returns>
        public static string ConvertNameToContentDisposition(this string anyBlobName)
        {
            return string.IsNullOrWhiteSpace(anyBlobName) ? anyBlobName : string.Format(dispositionFormat, anyBlobName.Trim().ToUrlEncodedText());
        }

        /// <summary>
        /// Converts the name of the content disposition to.
        /// </summary>
        /// <param name="contentDisposition">The content disposition.</param>
        /// <returns>System.String.</returns>
        public static string ConvertContentDispositionToName(this string contentDisposition)
        {
            var match = regex.Match(contentDisposition.SafeToString());
            if (match.Success)
            {
                return match.Result("${name}");
            }

            return contentDisposition;
        }

        /// <summary>
        /// Fills the meta.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="blobProperties">The BLOB properties.</param>
        public static void FillMeta(this BinaryStorageMetaData meta, BlobProperties blobProperties)
        {
            if (meta != null && blobProperties != null)
            {
                meta.Hash = blobProperties.ContentMD5;
                meta.Mime = blobProperties.ContentType;
                meta.Name = blobProperties.ContentDisposition.ConvertContentDispositionToName();
            }
        }

        /// <summary>
        /// Sets the BLOB property.
        /// </summary>
        /// <param name="blobProperties">The BLOB properties.</param>
        /// <param name="meta">The meta.</param>
        public static void FillMeta(this BlobProperties blobProperties, BinaryStorageMetaBase meta)
        {
            if (meta != null && blobProperties != null)
            {
                blobProperties.ContentType = meta.Mime.SafeToString(HttpConstants.ContentType.BinaryDefault);
                blobProperties.ContentDisposition = meta.Name?.ConvertNameToContentDisposition();
            }
        }

        /// <summary>
        /// Fills the meta.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        public static void FillMeta(this Dictionary<string, string> meta, BinaryStorageMetaData metaData)
        {
            if (meta != null && metaData != null)
            {
                if (metaData.Duration != null)
                {
                    meta.Merge(metaDuration, metaData.Duration.ToString());
                }

                if (metaData.Height != null)
                {
                    meta.Merge(metaHeight, metaData.Height.ToString());
                }

                if (metaData.Width != null)
                {
                    meta.Merge(metaWidth, metaData.Width.ToString());
                }
            }
        }

        #endregion

        /// <summary>
        /// Connections the string to credential.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <returns>Microsoft.WindowsAzure.Storage.CloudStorageAccount.</returns>
        public static CloudStorageAccount ConnectionStringToCredential(this string storageConnectionString)
        {
            var keyValues = storageConnectionString.SafeToString().ParseToKeyValuePairCollection(';');
            var useHttps = keyValues.Get("DefaultEndpointsProtocol")?.Equals(HttpConstants.HttpProtocols.Https, StringComparison.InvariantCultureIgnoreCase) ?? false;
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
        /// Connections the string to credential.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>Microsoft.WindowsAzure.Storage.CloudStorageAccount.</returns>
        public static CloudStorageAccount ConnectionStringToCredential(this ApiEndpoint endpoint)
        {
            if (endpoint != null)
            {
                var useHttps = endpoint.Protocol.SafeEquals(HttpConstants.HttpProtocols.Https, StringComparison.InvariantCultureIgnoreCase);
                var accountKey = endpoint.Token;
                var accountName = endpoint.Account;
                var customBlobDomain = endpoint.Host;

                AzureServiceProviderRegion region;
                Enum.TryParse(endpoint.Version, true, out region);

                return new CloudStorageAccount(
                    new StorageCredentials(accountName, accountKey),
                    string.IsNullOrWhiteSpace(customBlobDomain)
                        ? GetStorageEndpointUri(useHttps, accountName, "blob", region)
                        : new Uri(customBlobDomain),
                    GetStorageEndpointUri(useHttps, accountName, "queue", region),
                    GetStorageEndpointUri(useHttps, accountName, "table", region),
                    GetStorageEndpointUri(useHttps, accountName, "file", region));
            }

            return null;
        }

        /// <summary>
        /// To the connection string.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.String.</returns>
        public static string ToConnectionString(this ApiEndpoint endpoint)
        {
            if (endpoint != null)
            {
                var values = new Dictionary<string, string>();
                values.Add("Region", endpoint.Version.SafeToString("China"));
                values.Add("DefaultEndpointsProtocol", endpoint.Protocol.SafeToString(HttpConstants.HttpProtocols.Https));
                values.AddIfNotNullOrEmpty("AccountKey", endpoint.Token);
                values.AddIfNotNullOrEmpty("AccountName", endpoint.Account);
                values.AddIfNotNullOrEmpty("CustomBlobDomain", endpoint.Host);

                return values.ToKeyValuePairString(';');
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the storage endpoint URI.
        /// </summary>
        /// <param name="useHttps">if set to <c>true</c> [use HTTPS].</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="feature">The feature.</param>
        /// <param name="region">The region.</param>
        /// <returns>Uri.</returns>
        internal static Uri GetStorageEndpointUri(bool useHttps, string accountName, string feature, AzureServiceProviderRegion region)
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

            return new Uri(string.Format(uriFormat, useHttps ? HttpConstants.HttpProtocols.Https : HttpConstants.HttpProtocols.Http, accountName, feature));
        }
    }
}
