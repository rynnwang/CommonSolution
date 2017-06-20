using System;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiEventGroupStatistic.
    /// </summary>
    public class ApiEventGroupStatistic : GroupStatistic, IGroupByResult
    {
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
        /// Gets or sets the stamp identifier.
        /// </summary>
        /// <value>
        /// The stamp identifier.
        /// </value>
        public DateTime? StampIdentifier { get; set; }

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
        /// Initializes a new instance of the <see cref="ApiEventGroupStatistic" /> class.
        /// </summary>
        public ApiEventGroupStatistic() : base()
        {
        }
    }
}