using System;
using System.Collections.Generic;

namespace Beyova.Gravity
{
    /// <summary>
    /// Interface IGravityServiceProtocol
    /// </summary>
    public interface IGravityServiceProtocol
    {
        /// <summary>
        /// Heartbeats the specified heartbeat.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        /// <returns>HeartbeatEcho.</returns>
        HeartbeatEcho Heartbeat(Heartbeat heartbeat);

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        RemoteConfigurationObject RetrieveConfiguration(string name = null);

        /// <summary>
        /// Commits the command result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        Guid? CommitCommandResult(GravityCommandResult result);
    }
}
