using System.Net;
using System.Text;

namespace Beyova.Diagnostic
{
    /// <summary>
    ///
    /// </summary>
    public class DebugInfo
    {
        /// <summary>
        /// The builder
        /// </summary>
        protected StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public string Messages
        {
            get { return builder.ToString(); }
            set { builder = new StringBuilder(value.SafeToString()); }
        }

        /// <summary>
        /// Gets or sets the HTTP request raw.
        /// </summary>
        /// <value>
        /// The HTTP request raw.
        /// </value>
        public string HttpRequestRaw { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response raw.
        /// </summary>
        /// <value>
        /// The HTTP response raw.
        /// </value>
        public string HttpResponseRaw { get; set; }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void WriteLine(string format, params object[] args)
        {
            format = format == null ? string.Empty : format;
            if (args.HasItem())
            {
                builder.AppendLineWithFormat(format, args);
            }
            else
            {
                builder.AppendLine(format);
            }
        }

        /// <summary>
        /// Writes the HTTP response raw.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="body">The body.</param>
        public void WriteHttpResponseRaw(WebHeaderCollection headers, string body)
        {
            StringBuilder builder = new StringBuilder(512);

            builder.FillHeaders(headers);

            builder.AppendLine();
            builder.AppendLine(body);

            this.HttpResponseRaw = builder.ToString();
        }
    }
}