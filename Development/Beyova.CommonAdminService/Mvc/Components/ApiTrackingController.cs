using System;
using System.Web.Mvc;
using Beyova;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class ApiTrackingBaseController.
    /// </summary>
    public abstract class ApiTrackingController<T> : EnvironmentBaseController where T : IApiAnalytics
    {
        /// <summary>
        /// The client
        /// </summary>
        protected IApiAnalytics client;

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        public abstract IApiAnalytics Client { get; }

        /// <summary>
        /// The _analytic index name
        /// </summary>
        protected string _analyticIndexName;

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>IApiAnalytics.</returns>
        protected IApiAnalytics GetClient(EnvironmentEndpoint endpoint)
        {
            return endpoint == null ? null : InitializeClient(endpoint, _analyticIndexName);
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingController" /> class.
        /// </summary>
        /// <param name="analyticIndexName">Name of the analytic index.</param>
        public ApiTrackingController(string analyticIndexName = null) : base("ApiTracking")
        {
            this._analyticIndexName = analyticIndexName.SafeToString("apitracking");
        }

        #endregion

        /// <summary>
        /// Initializes the client.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <returns>IApiAnalytics.</returns>
        protected IApiAnalytics InitializeClient(EnvironmentEndpoint endpoint, string indexName)
        {
            return new Elastic.ElasticApiTracking(string.Format("{0}://{1}:{2}", endpoint.Protocol.SafeToString("http"), endpoint.Host, endpoint.Port ?? 9200), indexName);
        }

        [HttpGet]
        public ActionResult ApiEvent()
        {
            return View(Constants.ViewNames.ApiEventPanelView);
        }

        /// <summary>
        /// APIs the event.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult ApiEvent(ApiEventCriteria criteria)
        {
            if (Client != null && criteria != null)
            {
                try
                {
                    var events = client.QueryApiEvent(criteria);
                    return View(Constants.ViewNames.ApiEventListView, events);
                }
                catch (Exception ex)
                {
                    return this.HandleExceptionToPartialView(ex, HttpConstants.HttpMethod.Post, "ApiEvent", criteria);
                }
            }

            return RedirectToNotFoundPage();
        }

        /// <summary>
        /// Exceptions the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult Exception(ExceptionCriteria criteria)
        {
            if (Client != null && criteria != null)
            {
                try
                {
                    var exceptions = client.QueryException(criteria);
                    return View(Constants.ViewNames.ExceptionListView, exceptions);
                }
                catch (Exception ex)
                {
                    return this.HandleExceptionToPartialView(ex, HttpConstants.HttpMethod.Post, "Exception", criteria);
                }
            }

            return RedirectToNotFoundPage();
        }

        /// <summary>
        /// Events the trace.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult EventTrace(string traceId)
        {
            if (Client != null && !string.IsNullOrWhiteSpace(traceId))
            {

            }

            return RedirectToNotFoundPage();
        }
    }
}