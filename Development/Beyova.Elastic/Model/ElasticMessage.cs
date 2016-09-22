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
    /// Class ElasticMessage.
    /// </summary>
    public class ElasticMessage : IIdentifier
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticMessage"/> class.
        /// </summary>
        public ElasticMessage()
        {
            this.Key = Guid.NewGuid();
            this.CreatedStamp = DateTime.UtcNow;
        }
    }
}
