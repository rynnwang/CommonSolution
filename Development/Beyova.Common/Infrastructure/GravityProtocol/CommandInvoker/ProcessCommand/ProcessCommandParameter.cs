using System;
using System.Text;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class ProcessCommandParameter.
    /// </summary>
    public class ProcessCommandParameter : WindowsProcessCommandParameter
    {
        /// <summary>
        /// Gets or sets a value indicating whether [output command result].
        /// </summary>
        /// <value><c>true</c> if [output command result]; otherwise, <c>false</c>.</value>
        public bool OutputCommandResult { get; set; }

        /// <summary>
        /// To the windows process command parameter.
        /// </summary>
        /// <returns>WindowsProcessCommandParameter.</returns>
        internal WindowsProcessCommandParameter ToWindowsProcessCommandParameter(StringBuilder commandOutputBuilder = null)
        {
            var result = new WindowsProcessCommandParameter
            {
                CommandArguments = this.CommandArguments,
                CommandPath = this.CommandPath
            };

            if (OutputCommandResult && commandOutputBuilder != null)
            {
                commandOutputBuilder = new StringBuilder();
                result.OutputDelegate = new Action<string>((s) => { commandOutputBuilder.AppendLine(s); });
            }

            return result;
        }
    }
}