﻿
namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiEventGroupStatistic.
    /// </summary>
    public class ApiEventGroupStatistic : ApiEventLogBase, IGroupByResult
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the stamp identifier.
        /// </summary>
        /// <value>The stamp identifier.</value>
        public string StampIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventGroupStatistic"/> class.
        /// </summary>
        public ApiEventGroupStatistic()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventGroupStatistic" /> class.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public ApiEventGroupStatistic(ApiEventGroupStatistic statistic)
            : base(statistic)
        {
            if (statistic != null)
            {
                this.StampIdentifier = statistic.StampIdentifier;
                this.Count = statistic.Count;
                this.DisplayName = statistic.DisplayName;
            }
        }
    }
}