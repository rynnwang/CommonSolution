namespace Beyova
{
    /// <summary>
    /// Interface IGlobalObjectName
    /// </summary>
    public interface IGlobalObjectName
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>
        /// The culture code.
        /// e.g.: zh-CN, en-US, pt-PT, etc.
        /// </value>
        string CultureCode { get; set; }
    }
}