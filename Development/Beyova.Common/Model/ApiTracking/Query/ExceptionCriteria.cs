using System;
using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking.Model
{
    /// <summary>
    /// Class ExceptionCriteria.
    /// </summary>
    public class ExceptionCriteria : ExceptionBase, ICriteria
    {
        #region Property

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

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
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCriteria" /> class.
        /// </summary>
        /// <param name="exceptionBase">The exception base.</param>
        public ExceptionCriteria(ExceptionBase exceptionBase)
            : base(exceptionBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCriteria"/> class.
        /// </summary>
        public ExceptionCriteria()
            : this(null)
        {
        }
    }
}
