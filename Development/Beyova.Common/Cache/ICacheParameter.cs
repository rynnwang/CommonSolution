namespace Beyova.Cache
{
    /// <summary>
    /// Interface ICacheParameter
    /// </summary>
    public interface ICacheParameter
    {
        /// <summary>
        /// The expiration in second
        /// </summary>
        long? ExpirationInSecond { get; }

        /// <summary>
        /// Gets the failure expiration in second. If entity is failed to get, use this expiration if specified, otherwise use <see cref="ICacheParameter.ExpirationInSecond"/>.
        /// </summary>
        /// <value>The failure expiration in second.</value>
        long FailureExpirationInSecond { get; }
    }
}