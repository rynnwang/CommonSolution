using System;

namespace Beyova
{
    /// <summary>
    /// Class WindowsProcessCommandParameter.
    /// </summary>
    public class WindowsProcessCommandParameter : ProcessCommandParameterBase
    {
        /// <summary>
        /// Gets or sets the output delegate.
        /// </summary>
        /// <value>The output delegate.</value>
        public Action<string> OutputDelegate { get; set; }
    }
}