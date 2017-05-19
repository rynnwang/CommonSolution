using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Interface IGravityCommandInvoker
    /// </summary>
    public interface IGravityCommandInvoker
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        string Action { get; }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>JToken.</returns>
        JToken Invoke(JToken parameters);
    }
}
