using System;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class UpdateConfigurationCommandInvoker.
    /// </summary>
    public class UpdateConfigurationCommandInvoker : IGravityCommandInvoker
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get { return GravityConstants.BuiltInAction.UpdateConfiguration; } }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>JToken.</returns>
        public JToken Invoke(JToken parameters)
        {
            try
            {
                var configurationReader = GravityShell.Host.ConfigurationReader;
                configurationReader.CheckNullObject(nameof(configurationReader));

                configurationReader.RefreshSettings();
                return configurationReader.Hash.ToJson(false);
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }
    }
}