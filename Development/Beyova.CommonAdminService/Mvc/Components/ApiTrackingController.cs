using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beyova;
using Beyova.Api;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class ApiTrackingBaseController.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected override string GetViewFullPath(string viewName)
        {
            return string.Format(Constants.ViewNames.BeyovaComponentDefaultViewPath, "ApiTracking", viewName);
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
        /// Initializes a new instance of the <see cref="ApiTrackingController{T}" /> class.
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
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <returns>IApiAnalytics.</returns>
        protected IApiAnalytics InitializeClient(EnvironmentEndpoint endpoint, string indexName)
        {
            return new Elastic.ElasticApiTracking(endpoint, indexName);
        }

        /// <summary>
        /// APIs the event.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ApiEvent()
        {
            return View(GetViewFullPath(Constants.ViewNames.ApiEventPanelView));
        }

        /// <summary>
        /// APIs the event.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult ApiEvent(ApiEventCriteria criteria, string environment)
        {
            try
            {
                var client = GetClient(environment);

                List<ApiEventLog> result = null;
                if (client != null && criteria != null)
                {
                    result = client.QueryApiEvent(criteria);
                }

                return PartialView(GetViewFullPath(Constants.ViewNames.ApiEventListView), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
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
            try
            {
                var client = GetClient(environment);

                ApiEventLog result = null;
                if (client != null && key != null)
                {
                    result = client.QueryApiEvent(new ApiEventCriteria { Key = key, Count = 1 }).SafeFirstOrDefault();
                }

                if (result != null)
                {
                    return View(GetViewFullPath(Constants.ViewNames.ApiEventDetailView), result);
                }
                else
                {
                    return this.RenderAsNotFoundPage();
                }
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, key);
            }
        }

        /// <summary>
        /// Exceptions this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Exception()
        {
            return View(GetViewFullPath(Constants.ViewNames.ExceptionPanelView));
        }

        /// <summary>
        /// Exceptions the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult Exception(ExceptionCriteria criteria, string environment)
        {
            try
            {
                var client = GetClient(environment);

                List<ExceptionInfo> result = null;
                if (client != null && criteria != null)
                {
                    result = client.QueryException(criteria);
                }

                return PartialView(GetViewFullPath(Constants.ViewNames.ExceptionListView), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, criteria);
            }
        }

        /// <summary>
        /// Exceptions the detail.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ExceptionDetail(Guid? key, string environment)
        {
            try
            {
                var client = GetClient(environment);

                ExceptionInfo result = null;
                if (client != null && key != null)
                {
                    result = client.QueryException(new ExceptionCriteria { Key = key, Count = 1 }).SafeFirstOrDefault();
                }

                if (result != null)
                {
                    return View(GetViewFullPath(Constants.ViewNames.ExceptionDetailView), result);
                }
                else
                {
                    return this.RenderAsNotFoundPage();
                }
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToRedirection(ex, key);
            }
        }

        /// <summary>
        /// Events the trace.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult EventTrace()
        {
            return View(GetViewFullPath(Constants.ViewNames.ApiTracePanelView));
        }

        /// <summary>
        /// Events the trace.
        /// </summary>
        /// <param name="traceId">The trace identifier.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult EventTrace(string traceId, string environment)
        {
            try
            {
                var client = GetClient(environment);

                List<ApiTraceLog> result = null;
                if (client != null && !string.IsNullOrWhiteSpace(traceId))
                {
                    result = client.GetApiTraceLogById(traceId);
                }

                return PartialView(GetViewFullPath(Constants.ViewNames.ApiTraceDetailView), result);
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, traceId);
            }
        }

        /// <summary>
        /// APIs the event statistic.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ApiEventStatistic()
        {
            return View(GetViewFullPath(Constants.ViewNames.ApiEventStatisticPanel));
        }

        /// <summary>
        /// Exceptions the statistic.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult ExceptionStatistic()
        {
            return View(GetViewFullPath(Constants.ViewNames.ApiExceptionStatisticPanel));
        }

        /// <summary>
        /// APIs the event statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult ApiEventStatistic(ApiEventStatisticCriteria criteria, string environment)
        {
            try
            {
                var client = GetClient(environment);
                criteria.HasException = null;
                var events = client.GetApiEventStatistic(criteria);
                criteria.HasException = true;
                var eventsWithException = client.GetApiEventStatistic(criteria);

                return PartialView(GetViewFullPath(Constants.ViewNames.ApiEventStatisticLineBarMixedChart), new { eventsWithException, events });
            }
            catch (Exception ex)
            {
                return this.HandleExceptionToPartialView(ex, new { criteria, environment });
            }
        }
    }
}