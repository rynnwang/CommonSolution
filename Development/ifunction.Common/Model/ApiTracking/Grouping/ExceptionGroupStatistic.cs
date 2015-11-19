using System.Net;

namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Class ExceptionGroupStatistic.
    /// </summary>
    public class ExceptionGroupStatistic : ExceptionStatistic, IGroupByResult
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

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
        /// Initializes a new instance of the <see cref="ExceptionGroupStatistic"/> class.
        /// </summary>
        public ExceptionGroupStatistic()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionGroupStatistic"/> class.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public ExceptionGroupStatistic(ExceptionGroupStatistic statistic)
            : base(statistic)
        {
            if (statistic != null)
            {
                this.Country = statistic.Country;
                this.City = statistic.City;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionGroupStatistic" /> class.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public ExceptionGroupStatistic(ExceptionStatistic statistic)
            : base(statistic)
        {
        }
    }
}
