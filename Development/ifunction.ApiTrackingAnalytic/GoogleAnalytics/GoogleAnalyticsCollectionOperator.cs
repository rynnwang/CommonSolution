using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.Configuration;
using ifunction.RestfulApi;

namespace ifunction.ApiTrackingAnalytic.GoogleAnalytics
{
    /// <summary>
    /// Class GoogleAnalyticsCollectionOperator.
    /// </summary>
    public class GoogleAnalyticsCollectionOperator : IApiTracking
    {
        /// <summary>
        /// The endpoint URL
        /// </summary>
        protected const string EndpointUrl = "https://ssl.google-analytics.com/collect";

        /// <summary>
        /// The google analytics version
        /// </summary>
        protected const string GoogleAnalyticsVersion = "1";

        #region Property

        /// <summary>
        /// Gets or sets the tracking identifier.
        /// </summary>
        /// <value>The tracking identifier.</value>
        public string TrackingId { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAnalyticsCollectionOperator" /> class.
        /// </summary>
        /// <param name="trackingId">The tracking identifier.
        /// <example>
        /// UA-XXXXX-Y
        /// </example></param>
        public GoogleAnalyticsCollectionOperator(string trackingId)
        {
            this.TrackingId = trackingId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAnalyticsCollectionOperator"/> class.
        /// </summary>
        public GoogleAnalyticsCollectionOperator()
            : this(Framework.GetConfiguration("GoogleTrackingId"))
        {
        }

        #endregion

        /// <summary>
        /// Creates the HTTP request.
        /// </summary>
        /// <param name="measurementLog">The measurement log.</param>
        /// <returns>HttpWebRequest.</returns>
        protected HttpWebRequest CreateHttpRequest(GoogleMeasurementLog measurementLog)
        {
            var httpRequest = EndpointUrl.CreateHttpWebRequest();
            FillHttpRequest(httpRequest, measurementLog);

            return httpRequest;
        }

        /// <summary>
        /// Fills the HTTP request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="measurementLog">The measurement log.</param>
        protected void FillHttpRequest(HttpWebRequest httpRequest, GoogleMeasurementLog measurementLog)
        {
            if (httpRequest != null && measurementLog != null)
            {
                httpRequest.FillData("POST", measurementLog.ToNameValueQueryString());
            }
        }

        /// <summary>
        /// Creates the google measurement log.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="language">The language.</param>
        /// <param name="HitType">Type of the hit.</param>
        /// <returns>GoogleMeasurementLog.</returns>
        public GoogleMeasurementLog CreateGoogleMeasurementLog(string clientId, string userId, string userAgent, string ipAddress, string language, ApiTrackerHitType HitType)
        {
            return new GoogleMeasurementLog
                  {
                      TrackingId = this.TrackingId,
                      ClientId = clientId,
                      UserId = userId,
                      UserAgent = userAgent,
                      IpAddress = ipAddress,
                      UserLanguage = language,
                      HitType = HitType
                  };
        }

        /// <summary>
        /// Commits the log.
        /// </summary>
        /// <param name="log">The log.</param>
        public void CommitLog(GoogleMeasurementLog log)
        {
            if (log != null)
            {
                try
                {
                    var httpRequest = CreateHttpRequest(log);
                    HttpStatusCode statusCode;
                    var result = httpRequest.ReadResponseAsText(Encoding.UTF8, out statusCode);
                }
                catch (Exception ex)
                {
                    ex.Handle("CommitLog", log);
                }
            }
        }

        /// <summary>
        /// Logs the API event.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public Guid? LogApiEvent(ApiEventLog eventLog)
        {
            if (eventLog != null)
            {
                var log = this.CreateGoogleMeasurementLog(eventLog.ClientIdentifier, eventLog.UserIdentifier, eventLog.UserAgent, eventLog.IpAddress, eventLog.Language, ApiTrackerHitType.Event);

                log.EventAction = eventLog.Action;
                log.EventValue = eventLog.ResourceKey;
                log.EventCategory = eventLog.ResourceName;
                if (eventLog.ExitStamp != null && eventLog.EntryStamp != null)
                {
                    log.UserTimingTime = (int)((eventLog.ExitStamp.Value - eventLog.EntryStamp.Value).TotalMilliseconds);
                }

                CommitLog(log);
                return eventLog.Key;
            }

            return null;
        }


        public Guid? LogException(BaseException exception, string serviceIdentifier = null, string serverIdentifier = null)
        {
            throw new NotImplementedException();
        }


        public void LogApiTraceLog(ApiTraceLog traceLog)
        {
            throw new NotImplementedException();
        }
    }
}
