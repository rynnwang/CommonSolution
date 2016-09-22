using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// Class UriRewriter.
    /// </summary>
    public class UriRewriteRule
    {
        /// <summary>
        /// The _uri regex
        /// </summary>
        private Regex _uriRegex;

        /// <summary>
        /// The _shift shard
        /// </summary>
        private StringShiftShard _shiftShard;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriRewriteRule"/> class.
        /// </summary>
        /// <param name="uriRegex">The URI regex.</param>
        /// <param name="destination">The destination.</param>
        internal UriRewriteRule(Regex uriRegex, string destination)
        {
            _uriRegex = uriRegex;
            _shiftShard = destination.ConvertFormatToShiftShard();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriRewriteRule" /> class.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="protocolConstraint">The protocol constraint.</param>
        /// <param name="hostFormat">The host format. Like: "{feature}.host.com", "{feature}.{region}.host.com" or "test.host.com"</param>
        /// <param name="hostConstraint">The host constraint. Like: "[A-Za-z0-9]+" or "[^\/]+"</param>
        /// <param name="pathSectionNames">The path section names.</param>
        public UriRewriteRule(string destination, string protocolConstraint, string hostFormat, string hostConstraint, params string[] pathSectionNames)
        {
            StringBuilder builder = new StringBuilder("{protocol}://");
            Dictionary<string, string> constraints = new Dictionary<string, string>();
            constraints.Add("protocol", protocolConstraint.SafeToString("[a-zA-Z0-9]+"));

            if (string.IsNullOrWhiteSpace(hostFormat))
            {
                builder.Append("{host}");
                constraints.Add("host", @"[^\/]+");
            }
            else
            {
                builder.Append(hostFormat);
                foreach (var one in hostFormat.FindPlaceHolders())
                {
                    constraints.Add(one, @"[0-9a-zA-Z_-]+");
                }
            }

            builder.Append("/");

            foreach (var one in pathSectionNames)
            {
                builder.AppendFormat("{{{0}}}", hostFormat);
                constraints.Merge(one, @"[0-9a-zA-Z_-]+");
                builder.Append("/");
            }

            _uriRegex = builder.ToString().ConvertFormatToRegex(constraints, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _shiftShard = destination.ConvertFormatToShiftShard();
        }

        /// <summary>
        /// Rewrites to URI.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <param name="outputUri">The output URI.</param>
        /// <returns><c>true</c> if matched, <c>false</c> otherwise.</returns>
        public bool RewriteToUri(string sourceUri, out string outputUri)
        {
            Match match = string.IsNullOrWhiteSpace(sourceUri) ? null : _uriRegex.Match(sourceUri);

            if (_shiftShard == null || match == null || !match.Success)
            {
                outputUri = string.Empty;
                return false;
            }
            else
            {
                outputUri = _shiftShard.ToString(match);
                return true;
            }
        }
    }
}
