using System;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class FeatureModuleSwitchCommandInvoker.
    /// </summary>
    public class FeatureModuleSwitchCommandInvoker : IGravityCommandInvoker
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get { return GravityConstants.BuiltInAction.SwitchFeature; } }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>JToken.</returns>
        public JToken Invoke(JToken parameters)
        {
            try
            {
                parameters.CheckNullObject(nameof(parameters));
                var switchObject = parameters.ToObject<FeatureModuleSwitchObject>();
                switchObject.EnsureStatus();
                FeatureModuleSwitch.ChangeStatus(switchObject);

                return JToken.FromObject(switchObject);
            }
            catch (Exception ex)
            {
                throw ex.Handle(parameters);
            }
        }
    }
}