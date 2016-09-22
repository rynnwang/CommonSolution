using Beyova.ApiTracking;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class ExceptionScene.
    /// </summary>
    public class ExceptionScene
    {
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>The line number.</value>
        public int LineNumber { get; set; }
    }
}
