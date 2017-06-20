using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandResult.
    /// </summary>
    public class GravityCommandResult : GravityCommandResultBase
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public JToken Content { get; set; }
    }
}