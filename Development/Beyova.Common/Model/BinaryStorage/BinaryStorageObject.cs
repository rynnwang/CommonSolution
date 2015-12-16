using System;
using System.Runtime.Serialization;

namespace Beyova.BinaryStorage
{
    /// <summary>
    /// Class BinaryStorageObject.
    /// </summary>
    public class BinaryStorageObject : BinaryStorageMetaData
    {
        /// <summary>
        /// Gets or sets the data.
        /// This field shares value with field DataInByte.
        /// The difference is, this field saves data as base64 string.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the data in bytes.
        /// This property is mapped to property 
        /// <c>Data</c> and this property would not be passed in WCF communication.
        /// </summary>
        /// <value>The data in bytes.</value>
        public byte[] DataInBytes
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Data) ? Convert.FromBase64String(Data) : null;
            }
            set
            {
                Data = Convert.ToBase64String(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageObject"/> class.
        /// </summary>
        public BinaryStorageObject()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageObject"/> class.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        public BinaryStorageObject(BinaryStorageMetaData metaData)
            : base()
        {
            SetMetaData(metaData);
        }

        /// <summary>
        /// Sets the meta data.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        public void SetMetaData(BinaryStorageMetaData metaData)
        {
            if (metaData != null)
            {
                Container = metaData.Container;
                Identifier = metaData.Identifier;
                Name = metaData.Name;
                Hash = metaData.Hash;
                Mime = metaData.Mime;
                Length = metaData.Length;
                Width = metaData.Width;
                Height = metaData.Height;
                Duration = metaData.Duration;
                OwnerKey = metaData.OwnerKey;
            }
        }
    }
}
