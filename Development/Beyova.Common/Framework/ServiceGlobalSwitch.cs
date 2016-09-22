using System;
using System.Net;
using Beyova.Gravity;

namespace Beyova
{
    /// <summary>
    /// Class ServiceGlobalSwitch.
    /// </summary>
    public class ServiceGlobalSwitch
    {
        /// <summary>
        /// Gets or sets the working status.
        /// </summary>
        /// <value>The working status.</value>
        public ComponentWorkingStatus WorkingStatus { get; set; }

        /// <summary>
        /// Gets or sets the redirect to URI.
        /// </summary>
        /// <value>The redirect to URI.</value>
        public Uri RedirectToUri { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the effected stamp.
        /// </summary>
        /// <value>The effected stamp.</value>
        public DateTime? EffectedStamp { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }
    }
}
