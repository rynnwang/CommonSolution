namespace Beyova.Gravity
{
    /// <summary>
    /// Interface IBaseGravityProtocol
    /// </summary>
    internal interface IBaseGravityProtocol
    {
        /// <summary>
        /// Heartbeats the specified heartbeat.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        /// <returns>GravityValidationResult.</returns>
        GravityValidationResult Heartbeat(GravityHeartbeat heartbeat);

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        RemoteConfigurationObject GetConfiguration(string name);
    }
}
