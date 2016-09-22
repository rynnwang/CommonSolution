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

            /// <summary>
            /// The fetch
            /// </summary>
            public const string Fetch = "FETCH";
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
            /// The trace sequence
            /// </summary>
            public const string TRACESEQUENCE = "X-BA-TRACE-SEQ";

            /// <summary>
            /// The content encoding
            /// </summary>
            public const string ContentEncoding = "Content-Encoding";

            /// <summary>
            /// The accept encoding
            /// </summary>
            public const string AcceptEncoding = "Accept-Encoding";

            /// <summary>
            /// The transfer encoding
            /// </summary>
            public const string TransferEncoding = "Transfer-Encoding";

            /// <summary>
            /// The content length
            /// </summary>
            public const string ContentLength = "Content-Length";

            /// <summary>
            /// The access control allow origin
            /// </summary>
            public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

            /// <summary>
            /// The access control allow headers
            /// </summary>
            public const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

            /// <summary>
            /// The access control allow methods
            /// </summary>
            public const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        }

        /// <summary>
        /// Class HttpProtocols.
        /// </summary>
        public static class HttpProtocols
        {
            /// <summary>
            /// The HTTP
            /// </summary>
            public const string Http = "http";

            /// <summary>
            /// The HTTP protocol
            /// </summary>
            public const string HttpProtocol = "http://";

            /// <summary>
            /// The HTTPS
            /// </summary>
            public const string Https = "https";

            /// <summary>
            /// The HTTPS protocol
            /// </summary>
            public const string HttpsProtocol = "https://";
        }

        /// <summary>
        /// Class HttpValues.
        /// </summary>
        public static class HttpValues
        {
            /// <summary>
            /// The GZIP
            /// </summary>
            public const string GZip = "gzip";

            /// <summary>
            /// The localhost
            /// </summary>
            public const string Localhost = "localhost";

            /// <summary>
            /// The localhost ip v4
            /// </summary>
            public const string LocalhostIpV4 = "127.0.0.1";

            /// <summary>
            /// The localhost ip v6
            /// </summary>
            public const string LocalhostIpV6 = "::1";
        }

        /// <summary>
        /// Class QueryString.
        /// </summary>
        public static class QueryString
        {
            /// <summary>
            /// The language
            /// </summary>
            public const string Language = "language";
        }

        /// <summary>
        /// Class ContentType.
        /// </summary>
        public static class ContentType
        {
            /// <summary>
            /// The language
            /// </summary>
            public const string Json = "application/json";

            /// <summary>
            /// The HTML
            /// </summary>
            public const string Html = "text/html";

            /// <summary>
            /// The binary
            /// </summary>
            public const string BinaryDefault = "application/octet-stream";

            /// <summary>
            /// The zip file
            /// </summary>
            public const string ZipFile = "application/zip";

            /// <summary>
            /// The PNG image
            /// </summary>
            public const string PngImage = "image/png";

            /// <summary>
            /// The JPEG image
            /// </summary>
            public const string JpegImage = "image/jpeg";

            /// <summary>
            /// The MP3 audio
            /// </summary>
            public const string Mp3Audio = "audio/mpeg";
        }
    }
}
