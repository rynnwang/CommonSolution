using System.Diagnostics;

namespace Beyova
{
    /// <summary>
    /// Class ProcessCommandParameterBase.
    /// </summary>
    public abstract class ProcessCommandParameterBase
    {
        /// <summary>
        /// Gets or sets the command path.
        /// </summary>
        /// <value>The command path.</value>
        public string CommandPath { get; set; }

        /// <summary>
        /// Gets or sets the command arguments.
        /// </summary>
        /// <value>The command arguments.</value>
        public string CommandArguments { get; set; }

        /// <summary>
        /// Fills the process start information.
        /// </summary>
        /// <param name="processStartInfo">The process start information.</param>
        public void FillProcessStartInfo(ProcessStartInfo processStartInfo)
        {
            if (processStartInfo != null)
            {
                processStartInfo.Arguments = CommandArguments;
                processStartInfo.FileName = CommandPath;
            }
        }
    }
}