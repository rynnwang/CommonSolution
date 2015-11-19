using System;
using System.Runtime.Serialization;
using ifunction.Model;

namespace ifunction.BinaryStorage
{
    /// <summary>
    /// Class BinaryStorageMetaData.
    /// </summary>
    public class BinaryStorageMetaBase : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the MIME.
        /// <remarks>
        /// http://www.w3.org/wiki/Evolution/MIME
        /// </remarks>
        /// </summary>
        /// <value>The MIME.</value>
        
        public string Mime { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        
        public long? Length { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// <remarks>It is used when storage is image or video.</remarks>
        /// </summary>
        /// <value>The width.</value>
        
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// <remarks>It is used when storage is image or video.</remarks>
        /// </summary>
        /// <value>The height.</value>
        
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// <remarks>It is used when storage is audio or video. Unit: second.</remarks>
        /// </summary>
        /// <value>The duration.</value>
        
        public int? Duration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageMetaBase"/> class.
        /// </summary>
        /// <param name="metaBase">The meta base.</param>
        public BinaryStorageMetaBase(BinaryStorageMetaBase metaBase = null)
        {
            if (metaBase != null)
            {
                this.Name = metaBase.Name;
                this.Mime = metaBase.Mime;
                this.Length = metaBase.Length;
                this.Width = metaBase.Width;
                this.Height = metaBase.Height;
                this.Duration = metaBase.Duration;
            }
        }
    }
}
