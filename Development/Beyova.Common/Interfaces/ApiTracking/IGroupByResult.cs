
namespace Beyova.ApiTracking
{
    /// <summary>
    /// Interface IGroupByResult
    /// </summary>
    public interface IGroupByResult
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        long Count { get; set; }
    }
}
