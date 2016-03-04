namespace Beyova
{
    /// <summary>
    /// Class HttpConstants.
    /// </summary>
    public static class HttpConstants
    {
        /// <summary>
        /// Class HttpMethod.
        /// </summary>
        public static class HttpMethod
        {
            /// <summary>
            /// The get
            /// </summary>
            public const string Get = "GET";

            /// <summary>
            /// The post
            /// </summary>
            public const string Post = "POST";

            /// <summary>
            /// The put
            /// </summary>
            public const string Put = "PUT";

            /// <summary>
            /// The delete
            /// </summary>
            public const string Delete = "DELETE";

            /// <summary>
            /// The head
            /// </summary>
            public const string Head = "HEAD";

            /// <summary>
            /// The options
            /// </summary>
            public const string Options = "OPTIONS";

            /// <summary>
            /// The trace
            /// </summary>
            public const string Trace = "TRACE";

            /// <summary>
            /// The patch
            /// </summary>
            public const string Patch = "PATCH";

            /// <summary>
            /// The lock
            /// </summary>
            public const string Lock = "LOCK";

            /// <summary>
            /// The unlock
            /// </summary>
            public const string Unlock = "UNLOCK";
        }

        /// <summary>
        /// Class HttpHeader.
        /// </summary>
        public static class HttpHeader
        {
            /// <summary>
            /// The token
            /// </summary>
            public const string TOKEN = "X-BA-TOKEN";

            /// <summary>
            /// The admin token
            /// </summary>
            public const string ADMINTOKEN = "X-BA-ADMIN-TOKEN";

            /// <summary>
            /// The secure key
            /// </summary>
            public const string SECUREKEY = "X-BA-SECURED-KEY";

            /// <summary>
            /// The client identifier
            /// </summary>
            public const string CLIENTIDENTIFIER = "X-BA-CLIENT-ID";

            /// <summary>
            /// The server exit time
            /// </summary>
            public const string SERVEREXITTIME = "X-SERVER-EXIT-UTC-TIME";

            /// <summary>
            /// The server entry time
            /// </summary>
            public const string SERVERENTRYTIME = "X-SERVER-ENTRY-UTC-TIME";

            /// <summary>
            /// </summary>
            public const string SERVERNAME = "X-SERVER-NAME";

            /// <summary>
            /// The original IP address
            /// </summary>
            public const string ORIGINAL = "X-BA-ORIGINAL";

            /// <summary>
            /// The trace ID
            /// </summary>
            public const string TRACEID = "X-BA-TRACE-ID";

            /// <summary>
            /// The content encoding
            /// </summary>
            public const string ContentEncoding = "Content-Encoding";

            /// <summary>
            /// The accept encoding
            /// </summary>
            public const string AcceptEncoding="Accept-Encoding";
        }
    }
}
