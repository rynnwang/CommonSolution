using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class DiskDriveInfo.
    /// </summary>
    public class DiskDriveInfo
    {
        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the total free space.
        /// </summary>
        /// <value>The total free space.</value>
        public long TotalFreeSpace { get; set; }

        /// <summary>
        /// Gets the total size.
        /// </summary>
        /// <value>The total size.</value>
        public long TotalSize { get; set; }

        /// <summary>
        /// Gets or sets the volume label.
        /// </summary>
        /// <value>The volume label.</value>
        public string VolumeLabel { get; set; }
    }
}
