using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.ApiTrackingAnalytic.GoogleAnalytics
{
    /// <summary>
    /// Class GoogleMeasurementLog.
    /// </summary>
    public class GoogleMeasurementLog : ICloneable
    {
        /// <summary>
        /// The version
        /// </summary>
        public const string Version = "1";

        #region Property

        /// <summary>
        /// Gets or sets the tracking identifier.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#tid
        /// The tracking ID / web property ID. The format is UA-XXXX-Y. All collected data is associated by this ID.
        /// <example>
        /// Example value: UA-XXXX-Y
        /// </example></remarks>
        /// </summary>
        /// <value>The tracking identifier.</value>
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// <remarks>
        /// If it is for AppView, stands for app id (aid)
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#aid
        /// <example>
        /// Example value: com.company.app
        /// </example>
        /// If it is for PageView, stands for client id (cid)
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#cid
        /// This anonymously identifies a particular user, device, or browser instance. For the web, this is generally stored as a first-party cookie with a two-year expiration. For mobile apps, this is randomly generated for each particular instance of an application install. The value of this field should be a random UUID (version 4) as described in http://www.ietf.org/rfc/rfc4122.txt
        /// </remarks>
        /// <example>
        /// Example value: 35009a79-1a05-49d7-b876-2b884d0f825b
        /// </example>
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#uid
        /// This is intended to be a known identifier for a user provided by the site owner/tracking library user. It may not itself be PII (personally identifiable information). The value should never be persisted in GA cookies or other Analytics provided storage.
        /// </remarks>
        /// <example>
        /// Example value: as8eknlll
        /// </example>
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#uip
        /// The IP address of the user. This should be a valid IP address. It will always be anonymized just as though &amp; aip (anonymize IP) had been used.
        /// </remarks>
        /// <example>
        /// Example value: 1.2.3.4
        /// </example>
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether it is IpAddressAnonymized
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#aip
        /// When present, the IP address of the sender will be anonymized. For example, the IP will be anonymized if any of the following parameters are present in the payload: &amp;aip=,  &amp;aip=0, or  &amp;aip=1
        /// </remarks>
        /// <example>
        /// Example value: 1
        /// </example>
        /// </summary>
        /// <value>The value indicating whether it is IpAddressAnonymized.</value>
        public bool IpAddressAnonymized { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#ua
        /// The User Agent of the browser. Note that Google has libraries to identify real user agents. Hand crafting your own agent could break at any time.
        /// </remarks>
        /// <example>
        /// Example value: Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14
        /// </example>
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the user language.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#ul
        /// Specifies the language.
        /// </remarks>
        /// <example>
        /// Example value: en-us
        /// </example>
        /// </summary>
        /// <value>The user language.</value>
        public string UserLanguage { get; set; }

        /// <summary>
        /// Gets or sets the type of the hit.
        /// </summary>
        /// <value>The type of the hit.</value>
        public ApiTrackerHitType HitType { get; set; }

        /// <summary>
        /// Gets or sets the Uri
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#dh
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#dp
        /// Host + Path
        /// </remarks>
        /// <example>
        /// Example value: foo.com
        /// </example>
        /// </summary>
        /// <value>The name of the host.</value>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?#dt
        /// The title of the page / document.
        /// </remarks>
        /// <example>
        /// Example value: Settings
        /// </example>
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Link ID.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#linkid
        /// The ID of a clicked DOM element, used to disambiguate multiple links to the same URL in In-Page Analytics reports when Enhanced Link Attribution is enabled for the property.
        /// </remarks>
        /// <example>
        /// Example value: nav_bar
        /// </example>
        /// </summary>
        /// <value>The link ID.</value>
        public string LinkId { get; set; }

        #region App View

        /// <summary>
        /// Gets or sets the name of the application.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#an
        /// </remarks>
        /// </summary>
        /// <value>The name of the application.</value>
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the application version.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#av
        /// </remarks>
        /// <example>
        /// Example value: 1.2
        /// </example>
        /// </summary>
        /// <value>The application version.</value>
        public string AppVersion { get; set; }

        /// <summary>
        /// Gets or sets the application installer identifier.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#aiid
        /// </remarks>
        /// <example>
        /// Example value: com.platform.vending
        /// </example>
        /// </summary>
        /// <value>The application installer identifier.</value>
        public string AppInstallerId { get; set; }

        #endregion

        #region Event

        /// <summary>
        /// Gets or sets the event category.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#ec
        /// Specifies the event category. Must not be empty.
        /// </remarks>
        /// <example>
        /// Example value: Category
        /// </example>
        /// </summary>
        /// <value>The event category.</value>
        public string EventCategory { get; set; }

        /// <summary>
        /// Gets or sets the event action.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#ea
        /// Specifies the event action. Must not be empty.
        /// </remarks>
        /// <example>
        /// Example value: Action
        /// </example>
        /// </summary>
        /// <value>The event action.</value>
        public string EventAction { get; set; }

        /// <summary>
        /// Gets or sets the event label.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#el
        /// Specifies the event label.
        /// </remarks>
        /// <example>
        /// Example value: Label
        /// </example>
        /// </summary>
        /// <value>The event label.</value>
        public string EventLabel { get; set; }

        /// <summary>
        /// Gets or sets the event value.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#ev
        /// Specifies the event value. Values must be non-negative.
        /// </remarks>
        /// <example>
        /// Example value: 55
        /// </example>
        /// </summary>
        /// <value>The event value.</value>
        public string EventValue { get; set; }

        #endregion

        #region Social network

        /// <summary>
        /// Gets or sets the social network.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#sn
        /// Specifies the social network, for example Facebook or Google Plus.
        /// </remarks>
        /// <example>
        /// Example value: facebook
        /// </example>
        /// </summary>
        /// <value>The social network.</value>
        public string SocialNetwork { get; set; }

        /// <summary>
        /// Gets or sets the social action.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#sa
        /// Specifies the social interaction action. For example on Google Plus when a user clicks the +1 button, the social action is 'plus'.
        /// </remarks>
        /// <example>
        /// Example value: like
        /// </example>
        /// </summary>
        /// <value>The social action.</value>
        public string SocialAction { get; set; }

        /// <summary>
        /// Gets or sets the social action target.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#st
        /// Specifies the target of a social interaction. This value is typically a URL but can be any text.
        /// </remarks>
        /// <example>
        /// Example value: http://foo.com
        /// </example>
        /// </summary>
        /// <value>The social action target.</value>
        public string SocialActionTarget { get; set; }

        #endregion

        #region Timing

        /// <summary>
        /// Gets or sets the user timing category.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#utc
        /// Specifies the user timing category.
        /// </remarks>
        /// <example>
        /// Example value: category
        /// </example>
        /// </summary>
        /// <value>The user timing category.</value>
        public string UserTimingCategory { get; set; }

        /// <summary>
        /// Gets or sets the name of the user timing variable.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#utv
        /// Specifies the user timing variable.
        /// </remarks>
        /// </summary>
        /// <example>
        /// Example value: lookup
        /// </example>
        /// <value>The name of the user timing variable.</value>
        public string UserTimingVariableName { get; set; }

        /// <summary>
        /// Gets or sets the user timing time.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#utt
        /// Specifies the user timing value. The value is in milliseconds.
        /// </remarks>
        /// <example>
        /// Example value: 123
        /// </example>
        /// </summary>
        /// <value>The user timing time.</value>
        public int UserTimingTime { get; set; }

        /// <summary>
        /// Gets or sets the user timing label.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#utl
        /// Specifies the user timing label.
        /// </remarks>
        /// <example>
        /// Example value: label
        /// </example>
        /// </summary>
        /// <value>The user timing label.</value>
        public string UserTimingLabel { get; set; }

        #endregion

        #region Exception

        /// <summary>
        /// Gets or sets the exception description.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#exd
        /// Specifies the description of an exception.
        /// </remarks>
        /// <example>
        /// Example value: DatabaseError
        /// </example>
        /// </summary>
        /// <value>The exception description.</value>
        public string ExceptionDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fatal exception.
        /// <remarks>
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#exf
        /// Specifies whether the exception was fatal.
        /// </remarks>
        /// <example>
        /// Example value: 0
        /// </example>
        /// </summary>
        /// <value><c>true</c> if this instance is fatal exception; otherwise, <c>false</c>.</value>
        public bool IsFatalException { get; set; }

        #endregion

        #region Custom Dimensions / Metrics

        #endregion

        #endregion

        /// <summary>
        /// To the name value query string.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToNameValueQueryString()
        {
            var collection = ToNameValueCollection();

            StringBuilder builder = new StringBuilder();
            foreach (string key in collection.AllKeys)
            {
                builder.AppendFormat("{0}={1}&", key, collection.Get(key).ToUrlEncodedText());
            }

            return builder.ToString();
        }

        /// <summary>
        /// To the name value collection.
        /// </summary>
        /// <returns>NameValueCollection.</returns>
        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection result = new NameValueCollection();

            // Required fields
            result.Add("v", Version);
            result.Add("tid", this.TrackingId);
            result.Add("cid", this.ClientId);
            result.Add("t", this.HitType.ToString().ToLowerInvariant());

            // Common fields
            result.Add("uip", this.IpAddress);
            result.Add("uid", this.UserId);
            result.Add("ua", this.UserAgent);
            result.Add("aip", this.IpAddressAnonymized ? "1" : "0");

            // Host
            result.Add("dh", this.Uri.Host);
            result.Add("dp", this.Uri.PathAndQuery);
            result.Add("dt", this.Title);

            switch (this.HitType)
            {
                case ApiTrackerHitType.AppView:
                    result.Add("an", this.AppName);
                    result.Add("av", this.AppVersion);
                    result.Add("aiid", this.AppInstallerId);
                    break;
                case ApiTrackerHitType.PageView:
                    result.Add("linkid", this.LinkId);
                    break;
                case ApiTrackerHitType.Event:
                    result.Add("ec", this.EventCategory);
                    result.Add("ea", this.EventAction);
                    result.Add("el", this.EventLabel);
                    result.Add("ev", this.EventValue);
                    break;
                case ApiTrackerHitType.Social:
                    result.Add("sn", this.SocialNetwork);
                    result.Add("sa", this.SocialAction);
                    result.Add("st", this.SocialActionTarget);
                    break;
                case ApiTrackerHitType.Exception:
                    result.Add("exd", this.ExceptionDescription);
                    result.Add("exf", this.IsFatalException ? "1" : "0");
                    break;
                case ApiTrackerHitType.Timing:
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
