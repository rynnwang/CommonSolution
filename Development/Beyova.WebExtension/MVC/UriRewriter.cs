using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Beyova
{
    /// <summary>
    /// Class UriRewriter.
    /// </summary>
    public sealed class UriRewriter
    {
        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <value>The rules.</value>
        public IReadOnlyCollection<UriRewriteRule> Rules { get { return _rules.AsReadOnly(); } }

        /// <summary>
        /// The _rules
        /// </summary>
        private List<UriRewriteRule> _rules = new List<UriRewriteRule>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UriRewriter"/> class.
        /// </summary>
        public UriRewriter()
        {
        }

        /// <summary>
        /// Rewrites to URI.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <param name="outputUri">The output URI.</param>
        /// <returns><c>true</c> if matched to rewrite to uri, <c>false</c> otherwise.</returns>
        public bool RewriteToUri(string sourceUri, out string outputUri)
        {
            outputUri = null;

            if (!string.IsNullOrWhiteSpace(sourceUri))
            {
                foreach (var one in this.Rules)
                {
                    if (one.RewriteToUri(sourceUri, out outputUri))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Rewrites the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public void Rewrite(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                string outputUri = null;

                if (RewriteToUri(httpContext.Request.RawUrl, out outputUri))
                {
                    httpContext.RewritePath(outputUri);
                }
            }
        }

        /// <summary>
        /// Redirects the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns><c>true</c> if matched, <c>false</c> otherwise.</returns>
        public bool Redirect(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                string outputUri = null;

                if (RewriteToUri(httpContext.Request.RawUrl, out outputUri))
                {
                    httpContext.Response.Redirect(outputUri);
                    return true;
                }
            }

            return false;
        }
    }
}
