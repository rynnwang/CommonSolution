namespace Beyova.ApiTracking.Model
{
    /// <summary>
    /// Class ApiEventGroupStatistic.
    /// </summary>
    public class ApiEventGroupStatistic : ApiEventStatistic, IGroupByResult
    {
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
        /// Initializes a new instance of the <see cref="ApiEventGroupStatistic"/> class.
        /// </summary>
        public ApiEventGroupStatistic()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventGroupStatistic"/> class.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public ApiEventGroupStatistic(ApiEventGroupStatistic statistic)
            : base(statistic)
        {
            if (statistic != null)
            {
                this.Country = statistic.Country;
                this.City = statistic.City;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventGroupStatistic" /> class.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public ApiEventGroupStatistic(ApiEventStatistic statistic)
            : base(statistic)
        {
        }
    }
}
