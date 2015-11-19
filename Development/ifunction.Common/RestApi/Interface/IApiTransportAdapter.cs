using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace ifunction.RestApi
{
    /// <summary>
    /// Interface IApiTransportAdapter
    /// </summary>
    public interface IApiTransportAdapter
    {
        /// <summary>
        /// Gets the destination host.
        /// </summary>
        /// <value>The destination host.</value>
        string DestinationHost { get; }

        /// <summary>
        /// Rewrites the header.
        /// </summary>
        /// <param name="destinationHeaderContainer">The destination header container.</param>
        /// <param name="sourceHeaderContainer">The source header container.</param>
        /// <returns></returns>
        Exception RewriteHeader(NameValueCollection destinationHeaderContainer, NameValueCollection sourceHeaderContainer);

        /// <summary>
        /// Prepares the interaction.
        /// </summary>
        /// <param name="executingMethodInfo">The executing method information.</param>
        /// <param name="executingInstance">The executing instance.</param>
        /// <param name="inputParameter">The input parameter.</param>
        void PrepareInteraction(MethodInfo executingMethodInfo, object executingInstance, object inputParameter);
    }
}
