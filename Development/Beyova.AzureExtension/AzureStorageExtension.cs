using System.Collections.Generic;
using System.Text.RegularExpressions;
using Beyova;
using Beyova.BinaryStorage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Beyova.AzureExtension
{
    /// <summary>
    /// Class AzureStorageExtension.
    /// </summary>
    public static class AzureStorageExtension
    {
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
        /// Fills the meta.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="blobMeta">The BLOB meta.</param>
        public static void FillMeta(this BinaryStorageMetaData meta, Dictionary<string, string> blobMeta)
        {
            if (meta != null && blobMeta != null)
            {
                meta.Duration = blobMeta.SafeGetValue("duration").ObjectToNullableInt32();
                meta.Height = blobMeta.SafeGetValue("height").ObjectToNullableInt32();
                meta.Width = blobMeta.SafeGetValue("width").ObjectToNullableInt32();
                meta.OwnerKey = blobMeta.SafeGetValue("owner").ObjectToGuid();
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
                blobProperties.ContentType = meta.Mime;
                blobProperties.ContentDisposition = string.IsNullOrWhiteSpace(meta.Name)
                    ? null
                    : meta.Name.ToUrlEncodedText();
                blobProperties.ContentDisposition = meta.Name.ConvertNameToContentDisposition();
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
                    meta.Merge("duration", metaData.Duration.ToString());
                }

                if (metaData.Height != null)
                {
                    meta.Merge("height", metaData.Height.ToString());
                }

                if (metaData.Width != null)
                {
                    meta.Merge("width", metaData.Width.ToString());
                }

                if (metaData.OwnerKey != null)
                {
                    meta.Merge("owner", metaData.OwnerKey.ToString());
                }
            }
        }

        #endregion
    }
}
