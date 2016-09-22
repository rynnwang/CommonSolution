using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticWorkObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElasticWorkObject<T> : ElasticWorkObject<T, object>, IElasticWorkObject<T, object>
    {
        /// <summary>
        /// Preprocesses the data.
        /// </summary>
        public override void Preprocess()
        {
            //nothing to do.
        }
    }

    /// <summary>
    /// Class ElasticWorkObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="F"></typeparam>
    public abstract class ElasticWorkObject<T, F> : IElasticWorkObject<T, F>
    {
        /// <summary>
        /// Gets or sets the name of the index.
        /// </summary>
        /// <value>The name of the index.</value>
        public string IndexName { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T RawData { get; set; }

        /// <summary>
        /// Gets or sets the process factor.
        /// </summary>
        /// <value>The process factor.</value>
        public F ProcessFactor { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        public int? Timeout { get; set; }

        /// <summary>
        /// Preprocesses this instance.
        /// </summary>
        public abstract void Preprocess();
    }
}
