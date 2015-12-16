using System;
using System.Runtime.Serialization;

namespace Beyova.Model
{
    /// <summary>
    /// Class BasePageIndexedCriteria.
    /// </summary>
    public class BaseWaterFallCriteria : BaseCriteria
    {
        #region Properties

        /// <summary>
        /// Gets or sets the data cursor.
        /// </summary>
        /// <value>The data cursor.</value>
        public Guid? DataCursor { get; set; }

        /// <summary>
        /// Gets or sets from stamp.
        /// </summary>
        /// <value>From stamp.</value>
        public DateTime? FromStamp { get; set; }

        /// <summary>
        /// Gets or sets to stamp.
        /// </summary>
        /// <value>To stamp.</value>
        public DateTime? ToStamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time forwarding].
        /// </summary>
        /// <value><c>true</c> if [time forwarding]; otherwise, <c>false</c>.</value>
        public bool TimeForwarding { get; set; }

        #endregion
    }
}
