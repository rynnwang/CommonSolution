using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityClient.
    /// </summary>
    internal class GravityClient : GravityBaseClient, IGravityServiceProtocol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravityClient" /> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="hook">The hook.</param>
        internal GravityClient(GravityEntryObject entry, GravityEventHook hook = null) : base(entry, hook)
        {
            this.Entry = entry;
            this.EventHook = hook;
        }

        /// <summary>
        /// Heartbeats the specified heartbeat.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        public HeartbeatEcho Heartbeat(Heartbeat heartbeat)
        {
            if (heartbeat != null)
            {
                return Invoke<Heartbeat, HeartbeatEcho>("heartbeat", heartbeat);
            }

            return null;
        }

        /// <summary>
        /// Commits the command result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CommitCommandResult(GravityCommandResult result)
        {
            if (result != null)
            {
                return Invoke<GravityCommandResult, Guid?>("command", result);
            }

            return null;
        }

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        public RemoteConfigurationObject RetrieveConfiguration(string name = null)
        {
            return Invoke<string, RemoteConfigurationObject>("configuration", name);
        }

        /// <summary>
        /// Invokes the specified feature.
        /// </summary>
        /// <typeparam name="TIn">The type of the t in.</typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="feature">The feature.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>TOut.</returns>
        protected TOut Invoke<TIn, TOut>(string feature, TIn parameter)
        {
            return Invoke<TIn, TOut>(string.Empty, feature, parameter);
        }
    }
}
