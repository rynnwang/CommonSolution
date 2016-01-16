using System;

namespace Beyova
{
    /// <summary>
    /// Class HttpMaintanenceJob.
    /// </summary>
    public class HttpMaintanenceJob : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the HTTP content raw.
        /// </summary>
        /// <value>The HTTP content raw.</value>
        public string HttpContentRaw { get; set; }

        /// <summary>
        /// Gets or sets the time pin.
        /// </summary>
        /// <value>The time pin.</value>
        public TimePin? TimePin { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}