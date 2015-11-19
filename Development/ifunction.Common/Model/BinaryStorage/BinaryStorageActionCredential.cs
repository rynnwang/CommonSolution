using System;

namespace ifunction.BinaryStorage
{
    /// <summary>
    /// Class BinaryStorageActionCredential.
    /// </summary>
    public class BinaryStorageActionCredential : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the storage URI.
        /// </summary>
        /// <value>The storage URI.</value>        
        public string StorageUri { get; set; }

        /// <summary>
        /// Gets or sets the credential URI.
        /// <remarks>Credential uri, which is used to upload, download or any other action on target.</remarks>
        /// </summary>
        /// <value>The credential URI.</value>
        public string CredentialUri { get; set; }

        /// <summary>
        /// Gets or sets the credential expired stamp.
        /// </summary>
        /// <value>The credential expired stamp.</value>        
        public DateTime? CredentialExpiredStamp { get; set; }
    }
}
