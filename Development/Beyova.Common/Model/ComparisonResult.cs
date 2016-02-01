using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class ComparisonResult.
    /// </summary>
    public class ComparisonResult
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the value1.
        /// </summary>
        /// <value>The value1.</value>
        public object Value1 { get; set; }

        /// <summary>
        /// Gets or sets the value2.
        /// </summary>
        /// <value>The value2.</value>
        public object Value2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonResult"/> class.
        /// </summary>
        public ComparisonResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonResult"/> class.
        /// </summary>
        /// <param name="path">The last identifier.</param>
        /// <param name="identifier">The identifier.</param>
        public ComparisonResult(string path, string identifier) : this()
        {
            Path = string.Format("{0}{1}", path, identifier);
            Identifier = identifier.SafeToString();
        }
    }
}
