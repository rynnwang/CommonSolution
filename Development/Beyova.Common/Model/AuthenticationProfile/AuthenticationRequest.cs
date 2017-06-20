namespace Beyova
{
    /// <summary>
    /// Class AuthenticationRequest.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuthenticationRequest<T> : AccessCredential
    {
        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public PlatformType? Platform { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        public T Realm { get; set; }
    }

    /// <summary>
    /// Class AuthenticationRequest.
    /// </summary>
    public class AuthenticationRequest : AuthenticationRequest<string>
    {
    }
}