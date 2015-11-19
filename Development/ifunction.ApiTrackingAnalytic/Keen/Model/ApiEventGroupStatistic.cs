using System.Net;
using ifunction.KeenSDK.Model;

namespace ifunction.ApiTrackingAnalytic.KeenAnalytics
{
    /// <summary>
    /// Class ApiEventGroupStatistic.
    /// </summary>
    public class ApiEventGroupStatistic : IGroupByResult
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the resource key.
        /// </summary>
        /// <value>The resource key.</value>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        public HttpStatusCode? ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>The service identifier.</value>
        public string ServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// </summary>
        /// <value>The server identifier.</value>
        public string ServerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        public string AppIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the stamp identifier.
        /// </summary>
        /// <value>The stamp identifier.</value>
        public string StampIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }
}
