using System;
using System.Collections.Generic;
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
    [TokenRequired]
    public abstract class ApiTrackingController<T> : EnvironmentBaseController where T : IApiAnalytics
    {
        /// <summary>
        /// The client
        /// </summary>
        protected IApiAnalytics client;

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

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>IApiAnalytics.</returns>
        protected IApiAnalytics GetClient(string environment)
        {
            return GetClient(GetEnvironmentEndpoint(environment));
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
        public ActionResult ApiEvent(ApiEventCriteria criteria, string environment)
        {
            var client = GetClient(environment);

            try
            {
                List<ApiEventLog> result = null;
                if (client != null && criteria != null)
                {
                    result = client.QueryApiEvent(criteria);
                }

                return PartialView(Constants.ViewNames.ApiEventListView, result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, HttpConstants.HttpMethod.Post, "ApiEvent", criteria);
            }
        }

        /// <summary>
        /// APIs the event detail.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ApiEventDetail(Guid? key, string environment)
        {
            var client = GetClient(environment);

            try
            {
                ApiEventLog result = null;
                if (client != null && key != null)
                {
                    result = client.QueryApiEvent(new ApiEventCriteria { Key = key }).SafeFirstOrDefault();
                }

                if (result != null)
                {
                    return View(Constants.ViewNames.ApiEventDetailView, result);
                }
                else
                {
                    return this.RedirectToNotFoundPage();
                }
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, HttpConstants.HttpMethod.Get, "ApiEventDetail", key);
            }
        }

        [HttpGet]
        public ActionResult Exception()
        {
            return View(Constants.ViewNames.ExceptionPanelView);
        }

        /// <summary>
        /// Exceptions the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult Exception(ExceptionCriteria criteria, string environment)
        {
            var client = GetClient(environment);

            try
            {
                List<ExceptionInfo> result = null;
                if (client != null && criteria != null)
                {
                    result = client.QueryException(criteria);
                }

                return PartialView(Constants.ViewNames.ExceptionListView, result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, HttpConstants.HttpMethod.Post, "Exception", criteria);
            }
        }

        [HttpGet]
        public ActionResult EventTrace()
        {
            return View(Constants.ViewNames.ApiTracePanelView);
        }

        /// <summary>
        /// Events the trace.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult EventTrace(string traceId, string environment)
        {
            var client = GetClient(environment);
            if (client != null && !string.IsNullOrWhiteSpace(traceId))
            {

            }

            return RedirectToNotFoundPage();
        }
    }
}