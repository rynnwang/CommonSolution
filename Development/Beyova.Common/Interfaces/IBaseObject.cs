namespace Beyova
{
    /// <summary>
    /// Interface IBaseObject
    /// </summary>
    public interface IBaseObject : ISimpleBaseObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last updated by.
        /// </summary>
        /// <value>
        /// The last updated by.
        /// </value>
        string LastUpdatedBy { get; set; }

        #endregion Properties
    }
}