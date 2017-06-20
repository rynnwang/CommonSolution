namespace Beyova
{
    /// <summary>
    /// Interface ICriteria
    /// </summary>
    public interface ICriteria : IIdentifier
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; set; }
    }
}