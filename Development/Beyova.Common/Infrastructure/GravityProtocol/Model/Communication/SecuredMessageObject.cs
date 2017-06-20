using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class SecuredMessageObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SecuredMessageObject<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>The stamp.</value>
        public DateTime? Stamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredMessageObject{T}"/> class.
        /// </summary>
        public SecuredMessageObject()
        {
            this.Stamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Validates the stamp.
        /// </summary>
        /// <returns><c>true</c> if Validation passed, <c>false</c> otherwise.</returns>
        public bool ValidateStamp()
        {
            const int allowedStampOffset = 60 * 60 * 24;
            return Stamp.HasValue && Math.Abs((Stamp.Value - DateTime.UtcNow).TotalSeconds) < allowedStampOffset;
        }
    }
}