namespace Beyova
{
    /// <summary>
    /// Interface ICredential
    /// </summary>
    public interface ICredential : IIdentifier
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
    }
}