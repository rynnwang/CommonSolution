using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Elastic
{
    /// <summary>
    /// Interface IElasticWorkObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="F"></typeparam>
    public interface IElasticWorkObject<T, F>
    {
        /// <summary>
        /// Gets or sets the name of the index.
        /// </summary>
        /// <value>The name of the index.</value>
        string IndexName { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        T RawData { get; }

        /// <summary>
        /// Gets the process factor.
        /// </summary>
        /// <value>The process factor.</value>
        F ProcessFactor { get; }

        /// <summary>
        /// Preprocesses this instance.
        /// </summary>
        void Preprocess();

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        int? Timeout { get; set; }
    }
}
