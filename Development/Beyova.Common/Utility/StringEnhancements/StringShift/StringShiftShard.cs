using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// Class StringShiftShard. This class cannot be inherited.
    /// </summary>
    public sealed class StringShiftShard
    {
        // Description:
        // Combine rules: Starts and Ends with static shards.
        // Like: {static[0]} + {dynamic[0]} + {static[1]} + {dynamic[1]} + ... + {static[i]} + {dynamic[i]} + ... + {static[n]}

        private const string placeHolderFormat = "{{{0}}}";

        /// <summary>
        /// The static shards
        /// </summary>
        internal Collection<string> staticShards = new Collection<string>();

        /// <summary>
        /// The dynamic shards
        /// </summary>
        internal Collection<string> dynamicShards = new Collection<string>();

        /// <summary>
        /// Gets the static shards.
        /// </summary>
        /// <value>The static shards.</value>
        public ReadOnlyCollection<string> StaticShards { get { return new ReadOnlyCollection<string>(staticShards); } }

        /// <summary>
        /// Gets the dynamic shards.
        /// </summary>
        /// <value>The dynamic shards.</value>
        public ReadOnlyCollection<string> DynamicShards { get { return new ReadOnlyCollection<string>(dynamicShards); } }

        /// <summary>
        /// Gets the placeholders.
        /// </summary>
        /// <value>The placeholders.</value>
        public ICollection<string> Placeholders { get { return new Collection<string>(dynamicShards); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringShiftShard"/> class.
        /// </summary>
        internal StringShiftShard()
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(IDictionary<string, string> values)
        {
            if (!IsValid)
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(values), values);
            }

            StringBuilder builder = this.CreateStringBuilder();

            values = values ?? new Dictionary<string, string>();

            for (var i = 0; i < dynamicShards.Count; i++)
            {
                string replacedValue;
                builder.Append(staticShards[i]);
                builder.Append(values.TryGetValue(dynamicShards[i], out replacedValue) ? replacedValue : string.Empty);
            }

            builder.Append(staticShards[dynamicShards.Count]);

            return builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString((from x in dynamicShards select new KeyValuePair<string, string>(x, string.Format(placeHolderFormat, x))).ToDictionary());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        internal string ToString(Match match)
        {
            if (match == null || !match.Success)
            {
                return string.Empty;
            }

            Dictionary<string, string> data = new Dictionary<string, string>();

            foreach (var one in this.dynamicShards)
            {
                data.Add(one, match.Result(string.Format(placeHolderFormat, one)));
            }

            return ToString(data);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        internal bool IsValid
        {
            get
            {
                return (dynamicShards.Count == 0 && staticShards.Count == 0) || (staticShards.Count == (dynamicShards.Count + 1));
            }
        }
    }
}