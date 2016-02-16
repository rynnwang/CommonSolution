using System;
using System.Net;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class HttpOperationException.
    /// </summary>
    public class HttpOperationException : BaseException
    {
        /// <summary>
        /// Class HttpExceptionReference.
        /// </summary>
        public class HttpExceptionReference
        {
            /// <summary>
            /// Gets or sets the destination URL.
            /// </summary>
            /// <value>The destination URL.</value>
            public string DestinationUrl { get; set; }

            /// <summary>
            /// Gets or sets the HTTP method.
            /// </summary>
            /// <value>The HTTP method.</value>
            public string HttpMethod { get; set; }

            /// <summary>
            /// Gets or sets the length of the body.
            /// </summary>
            /// <value>The length of the body.</value>
            public long? BodyLength { get; set; }

            /// <summary>
            /// Gets or sets the web exception status.
            /// </summary>
            /// <value>The web exception status.</value>
            public WebExceptionStatus? WebExceptionStatus { get; set; }

            /// <summary>
            /// Gets or sets the response text.
            /// </summary>
            /// <value>The response text.</value>
            public string ResponseText { get; set; }

            /// <summary>
            /// Gets or sets the HTTP status code.
            /// </summary>
            /// <value>The HTTP status code.</value>
            public HttpStatusCode HttpStatusCode { get; set; }
        }

        /// <summary>
        /// Gets the exception reference.
        /// </summary>
        /// <value>The exception reference.</value>
        public HttpExceptionReference ExceptionReference { get { return this.ReferenceData as HttpExceptionReference; } }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException" /> class. For broken requests.
        /// </summary>
        /// <param name="destinationUrl">The destination URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="message">The message.</param>
        /// <param name="bodyLength">Length of the body.</param>
        /// <param name="responseText">The response text.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="webExceptionStatus">The web exception status.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        public HttpOperationException(string destinationUrl, string httpMethod, string message, long? bodyLength, string responseText, HttpStatusCode httpStatusCode, WebExceptionStatus webExceptionStatus, string serverIdentifier = null, string operatorIdentifier = null)
            : base(string.Format("Failed to request destination URL [{0}] using method [{1}]. Responed within code [{2}], status [{3}], message: [{4}]. [{5}]", destinationUrl, httpMethod, (int)httpStatusCode, webExceptionStatus.ToString(), message, string.IsNullOrWhiteSpace(serverIdentifier) ? string.Empty : "Machine Name: " + serverIdentifier),
                  httpStatusCode.ConvertHttpStatusCodeToExceptionCode(webExceptionStatus), innerException: null, operatorIdentifier: operatorIdentifier,
                  parameterData: new HttpExceptionReference
                  {
                      DestinationUrl = destinationUrl,
                      HttpMethod = httpMethod,
                      BodyLength = bodyLength,
                      WebExceptionStatus = webExceptionStatus,
                      ResponseText = responseText,
                      HttpStatusCode = httpStatusCode
                  })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpOperationException" /> class.
        /// </summary>
        /// <param name="destinationUrl">The destination URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="message">The message.</param>
        /// <param name="bodyLength">Length of the body.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="serverIdentifier">The server identifier.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        public HttpOperationException(string destinationUrl, string httpMethod, long? bodyLength, HttpStatusCode httpStatusCode, BaseException innerException, string serverIdentifier = null, string operatorIdentifier = null)
                    : base(string.Format("Failed to request destination URL [{0}] using method [{1}]. Responed within code [{2}]. [{3}]", destinationUrl, httpMethod, (int)httpStatusCode, string.IsNullOrWhiteSpace(serverIdentifier) ? string.Empty : "Machine Name: " + serverIdentifier),
                          new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure, Minor = innerException?.Code.Minor }, innerException: innerException, operatorIdentifier: operatorIdentifier,
                          parameterData: new HttpExceptionReference
                          {
                              DestinationUrl = destinationUrl,
                              HttpMethod = httpMethod,
                              BodyLength = bodyLength,
                              WebExceptionStatus = null,
                              ResponseText = null,
                              HttpStatusCode = httpStatusCode
                          })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"/> class. For restore from <c>ExceptionInfo</c>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exceptionCode">The exception code.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="operatorIdentifier">The operator identifier.</param>
        internal HttpOperationException(string message, ExceptionCode exceptionCode, HttpExceptionReference reference, string operatorIdentifier = null)
            : base(message, exceptionCode, innerException: null, operatorIdentifier: operatorIdentifier, parameterData: reference)
        {
        }

        #endregion
    }
}
