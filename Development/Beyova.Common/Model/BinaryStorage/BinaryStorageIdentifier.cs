using Beyova;

namespace Beyova.BinaryStorage
{
    /// <summary>
    /// Class BinaryStorageIdentifier.
    /// </summary>
    public class BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public string Container { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", Container, Identifier);
        }

        /// <summary>
        /// To the cloud resource URI.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>CloudResourceUri.</returns>
        public CloudResourceUri ToCloudResourceUri(string type)
        {
            return new CloudResourceUri
            {
                Type = type.SafeToString("default"),
                Container = this.Container,
                Identifier = this.Identifier
            };
        }
    }
}
