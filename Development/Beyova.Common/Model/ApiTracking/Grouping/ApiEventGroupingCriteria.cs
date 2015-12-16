
namespace Beyova.ApiTracking.Model
{
    /// <summary>
    /// Class ApiEventGroupingCriteria.
    /// </summary>
    public class ApiEventGroupingCriteria : ApiEventStatisticCriteria
    {
        /// <summary>
        /// Gets or sets a value indicating whether [group by resource name].
        /// </summary>
        /// <value><c>true</c> if [group by resource name]; otherwise, <c>false</c>.</value>
        public bool GroupByResourceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [group by service identifier].
        /// </summary>
        /// <value><c>true</c> if [group by service identifier]; otherwise, <c>false</c>.</value>
        public bool GroupByServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [group by server identifier].
        /// </summary>
        /// <value><c>true</c> if [group by server identifier]; otherwise, <c>false</c>.</value>
        public bool GroupByServerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [group by location].
        /// </summary>
        /// <value><c>true</c> if [group by location]; otherwise, <c>false</c>.</value>
        public bool GroupByLocation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventGroupingCriteria" /> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public ApiEventGroupingCriteria(ApiEventStatisticCriteria criteria)
            : base(criteria)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEventGroupingCriteria"/> class.
        /// </summary>
        public ApiEventGroupingCriteria()
            : this(null)
        {
        }
    }
}
