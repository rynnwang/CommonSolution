using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class ProcessCommandInvoker.
    /// </summary>
    public class ProcessCommandInvoker : IGravityCommandInvoker
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get { return GravityConstants.BuiltInAction.RunProcess; } }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>JToken.</returns>
        public JToken Invoke(JToken parameters)
        {
            try
            {
                var commandParameter = parameters?.ToObject<ProcessCommandParameter>();

                commandParameter.CheckNullObject(nameof(commandParameter));
                commandParameter.CommandPath.CheckEmptyString(nameof(commandParameter.CommandPath));

                var resultBuilder = commandParameter.OutputCommandResult ? new StringBuilder() : null;
                WindowsProcessExtension.RunCommand(commandParameter.ToWindowsProcessCommandParameter(resultBuilder));

                return resultBuilder.ToJson();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }
    }
}