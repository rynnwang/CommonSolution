using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ifunction.Model;

namespace ifunction.WebExtension.HttpLongPolling
{
    /// <summary>
    /// Class PollingMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(BaseObject))]
    public class PollingMessage : BaseObject
    {
        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        [DataMember]
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        /// <value>The receiver.</value>
        [DataMember]
        public string Receiver { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [DataMember]
        public string Data { get; set; }

        /// <summary>
        /// Gets the sender polling identifier.
        /// </summary>
        /// <value>The sender polling identifier.</value>
        public PollingIdentifier SenderPollingIdentifier
        {
            get
            {
                return new PollingIdentifier { Identifier = this.Sender };
            }
        }

        /// <summary>
        /// Gets the receiver polling identifier.
        /// </summary>
        /// <value>The receiver polling identifier.</value>
        public PollingIdentifier ReceiverPollingIdentifier
        {
            get
            {
                return new PollingIdentifier { Identifier = this.Receiver };
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Sender) && !string.IsNullOrWhiteSpace(this.Receiver);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PollingMessage"/> class.
        /// </summary>
        public PollingMessage()
            : base()
        {

        }

    }
}
