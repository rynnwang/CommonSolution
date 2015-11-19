
namespace ifunction.Model
{
    /// <summary>
    /// Abstract class for base object, with key, created stamp and last updated stamp.
    /// </summary>
    public abstract class BaseObject : SimpleBaseObject, IBaseObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last updated by.
        /// </summary>
        /// <value>
        /// The last updated by.
        /// </value>
        public string LastUpdatedBy { get; set; }

        #endregion
    }
}
