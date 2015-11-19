using System;
using System.Text.RegularExpressions;
using ifunction.ExceptionSystem;

namespace ifunction.Model
{
    /// <summary>
    /// Class CloudResourceUri.
    /// </summary>
    public class CloudResourceUri
    {
        /// <summary>
        /// The protocol
        /// </summary>
        protected const string Protocol = "cloud://";

        /// <summary>
        /// The URI regex
        /// </summary>
        protected static Regex UriRegex = new Regex(Protocol + @"(?<Type>([^\/]+))/(?<Container>([^\/]+))/(?<Identifier>([^\/]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #region Properties

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

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

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(Protocol + "{0}/{1}/{2}", this.Type.SafeToString("default"), this.Container.SafeToString("default"), this.Identifier.SafeToString("default"));
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.SafeToString());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Parses the specified cloud protocol URI.
        /// </summary>
        /// <param name="cloudProtocolUri">The cloud protocol URI.</param>
        /// <returns>CloudResourceUri.</returns>
        /// <exception cref="InvalidObjectException">cloudProtocolUri;null</exception>
        public static CloudResourceUri Parse(string cloudProtocolUri)
        {
            cloudProtocolUri.CheckEmptyString("cloudProtocolUri");

            var match = UriRegex.Match(cloudProtocolUri);

            if (match.Success)
            {
                return new CloudResourceUri()
                {
                    Type = match.Result("${Type}"),
                    Container = match.Result("${Container}"),
                    Identifier = match.Result("${Identifier}")
                };
            }
            else
            {
                throw new InvalidObjectException("cloudProtocolUri", null, cloudProtocolUri);
            }
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="cloudProtocolUri">The cloud protocol URI.</param>
        /// <param name="cloudResourceUri">The cloud resource URI.</param>
        /// <returns><c>true</c> if succeed to parse, <c>false</c> otherwise.</returns>
        public static bool TryParse(string cloudProtocolUri, out CloudResourceUri cloudResourceUri)
        {
            try
            {
                cloudResourceUri = Parse(cloudProtocolUri);
                return true;
            }
            catch
            {
                cloudResourceUri = null;
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is cloud resource] [the specified URI].
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns><c>true</c> if [is cloud resource] [the specified URI]; otherwise, <c>false</c>.</returns>
        public static bool IsCloudResource(string uri)
        {
            return uri.SafeToString().StartsWith(Protocol, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
