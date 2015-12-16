using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.WebExtension.HttpLongPolling
{
    /// <summary>
    /// Class HeaderKeys.
    /// </summary>
    public static class HeaderKeys
    {
        /// <summary>
        /// The header key_ from
        /// </summary>
        public const string headerKey_From = "LPS-FROM";

        /// <summary>
        /// The header key_ to
        /// </summary>
        public const string headerKey_To = "LPS-TO";

        /// <summary>
        /// The header key_ action
        /// </summary>
        public const string headerKey_Action = "LPS-ACTION";

        /// <summary>
        /// The header key_ batch size
        /// </summary>
        public const string headerKey_BatchSize = "BATCH-SIZE";

        /// <summary>
        /// The header key_ authentication
        /// </summary>
        public const string headerKey_Authentication = "Authentication";
    }
}
