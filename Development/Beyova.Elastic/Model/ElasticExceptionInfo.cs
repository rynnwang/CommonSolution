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
    /// Class ElasticExceptionInfo.
    /// </summary>
    public class ElasticExceptionInfo : ExceptionBase
    {
        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the inner exception.
        /// </summary>
        /// <value>The inner exception.</value>
        public string InnerException { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public ExceptionCode Code { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the scene.
        /// </summary>
        /// <value>The scene.</value>
        public ExceptionScene Scene { get; set; }
    }
}
