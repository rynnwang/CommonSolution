using System.Text;
using Beyova;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class QueryString.
    /// </summary>
    public class QueryString
    {
        /// <summary>
        /// Gets or sets from index.
        /// </summary>
        /// <value>From index.</value>
        public int? FromIndex { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets the time zone. Example: "+08:00"
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>The order by.</value>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [order desc]. It works only when <c>OrderBy</c> is specified.
        /// </summary>
        /// <value><c>true</c> if [order desc]; otherwise, <c>false</c>.</value>
        public bool OrderDesc { get; set; }

        /// <summary>
        /// Gets or sets the query. Query value is like: (field:this OR field:that) AND field:this AND field:that
        /// </summary>
        /// <value>The query.</value>
        public string Query { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Query))
            {
                builder.AppendFormat("q={0}&", Query.ToUrlEncodedText());
            }

            if (this.Count != null && this.Count.Value > 0)
            {
                builder.AppendFormat("size={0}&", this.Count.Value);
            }

            if (this.FromIndex != null && this.FromIndex.Value > 0)
            {
                builder.AppendFormat("from={0}&", this.FromIndex.Value);
            }

            if (!string.IsNullOrWhiteSpace(this.TimeZone))
            {
                builder.AppendFormat("time_zone={0}&", this.TimeZone.ToUrlEncodedText());
            }

            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                builder.AppendFormat("sort={0}%3A{1}&", this.OrderBy, this.OrderDesc ? "desc" : "asc");
            }

            return builder.ToString().TrimEnd('&');
        }
    }
}
